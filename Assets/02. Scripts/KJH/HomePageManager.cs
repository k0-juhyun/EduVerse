using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePageManager : MonoBehaviour
{
    [Header("1. �⺻�÷ο�")]
    public GameObject �⺻;

    [Space(10)]
    [Header("------------------------------------")]
    [Header("2. �������÷ο�")]
    public GameObject ������;

    public void OnTeachersRoomButtonClick()
    {
        �⺻.SetActive(false);
        ������.SetActive(true);
    }

    public void OnBackButtonClick()
    {
        ������.SetActive(false);
        �⺻.SetActive(true);
    }
}
