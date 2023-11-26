using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnPointerEffectHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float moveDistance = 10f;  // 위로 올라갈 거리
    public float animationDuration = 0.2f;  // 애니메이션 지속 시간
    Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 위로 올라갔다가 원래 위치로 돌아오는 애니메이션 실행
        transform.DOLocalMoveY(originalPosition.y + moveDistance, animationDuration)
            .OnComplete(() => transform.DOLocalMoveY(originalPosition.y, animationDuration));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localPosition = originalPosition;
    }
}
