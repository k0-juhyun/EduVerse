using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnPointerEffectHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float moveDistance = 10f;  // ���� �ö� �Ÿ�
    public float animationDuration = 0.2f;  // �ִϸ��̼� ���� �ð�
    Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ���� �ö󰬴ٰ� ���� ��ġ�� ���ƿ��� �ִϸ��̼� ����
        transform.DOLocalMoveY(originalPosition.y + moveDistance, animationDuration)
            .OnComplete(() => transform.DOLocalMoveY(originalPosition.y, animationDuration));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localPosition = originalPosition;
    }
}
