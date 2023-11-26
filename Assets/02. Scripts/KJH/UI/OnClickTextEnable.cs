using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class OnClickTextEnable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text context;
    private Vector2 originalPosition;

    private void Awake()
    {
        originalPosition = context.rectTransform.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        context.gameObject.SetActive(true);
        context.rectTransform.DOKill(); // 이전 애니메이션 취소
        Vector2 targetPosition = new Vector2(originalPosition.x, originalPosition.y + 24); // 위로 이동할 위치
        context.rectTransform.DOAnchorPos(targetPosition, 0.5f).SetEase(Ease.OutBack);
        StartCoroutine(ICloseText(0.25f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        context.rectTransform.DOKill(); // 이전 애니메이션 취소
        context.rectTransform.DOAnchorPos(originalPosition, 0.5f).SetEase(Ease.InBack)
            .OnComplete(() => context.gameObject.SetActive(false));
    }

    private IEnumerator ICloseText(float delay)
    {
        yield return new WaitForSeconds(delay);
        context.rectTransform.DOAnchorPos(originalPosition, 0.5f).SetEase(Ease.InBack)
            .OnComplete(() => context.gameObject.SetActive(false));
    }
}
