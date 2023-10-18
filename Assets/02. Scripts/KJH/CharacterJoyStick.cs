using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterJoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform rectBackground;
    public RectTransform rectJoyStick;

    public Transform cameraPivotTransform;

    private float radius;
    public float moveSpeed;

    public GameObject Character;

    private bool isTouch = false;

    private Vector3 movePos;

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    // ���� ����
    public void OnPointerUp(PointerEventData eventData)
    {
        // �� ���� ���̽�ƽ �ʱ�ȭ
        rectJoyStick.localPosition = Vector3.zero;
        movePos = Vector3.zero;

        // ���� ���� �� �̵� ������ ī�޶� �������

        isTouch = false;
    }

    // �巡����
    // �巡�� �ϴ� ������ ������ �̵�
    public void OnDrag(PointerEventData eventData)
    {
        //Vector2 value = eventData.position - (Vector2)rectBackground.position;

        //value = Vector2.ClampMagnitude(value, radius);

        //rectJoyStick.localPosition = value;

        //value = value.normalized;

        //float dis = Vector2.Distance(rectBackground.position, rectJoyStick.position) / radius;

        //// ���̽�ƽ �������� ������
        //movePos = new Vector3(value.x * moveSpeed * Time.deltaTime, 0,
        //    value.y * moveSpeed * Time.deltaTime);

        //// �����̴� ���� �ٶ�

        Vector2 value = eventData.position - (Vector2)rectBackground.position;
        value = Vector2.ClampMagnitude(value, radius);
        rectJoyStick.localPosition = value;

        value = value.normalized;

        float dis = Vector2.Distance(rectBackground.position, rectJoyStick.position) / radius;

        // ���̽�ƽ �������� ������
        Vector3 moveDirection = new Vector3(value.x, 0, value.y);
        movePos = cameraPivotTransform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime;

        Character.transform.forward = movePos;
    }

    private void Awake()
    {
        radius = rectBackground.rect.width * 0.5f;
    }

    private void FixedUpdate()
    {
        // ��ġ�Ҷ��� �����̵���
        if (isTouch)
        {
            Character.transform.position += movePos;
        }
    }
}
