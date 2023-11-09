using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // ������ ����
    public void OnCreateTextbookBtnClick()
    {
        transform.gameObject.SetActive(false);
        ����������.SetActive(true);
    }

    // �л� ����
    public void OnStudentManageMentBtnClick()
    {

    }

    // ���� �ڷ�, ���� ����
    public void OnTextbookDataBtnClick()
    {
        transform.gameObject.SetActive(false);
        �����ڷ�.SetActive(true);
    }
}
