using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePageManager : MonoBehaviour
{
    [Header("1. 기본플로우")]
    public GameObject 기본;

    [Space(10)]
    [Header("------------------------------------")]
    [Header("2. 교무실플로우")]
    public GameObject 교무실;

    public void OnTeachersRoomButtonClick()
    {
        기본.SetActive(false);
        교무실.SetActive(true);
    }

    public void OnBackButtonClick()
    {
        교무실.SetActive(false);
        기본.SetActive(true);
    }
}
