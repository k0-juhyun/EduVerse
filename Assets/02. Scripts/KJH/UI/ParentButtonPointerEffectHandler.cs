using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ParentButtonPointerEffectHandler : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public float scaleMultiplier = 1.1f; // Ŭ�� �� Ȯ�� ����
    public float hoverScaleMultiplier = 0.9f; // ���콺 ���� �� ��� ����
    public float animationDuration = 0.2f; // �ִϸ��̼� ���� �ð�

    private TMP_Text text;
    private Vector3 originalScale; // ���� ������ ����

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
