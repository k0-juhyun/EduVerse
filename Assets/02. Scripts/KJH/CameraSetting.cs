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

    private void Awake()
    {
        originTransform = transform;
        originFieldOfView = Camera.main.fieldOfView;

        // ȭ���� Ư�� �κ��� ��ġ �������� �����մϴ�. (���÷� ȭ���� �߾� 200x200 ����)
        touchZone = new Rect((Screen.width - 200) / 2, (Screen.height - 200) / 2, 200, 200);

        // �θ𿡼� ������Ʈ ���
        characterInteraction = GetComponentInParent<CharacterInteraction>();

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

    // ��ġ �Է� ó��
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

    // ���콺 �Է� ó��
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

    // ī�޶� �巡��
    private void DragCamera(Vector2 currentMousePosition)
    {
        Vector2 delta = currentMousePosition - previousMousePosition;
        xRotate += delta.x * rotSpeed;
        cameraPivotTransform.localEulerAngles = new Vector3(0, xRotate, 0);
        previousMousePosition = currentMousePosition;
    }

    // �巡�� ����
    private void StartDragging(Vector2 mousePosition)
    {
        isDragging = true;
        previousMousePosition = mousePosition;
    }

    // �巡�� ����
    private void StopDragging()
    {
        isDragging = false;
    }

    // ���콺 ��ũ�� ó��
    private void HandleMouseScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        // ZoomCamera(scroll); // ���⿡ ���ϴ� Ȯ��/��� ������ ��������.
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
}
