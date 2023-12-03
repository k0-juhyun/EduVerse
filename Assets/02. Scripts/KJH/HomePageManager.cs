using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class HomePageManager : MonoBehaviour
{
    [Header("기본플로우")]
    public GameObject 기본;

    [Space(10)]
    [Header("교무실플로우")]
    public GameObject 교무실;

    [Space(10)]
    [Header("교과서제작")]
    public GameObject 교과서제작;

    [Space(10)]
    [Header("학생DB")]
    public GameObject 학생관리;
    [Space(10)]
    [Header("교과자료")]
    public GameObject 교과자료;

    public TMP_Text 홈페이지텍스트;
    public TMP_Text 홈페이지텍스트2;


    public void OnTeachersRoomButtonClick()
    {
        기본.SetActive(false);
        교무실.SetActive(true);
        홈페이지텍스트.text = "교 무 실";
        홈페이지텍스트2.text = "교 무 실";

    }

    public void OnBackButtonClick()
    {
        교무실.SetActive(false);
        기본.SetActive(true);
        홈페이지텍스트.text = "HOME PAGE";
        홈페이지텍스트2.text = "HOME PAGE";

    }

    // 교과서 제작
    public void OnCreateTextbookBtnClick()
    {
        교과서제작.SetActive(true);
        transform.gameObject.SetActive(false);
    }

    // 학생 관리
    public void OnStudentManageMentBtnClick()
    {
        PhotonNetwork.LoadLevel("StudentDB");
    }

    // 교과 자료, 나눔 마당
    public void OnTextbookDataBtnClick()
    {
        transform.gameObject.SetActive(false);
        교과자료.SetActive(true);
    }
}
