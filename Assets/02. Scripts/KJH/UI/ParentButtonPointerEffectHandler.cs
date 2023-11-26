using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ParentButtonPointerEffectHandler : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public float scaleMultiplier = 1.1f; // 클릭 시 확대 배율
    public float hoverScaleMultiplier = 0.9f; // 마우스 오버 시 축소 배율
    public float animationDuration = 0.2f; // 애니메이션 지속 시간

    private TMP_Text text;
    private Vector3 originalScale; // 원래 스케일 저장

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        originalScale = text.transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.transform.DOScale(originalScale * hoverScaleMultiplier, animationDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.transform.DOScale(originalScale, animationDuration);
    }
}
