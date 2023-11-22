using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonClickEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public float scaleMultiplier = 1.1f; // Ŭ�� �� Ȯ�� ����
    public float hoverScaleMultiplier = 0.9f; // ���콺 ���� �� ��� ����
    public float animationDuration = 0.2f; // �ִϸ��̼� ���� �ð�

    private Vector3 originalScale; // ���� ������ ����

    private PopUpClickHandler popUpClickHandler;

    void Start()
    {
        originalScale = transform.localScale;
        popUpClickHandler = GetComponent<PopUpClickHandler>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(originalScale * hoverScaleMultiplier, animationDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, animationDuration);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Ŭ�� �� ������ Ȯ��
        transform.DOScale(originalScale * scaleMultiplier, animationDuration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Ŭ�� ���� �� ���� �����Ϸ� ����
        transform.DOScale(originalScale, animationDuration);
    }
}
