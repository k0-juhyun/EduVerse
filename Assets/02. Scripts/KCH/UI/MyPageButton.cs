using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MyPageButton : MonoBehaviour
{
    public GameObject BackButton;

    bool isMenuOpen=false;
    public void MenuButton()
    {
        if (!isMenuOpen)
        {
            BackButton.GetComponent<RectTransform>().DOAnchorPosY(60, 0.5f);
            isMenuOpen=!isMenuOpen;
        }
        else
        {
            BackButton.GetComponent<RectTransform>().DOAnchorPosY(20, 0.5f);
            isMenuOpen = !isMenuOpen;
        }
    }
}
