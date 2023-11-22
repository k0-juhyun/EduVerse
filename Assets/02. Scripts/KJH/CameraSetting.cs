using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraSetting : MonoBehaviourPun
{
    [Header("카메라")]
    public Camera TPS_Camera;
    public Camera FPS_Camera;

    [Header("위치 값")]
    public Transform targetTransform;
    public Transform cameraPivotTransform;
    private Transform originTransform;

    private Vector3 cameraFollowVelo = Vector3.zero;
    private Vector2 previousMousePosition;

    private GameObject Character;

    [Space(10)]
    [Header("속도 값")]
    public float rotSpeed;
    public float scrollSpeed;
    public float dragSpeed;
    private float xRotate;
    private float yRotate;
    private float originFieldOfView;

    // 불리언 변수
    private bool isDragging = false;

    // 터치 가능한 부분
    private Rect touchZone;

    // 컴포넌트 들
    private CharacterInteraction characterInteraction;
    public TeacherInteraction teacherInteraction;
    private Customization customization;

    public float lerpCameraSpeed=0.2f;

    private void Awake()
    {
        originTransform = transform;
        originFieldOfView = Camera.main.fieldOfView;

        // 화면의 특정 부분을 터치 영역으로 지정합니다. (예시로 화면의 중앙 200x200 영역)
        touchZone = new Rect((Screen.width - 200) / 2, (Screen.height - 200) / 2, 200, 200);

        // 부모에서 컴포넌트 취득
        characterInteraction = GetComponentInParent<CharacterInteraction>();
        customization = FindAnyObjectByType<Customization?>();
    }

    private void FixedUpdate()
    {
        //UpdateCamera();
        FollowCamera();
    }

    private void LateUpdate()
    {
        HandleInput();
    }

    // 카메라가 target 오브젝트를 따라다님
    private void FollowCamera()
    {
        if (characterInteraction.isTPSCam)
        {
            
            Vector3 targetPos = Vector3.Lerp(originTransform.position, targetTransform.position, Time.deltaTime / lerpCameraSpeed);
            originTransform.position = targetPos;
        }
    }

    //private void UpdateCamera()
    //{
    //    if (photonView.IsMine)
    //    {
    //        TPS_Camera.gameObject.SetActive(!characterInteraction.isDrawing);
    //    }
    //}

    // 입력 처리
    private void HandleInput()
    {
        if (AnyInteractableIsDragging())
        {
            return;
        }
        
        if (characterInteraction.isTPSCam)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                //if (IsTouchInsideScrollView(touch.position))
                //    return;

                // 터치 영역 내에서만 터치 이벤트를 처리합니다.
                if (touchZone.Contains(touch.position))
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        // 터치 드래그로 회전
                        if (isDragging)
                        {
                            Vector2 delta = (Vector2)Input.mousePosition - previousMousePosition;
                            xRotate += delta.x * rotSpeed;

                            float yRotate = cameraPivotTransform.localEulerAngles.x - delta.y * rotSpeed;
                            yRotate = Mathf.Clamp(yRotate, -30f, 30f); // y축 회전 각도 제한

                            // x값 제한
                            xRotate = Mathf.Clamp(xRotate, -20f, 0f);

                            cameraPivotTransform.localEulerAngles = new Vector3(yRotate, xRotate, 0);
                            previousMousePosition = (Vector2)Input.mousePosition;
                        }
                    }
                    else if (touch.phase == TouchPhase.Began)
                    {
                        // 터치 시작
                        if (touch.phase == TouchPhase.Began && touch.tapCount == 1)
                        {
                            isDragging = true;
                            previousMousePosition = touch.position;
                        }
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        // 터치 종료
                        isDragging = false;
                    }

                    // 터치 확대 및 축소
                    //if (Input.touchCount == 2)
                    //{
                    //    Touch touchZero = Input.GetTouch(0);
                    //    Touch touchOne = Input.GetTouch(1);

                    //    Vector2 touchZeroPreviousPos = touchZero.position - touchZero.deltaPosition;
                    //    Vector2 touchOnePreviousPos = touchOne.position - touchOne.deltaPosition;

                    //    float prevMagnitude = (touchZeroPreviousPos - touchOnePreviousPos).magnitude;
                    //    float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                    //    float difference = currentMagnitude - prevMagnitude;
                    //    //ZoomCamera(difference * 0.01f);
                    //}
                }
            }

            // 마우스 입력 처리
            if (Input.GetMouseButtonDown(0))
            {
                if (touchZone.Contains(Input.mousePosition))
                {
                    isDragging = true;
                    previousMousePosition = Input.mousePosition;
                }
            }

            // 카메라 드래그 후 손을 뗐을 때
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            if (isDragging && teacherInteraction.isSpawnBtnClick == false)
            {
                Vector2 delta = (Vector2)Input.mousePosition - previousMousePosition;

                xRotate += delta.x * rotSpeed;
                yRotate -= delta.y * rotSpeed;
                yRotate = Mathf.Clamp(yRotate, -30, 30);
                    
                cameraPivotTransform.localEulerAngles = new Vector3(yRotate, xRotate, 0);
                //cameraPivotTransform.localEulerAngles = new Vector3(0, xRotate, 0);

                previousMousePosition = (Vector2)Input.mousePosition;
            }

            // 마우스 스크롤로 확대 및 축소
            //float scroll = Input.GetAxis("Mouse ScrollWheel");
            //ZoomCamera(scroll);
        }

        else
            FPS_Camera.gameObject.transform.forward = targetTransform.transform.forward;
    }

    public void ToggleCameraMode(bool isTPSCam, bool isSitting)
    {
        if (isSitting)
        {
            // 앉아 있을 때의 카메라 설정
            TPS_Camera.gameObject.SetActive(false);
            FPS_Camera.gameObject.SetActive(true);
            FPS_Camera.depth = 1;
        }
        else
        {
            // 서 있을 때의 카메라 설정
            TPS_Camera.gameObject.SetActive(isTPSCam);
            FPS_Camera.gameObject.SetActive(!isTPSCam);
            FPS_Camera.depth = isTPSCam ? -1 : 1;
        }
    }

    private bool AnyInteractableIsDragging()
    {
        InteractableModel[] interactables = FindObjectsOfType<InteractableModel>();
        foreach (var interactable in interactables)
        {
            if (interactable.IsDragging())
            {
                return true;
            }
        }
        return false;
    }
}