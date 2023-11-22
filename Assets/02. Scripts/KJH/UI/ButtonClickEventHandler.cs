using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonClickEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public float scaleMultiplier = 1.1f; // 클릭 시 확대 배율
    public float hoverScaleMultiplier = 0.9f; // 마우스 오버 시 축소 배율
    public float animationDuration = 0.2f; // 애니메이션 지속 시간

    private Vector3 originalScale; // 원래 스케일 저장

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
        // 클릭 시 스케일 확대
        transform.DOScale(originalScale * scaleMultiplier, animationDuration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 클릭 끝날 때 원래 스케일로 복귀
        transform.DOScale(originalScale, animationDuration);
    }
}
