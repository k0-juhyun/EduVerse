using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class semicircleUI : MonoBehaviour
{
    public Image semicircle;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DOTween.To(() => semicircle.fillAmount, x => semicircle.fillAmount = x, 0.5f, 1f).SetEase(Ease.InOutQuad);
        }
    }
}
