using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

// ĳ���� ������
// ���̽�ƽ �� �޾Ƽ� ĳ���Ͱ� ������
public class CharacterMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform rectBackground;
    public RectTransform rectJoyStick;

    [Space] public Transform cameraPivotTransform;

    [Space] [Header("ĳ����")] public GameObject Character;

    private float moveSpeed = 2;
    private float radius;
    private float animParameters;

    [Space]
    [Header("�̵��ӵ�")]
    public float minSpeed;
    public float maxSpeed;

    private bool isTouch = false;

    private Vector3 movePos;

    private Animator animator;

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
        animator.SetFloat("moveSpeed", 0);
        animator.SetTrigger("Idle");
    }

    // �巡����
    // �巡�� �ϴ� ������ ������ �̵�
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 value = eventData.position - (Vector2)rectBackground.position;
        value = Vector2.ClampMagnitude(value, radius);
        rectJoyStick.localPosition = value;

        value = value.normalized;

        float dis = Vector2.Distance(rectBackground.position, rectJoyStick.position) / radius;

        // ���̽�ƽ �������� ������
        Vector3 moveDirection = new Vector3(value.x, 0, value.y);
        movePos = cameraPivotTransform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime;

        Character.transform.forward = movePos;

        // ���̽�ƽ �Է¿� ���� ������ �� ���
        float distance = Vector3.Distance(rectJoyStick.localPosition, Vector3.zero);
        animParameters = distance / radius; // 0���� 1 ������ ��

        // ������ Ʈ���� Weight ���� �����Ͽ� �ִϸ��̼� ����
        animator.SetFloat("moveSpeed", animParameters);

        moveSpeed = Mathf.Lerp(minSpeed, maxSpeed, animParameters);
    }

    private void Awake()
    {
        radius = rectBackground.rect.width * 0.5f;

        // �ִϸ����� ������Ʈ ��������
        animator = Character.GetComponent<Animator>();
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