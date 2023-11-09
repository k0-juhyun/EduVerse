using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraSetting : MonoBehaviourPun
{
    [Header("ī�޶�")]
    public Camera TPS_Camera;
    public Camera FPS_Camera;

    [Header("��ġ ��")]
    public Transform targetTransform;
    public Transform cameraPivotTransform;
    private Transform originTransform;

    private Vector3 cameraFollowVelo = Vector3.zero;
    private Vector2 previousMousePosition;

    private GameObject Character;

    [Space(10)]
    [Header("�ӵ� ��")]
    public float rotSpeed;
    public float scrollSpeed;
    public float dragSpeed;
    private float xRotate;
    private float originFieldOfView;

    // �Ҹ��� ����
    private bool isDragging = false;

    // ��ġ ������ �κ�
    private Rect touchZone;

    // ������Ʈ ��
    private CharacterInteraction characterInteraction;
    private TeacherInteraction teacherInteraction;

    private void Awake()
    {
        originTransform = transform;
        originFieldOfView = Camera.main.fieldOfView;

        // ȭ���� Ư�� �κ��� ��ġ �������� �����մϴ�. (���÷� ȭ���� �߾� 200x200 ����)
        touchZone = new Rect((Screen.width - 200) / 2, (Screen.height - 200) / 2, 200, 200);

        // �θ𿡼� ������Ʈ ���
        characterInteraction = GetComponentInParent<CharacterInteraction>();
        teacherInteraction = GetComponentInParent<TeacherInteraction>();
    }

    private void LateUpdate()
    {
        UpdateCamera();
        HandleInput();
        FollowCamera();
    }

    // ī�޶� target ������Ʈ�� ����ٴ�
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

    // �Է� ó��
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

                // ��ġ ���� �������� ��ġ �̺�Ʈ�� ó���մϴ�.
                if (touchZone.Contains(touch.position))
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        // ��ġ �巡�׷� ȸ��
                        if (isDragging)
                        {
                            Vector2 delta = (Vector2)Input.mousePosition - previousMousePosition;
                            xRotate += delta.x * rotSpeed;

                            // x�� ����
                            xRotate = Mathf.Clamp(xRotate, -20f, 0f);

                            cameraPivotTransform.localEulerAngles = new Vector3(Mathf.Clamp(0f, 0f, 360f), Mathf.Clamp(xRotate, -20f, 0f), 0);
                            previousMousePosition = (Vector2)Input.mousePosition;
                        }
                    }
                    else if (touch.phase == TouchPhase.Began)
                    {
                        // ��ġ ����
                        if (touch.phase == TouchPhase.Began && touch.tapCount == 1)
                        {
                            isDragging = true;
                            previousMousePosition = touch.position;
                        }
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        // ��ġ ����
                        isDragging = false;
                    }

                    // ��ġ Ȯ�� �� ���
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

            // ���콺 �Է� ó��
            if (Input.GetMouseButtonDown(0))
            {
                if (touchZone.Contains(Input.mousePosition))
                {
                    isDragging = true;
                    previousMousePosition = Input.mousePosition;
                }
            }

            // ī�޶� �巡�� �� ���� ���� ��
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            if (isDragging && teacherInteraction.isSpawnBtnClick == false)
            {
                Vector2 delta = (Vector2)Input.mousePosition - previousMousePosition;
                xRotate += delta.x * rotSpeed;
                cameraPivotTransform.localEulerAngles = new Vector3(0, xRotate, 0);
                previousMousePosition = (Vector2)Input.mousePosition;
            }

            // ���콺 ��ũ�ѷ� Ȯ�� �� ���
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
            // �ɾ� ���� ���� ī�޶� ����
            TPS_Camera.gameObject.SetActive(false);
            FPS_Camera.gameObject.SetActive(true);
            FPS_Camera.depth = 1;
        }
        else
        {
            // �� ���� ���� ī�޶� ����
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