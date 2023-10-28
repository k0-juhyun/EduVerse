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
    private float originFieldOfView;

    // 불리언 변수
    private bool isDragging = false;

    // 터치 가능한 부분
    private Rect touchZone;

    // 컴포넌트 들
    private CharacterInteraction characterInteraction;

    private void Awake()
    {
        originTransform = transform;
        originFieldOfView = Camera.main.fieldOfView;

        // 화면의 특정 부분을 터치 영역으로 지정합니다. (예시로 화면의 중앙 200x200 영역)
        touchZone = new Rect((Screen.width - 200) / 2, (Screen.height - 200) / 2, 200, 200);

        // 부모에서 컴포넌트 취득
        characterInteraction = GetComponentInParent<CharacterInteraction>();

    }

    private void LateUpdate()
    {
        UpdateCamera();
        HandleInput();
        FollowCamera();
    }

    // 카메라가 target 오브젝트를 따라다님
    private void FollowCamera()
    {
        if (characterInteraction.isTPSCam)
        {
            Vector3 targetPos = Vector3.Lerp(originTransform.position, targetTransform.position, Time.deltaTime / 0.2f);
            originTransform.position = targetPos;
        }
    }

    private void UpdateCamera()
    {
        if (photonView.IsMine)
        {
            TPS_Camera.gameObject.SetActive(!characterInteraction.isDrawing);
        }
    }

    // 입력 처리
    private void HandleInput()
    {
        if (characterInteraction.isTPSCam)
        {
            HandleTouchInput();
            HandleMouseInput();
            HandleMouseScroll();
        }
        else
        {
            FPS_Camera.transform.forward = targetTransform.transform.forward;
        }
    }

    // 터치 입력 처리
    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touchZone.Contains(touch.position))
            {
                switch (touch.phase)
                {
                    case TouchPhase.Moved:
                        DragCamera(touch.position);
                        break;
                    case TouchPhase.Began:
                        StartDragging(touch.position);
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        StopDragging();
                        break;
                }
            }
        }
    }

    // 마우스 입력 처리
    private void HandleMouseInput()
    {
        if (touchZone.Contains(Input.mousePosition))
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartDragging(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                StopDragging();
            }
        }

        if (isDragging)
        {
            DragCamera(Input.mousePosition);
        }
    }

    // 카메라 드래그
    private void DragCamera(Vector2 currentMousePosition)
    {
        Vector2 delta = currentMousePosition - previousMousePosition;
        xRotate += delta.x * rotSpeed;
        cameraPivotTransform.localEulerAngles = new Vector3(0, xRotate, 0);
        previousMousePosition = currentMousePosition;
    }

    // 드래그 시작
    private void StartDragging(Vector2 mousePosition)
    {
        isDragging = true;
        previousMousePosition = mousePosition;
    }

    // 드래그 중지
    private void StopDragging()
    {
        isDragging = false;
    }

    // 마우스 스크롤 처리
    private void HandleMouseScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        // ZoomCamera(scroll); // 여기에 원하는 확대/축소 로직을 넣으세요.
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
}
