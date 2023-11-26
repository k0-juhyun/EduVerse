using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonClickEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public float scaleMultiplier = 1.1f; // Ŭ�� �� Ȯ�� ����
    public float hoverScaleMultiplier = 0.9f; // ���콺 ���� �� ��� ����
    public float animationDuration = 0.2f; // �ִϸ��̼� ���� �ð�

    private Vector3 originalScale; // ���� ������ ����

    private PopUpClickHandler popUpClickHandler;

    public Button thisButton;

    void Awake()
    {
        originalScale = transform.localScale;
        popUpClickHandler = GetComponent<PopUpClickHandler>();
        thisButton = GetComponent<Button?>();
    }

    void OnEnable()
    {
        // ������Ʈ�� Ȱ��ȭ�� �� ���� �����Ϸ� ����
        transform.localScale = originalScale;
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
        if (thisButton != null)
            thisButton.onClick.AddListener(() => OnClickDisable());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Ŭ�� ���� �� ���� �����Ϸ� ����
        transform.DOScale(originalScale, animationDuration);
    }

    private void OnClickDisable()
    {
        transform.DOScale(originalScale, animationDuration);
    }
}
