using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;
    private Transform originTransform;

    private Vector3 cameraFollowVelo = Vector3.zero;

    public float rotSpeed;
    public float scrollSpeed;
    public float dragSpeed;
    private float xRotate;
    private float originFieldOfView;

    private bool isRotating = false;
    private bool isDragging = false;
    private Vector2 previousMousePosition;

    private Rect touchZone;

    private void Awake()
    {
        originTransform = transform;
        originFieldOfView = Camera.main.fieldOfView;
        // ȭ���� Ư�� �κ��� ��ġ �������� �����մϴ�. (���÷� ȭ���� �߾� 200x200 ����)
        touchZone = new Rect((Screen.width - 200) / 2, (Screen.height - 200) / 2, 200, 200);
    }

    private void LateUpdate()
    {
        FollowCamera();
        HandleInput();
    }

    // ī�޶� target ������Ʈ�� ����ٴ�
    private void FollowCamera()
    {
        Vector3 targetPos = Vector3.SmoothDamp(originTransform.position, targetTransform.position,
            ref cameraFollowVelo, Time.deltaTime / 0.1f);

        originTransform.position = targetPos;
    }

    // �Է� ó��
    private void HandleInput()
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
                if (Input.touchCount == 2)
                {
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    Vector2 touchZeroPreviousPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePreviousPos = touchOne.position - touchOne.deltaPosition;

                    float prevMagnitude = (touchZeroPreviousPos - touchOnePreviousPos).magnitude;
                    float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                    float difference = currentMagnitude - prevMagnitude;
                    ZoomCamera(difference * 0.01f);
                }
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

        if (isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - previousMousePosition;
            xRotate += delta.x * rotSpeed;
            cameraPivotTransform.localEulerAngles = new Vector3(0, xRotate, 0);
            previousMousePosition = (Vector2)Input.mousePosition;
        }

        // ���콺 ��ũ�ѷ� Ȯ�� �� ���
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll);
    }

    // ī�޶� Ȯ�� �� ���
    //private void ZoomCamera(float increment)
    //{
    //    Vector3 pos = cameraTransform.localPosition;
    //    pos.z += increment * scrollSpeed;
    //    pos.z = Mathf.Clamp(pos.z, -5, -1);
    //    cameraTransform.localPosition = pos;
    //}

    private void ZoomCamera(float increment)
    {
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - increment * scrollSpeed, 30, 90);
    }
}
