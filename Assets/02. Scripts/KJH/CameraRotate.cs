using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public Transform targetTransform;
    public Transform cameraPivotTransform;
    private Transform originTransform;

    [Space(10)]
    [Header("속도 값")]
    public float rotSpeed;
    public float scrollSpeed;
    public float dragSpeed;
    private float xRotate;

    private bool isDragging = false;

    private Vector2 previousMousePosition;
    private float originFieldOfView;

    // 터치 가능한 부분
    private Rect touchZone;

    private void Awake()
    {
        originTransform = transform;
        originFieldOfView = Camera.main.fieldOfView;

        // 화면의 특정 부분을 터치 영역으로 지정합니다. (예시로 화면의 중앙 200x200 영역)
        //touchZone = new Rect((Screen.width - 200) / 2, (Screen.height - 200) / 2, 600, 400);
        touchZone = new Rect((Screen.width - 400) / 4, (Screen.height - 400) / 2, 400, 400);
    }

    private void LateUpdate()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

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

                        // x값 제한
                        xRotate = Mathf.Clamp(xRotate, -20f, 0f);

                        cameraPivotTransform.localEulerAngles = new Vector3(Mathf.Clamp(0f, 0f, 360f), Mathf.Clamp(xRotate, -20f, 0f), 0);
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

        if (isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - previousMousePosition;
            xRotate += delta.x * rotSpeed;
            cameraPivotTransform.localEulerAngles = new Vector3(0, xRotate, 0);
            previousMousePosition = (Vector2)Input.mousePosition;
        }

        // 마우스 스크롤로 확대 및 축소
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll);
    }

    private void ZoomCamera(float increment)
    {
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - increment * scrollSpeed, 30, 90);
    }
}

