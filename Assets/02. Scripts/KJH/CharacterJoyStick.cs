using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterJoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform rectBackground;
    public RectTransform rectJoyStick;

    [Space]
    public Transform cameraPivotTransform;

    [Space]
    public GameObject Character;

    public float moveSpeed;
    private float radius;

    private bool isTouch = false;

    private Vector3 movePos;

    public Animator animator;

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    // 손을 떼면
    public void OnPointerUp(PointerEventData eventData)
    {
        // 손 떼면 조이스틱 초기화
        rectJoyStick.localPosition = Vector3.zero;
        movePos = Vector3.zero;

        // 손을 뗐을 때 이동 방향은 카메라 정면방향

        isTouch = false;
    }

    // 드래그중
    // 드래그 하는 곳으로 포인터 이동
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 value = eventData.position - (Vector2)rectBackground.position;
        value = Vector2.ClampMagnitude(value, radius);
        rectJoyStick.localPosition = value;

        value = value.normalized;

        float dis = Vector2.Distance(rectBackground.position, rectJoyStick.position) / radius;

        // 조이스틱 방향으로 움직임
        Vector3 moveDirection = new Vector3(value.x, 0, value.y);
        movePos = cameraPivotTransform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime;

        Character.transform.forward = movePos;
    }

    private void Awake()
    {
        radius = rectBackground.rect.width * 0.5f;

        // 애니메이터 컴포넌트 가져오기
        animator = Character.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // 터치할때만 움직이도록
        if (isTouch)
        {
            Character.transform.position += movePos;
        }
    }
}
