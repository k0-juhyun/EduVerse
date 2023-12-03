using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class HomePageManager : MonoBehaviour
{
    [Header("�⺻�÷ο�")]
    public GameObject �⺻;

    [Space(10)]
    [Header("�������÷ο�")]
    public GameObject ������;

    [Space(10)]
    [Header("����������")]
    public GameObject ����������;

    [Space(10)]
    [Header("�л�DB")]
    public GameObject �л�����;
    [Space(10)]
    [Header("�����ڷ�")]
    public GameObject �����ڷ�;

    public TMP_Text Ȩ�������ؽ�Ʈ;
    public TMP_Text Ȩ�������ؽ�Ʈ2;


    public void OnTeachersRoomButtonClick()
    {
        �⺻.SetActive(false);
        ������.SetActive(true);
        Ȩ�������ؽ�Ʈ.text = "�� �� ��";
        Ȩ�������ؽ�Ʈ2.text = "�� �� ��";

    }

    public void OnBackButtonClick()
    {
        ������.SetActive(false);
        �⺻.SetActive(true);
        Ȩ�������ؽ�Ʈ.text = "HOME PAGE";
        Ȩ�������ؽ�Ʈ2.text = "HOME PAGE";

    }

    // ������ ����
    public void OnCreateTextbookBtnClick()
    {
        ����������.SetActive(true);
        transform.gameObject.SetActive(false);
    }

    // �л� ����
    public void OnStudentManageMentBtnClick()
    {
        PhotonNetwork.LoadLevel("StudentDB");
    }

    // ���� �ڷ�, ���� ����
    public void OnTextbookDataBtnClick()
    {
        transform.gameObject.SetActive(false);
        �����ڷ�.SetActive(true);
    }
}
