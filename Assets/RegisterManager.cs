using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    [Header("1. ��� ���� �� ��������")]
    public GameObject ������ǹ׺�������;
    public Button �б������Թ�ư;
    public Button �л���ư;
    public RawImage �б�������üũ;
    public RawImage �л�üũ;
    public RawImage ���üũ;
    private bool _isTeacher;
    private bool isTorSBtn;
    [Header("--------------------------------")]
    public Button ���񽺹�ư;
    public Button ����������ư;
    public RawImage ����üũ;
    public RawImage ��������üũ;
    private bool isService;
    private bool isIdentify;
    [Header("--------------------------------")]
    public Button ��ε��ǹ�ư;

    [Space(10)]
    [Header("2. ���� ȸ������ �Է�")]
    public GameObject ����ȸ�������Է�;
    public TMP_InputField �����̸�;
    public TMP_InputField �������̵�;
    public TMP_InputField ������й�ȣ;
    public TMP_InputField ������й�ȣȮ��;
    public TMP_InputField �����̸��Ͼ��ڸ�;
    public TMP_InputField �����̸��ϵ��ڸ�;

    [Space(10)]
    [Header("3. ���ԿϷ�")]
    public GameObject ���ԿϷ�;

    [Space(10)]
    [Header("4. �� 14�� �̸�")]
    public GameObject ��14��;
    private bool isFirst;
    public GameObject ����1;
    public GameObject ����2;

    [Space(10)]
    [Header("5. �л� ȸ������ �Է�")]
    public GameObject �л�ȸ�������Է�;
    public TMP_InputField �л��̸�;
    public TMP_InputField �л����̵�;
    public TMP_InputField �л���й�ȣ;
    public TMP_InputField �л���й�ȣȮ��;
    public TMP_InputField �л��̸��Ͼ��ڸ�;
    public TMP_InputField �л��̸��ϵ��ڸ�;
    public TMP_InputField �л��б�;
    public TMP_InputField �л��г�;

    public bool IsTeacher
    {
        get { return _isTeacher; }
    }

    public void OnTeacherBtnClick()
    {
        if(_isTeacher == false)
            _isTeacher = true;

        isTorSBtn = !isTorSBtn;
        �б�������üũ.gameObject.SetActive(!�б�������üũ.gameObject.activeSelf);
        �л�üũ.gameObject.SetActive(false);
    }

    public void OnStudentBtnClick()
    {
        _isTeacher = false;
        isTorSBtn = !isTorSBtn;
        �б�������üũ.gameObject.SetActive(false);
        �л�üũ.gameObject.SetActive(!�л�üũ.gameObject.activeSelf);
    }

    public void OnServiceBtnClick()
    {
        isService = !isService;
        ����üũ.gameObject.SetActive(isService);
    }

    public void OnIdentifyBtnClick()
    {
        isIdentify = !isIdentify;
        ��������üũ.gameObject.SetActive(isIdentify);
    }

    public void OnAgreeBtnClick()
    {
        ���üũ.gameObject.SetActive(!���üũ.gameObject.activeSelf);

        if (isService && isIdentify && _isTeacher && isTorSBtn)
        {
            ������ǹ׺�������.gameObject.SetActive(false);
            ����ȸ�������Է�.gameObject.SetActive(true);
        }

        else if (isService && isIdentify && _isTeacher == false && isTorSBtn)
        {
            ��14��.gameObject.SetActive(true);
        }
    }

    public void OnRegisterCompleteBtnClick()
    {
        if (�����̸�.text.Length > 0 )
        {
            ���ԿϷ�.SetActive(true);
            FireAuth.instance.OnClickSingIn(�����̸��Ͼ��ڸ�.text + "@" + �����̸��ϵ��ڸ�.text, ������й�ȣ.text);
            FireDatabase.instance.SaveUserInfo(new UserInfo(�����̸�.text, true));
        }

        else if(�л��̸�.text.Length > 0 )
        {
            ���ԿϷ�.SetActive(true);
            FireAuth.instance.OnClickSingIn(�л��̸��Ͼ��ڸ�.text + "@" + �л��̸��ϵ��ڸ�.text, �л���й�ȣ.text);
            FireDatabase.instance.SaveUserInfo(new UserInfo(�л��̸�.text, false));
        }
    }

    public void OnXButtonClick()
    {
        ���ԿϷ�.SetActive(!���ԿϷ�.gameObject.activeSelf);
    }

    public void OnNextBtnClick()
    {
        print(�����̸�.text + "+" + _isTeacher);
        DataBase.instance.SetMyInfo(new User(�����̸�.text, _isTeacher));
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.NickName = �����̸�.text;
            PhotonNetwork.LoadLevel(1);
        }
    }

    public void OnBackBtnClick()
    {
        if (isFirst == false)
        {
            ����1.SetActive(false);
            ����2.SetActive(true);
            isFirst = true;
        }
        else
        {
            ��14��.SetActive(false);
            isFirst = false;
            ������ǹ׺�������.SetActive(false);
            �л�ȸ�������Է�.SetActive(true);
        }
    }
}
