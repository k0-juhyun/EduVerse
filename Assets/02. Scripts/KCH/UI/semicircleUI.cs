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

    }

    public void Reset_value()
    {
        semicircle.fillAmount = 0;
    }

    public void semicircleTween(float value)
    {
        DOTween.To(() => semicircle.fillAmount, x => semicircle.fillAmount = x, value , 2f).SetEase(ease);
    }
}
