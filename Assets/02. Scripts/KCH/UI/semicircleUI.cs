using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class semicircleUI : MonoBehaviour
{
    public Image semicircle;
    public Ease ease;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            DOTween.To(() => semicircle.fillAmount, x => semicircle.fillAmount = x, 0.5f, 1f).SetEase(ease);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            semicircle.fillAmount = 0;
        }
    }
    public void tween()
    {
        DOTween.To(() => semicircle.fillAmount, x => semicircle.fillAmount = x, 0.5f, 1f).SetEase(ease);
    }
}
