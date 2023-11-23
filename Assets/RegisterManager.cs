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
    public TMP_InputField ��������г�;
    public TMP_InputField ��������;
    public TMP_Dropdown ����emailDropDown;

    [Space(10)]
    [Header("3. ���ԿϷ�")]
    public GameObject �������ԿϷ�;
    public GameObject �л����ԿϷ�;

    [Space(10)]
    [Header("4. �� 14�� �̸�")]
    public GameObject ��14��;
    private bool isFirst;
    public GameObject ����1;
    public GameObject ����2;
    public GameObject ����3;
    public RawImage �����븮�κ���Ȯ��üũ;
    public RawImage �����븮�ε���üũ;
    private bool �����븮�κ���Ȯ��;
    private bool �����븮�ε���;
    public GameObject ������ư;
    public GameObject ������ư;

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
    public TMP_InputField �л��������;
    public TMP_InputField �л��г�;
    public TMP_InputField �л���;
    public TMP_InputField �л���ȣ;
    public TMP_Dropdown �л�emailDropDown;

    [Space(10)]
    [Header("6. ȸ������ ���� �˸�")]
    public GameObject failedSingUp;


    private StartSceneHandler startSceneHandler;

    public bool IsTeacher
    {
        get { return _isTeacher; }
    }

    void Start()
    {
        startSceneHandler = GetComponentInParent<StartSceneHandler>();
        // ��Ӵٿ����� �̸��� ���ּ� �����ϸ� �ؽ�Ʈ �ٲ��
        ����emailDropDown.onValueChanged.AddListener((i) =>
        {
            �����̸��ϵ��ڸ�.text = ����emailDropDown.options[i].text;
        });

        �л�emailDropDown.onValueChanged.AddListener((i) =>
        {
            �л��̸��ϵ��ڸ�.text = �л�emailDropDown.options[i].text;
        });
    }

    public void OnTeacherBtnClick()
    {
        //if(_isTeacher == false)
        //    _isTeacher = true;

        //isTorSBtn = !isTorSBtn;
        �б�������üũ.gameObject.SetActive(!�б�������üũ.gameObject.activeSelf);
        �л�üũ.gameObject.SetActive(false);
        _isTeacher = �б�������üũ.gameObject.activeSelf;
        isTorSBtn = �б�������üũ.gameObject.activeSelf;
    }

    public void OnStudentBtnClick()
    {
        _isTeacher = false;
        //isTorSBtn = !isTorSBtn;
        �б�������üũ.gameObject.SetActive(false);
        �л�üũ.gameObject.SetActive(!�л�üũ.gameObject.activeSelf);
        isTorSBtn = �л�üũ.gameObject.activeSelf;
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
        string email;
        int grade, classNum, studentNum, studentBirth;
        if (�����̸�.text.Length > 0 && int.TryParse(��������г�.text, out grade) && int.TryParse(��������.text, out classNum))
        {
            email = �����̸��Ͼ��ڸ�.text + "@" + �����̸��ϵ��ڸ�.text;
            FireAuth.instance.OnClickSingIn(email, ������й�ȣ.text, () =>
                {
                    �������ԿϷ�.SetActive(true);
                    FireDatabase.instance.SaveUserInfo(new UserInfo(�����̸�.text, true, grade, classNum, email, ������й�ȣ.text));
                });
        }

        else if (�л��̸�.text.Length > 0 && int.TryParse(�л��г�.text, out grade) && int.TryParse(�л���.text, out classNum) && int.TryParse(�л���ȣ.text, out studentNum) && int.TryParse(�л��������.text, out studentBirth))
        {
            email = �л��̸��Ͼ��ڸ�.text + "@" + �л��̸��ϵ��ڸ�.text;
            FireAuth.instance.OnClickSingIn(email, �л���й�ȣ.text, () =>
            {
                �л����ԿϷ�.SetActive(true);
                print(�л��̸�.text);
                print(studentBirth);
                print(�л��б�.text);
                print(grade);
                print(classNum);
                print(email);
                print(�л���й�ȣ.text);
                FireDatabase.instance.SaveUserInfo(new UserInfo(�л��̸�.text, false, studentBirth, �л��б�.text, grade, classNum, studentNum, email, �л���й�ȣ.text));
            });
        }
    }

    public void OnTXButtonClick()
    {
        �������ԿϷ�.SetActive(!�������ԿϷ�.gameObject.activeSelf);
    }

    public void OnSTXButtonClick()
    {
        �л����ԿϷ�.SetActive(!�л����ԿϷ�.gameObject.activeSelf);
    }

    public void OnNextBtnClick()
    {
        startSceneHandler.����.SetActive(true);
        startSceneHandler.ȸ������.SetActive(false);
    }

    public void OnNextContentBtnClick()
    {
        if (isFirst == false)
        {
            ����1.SetActive(false);
            ����2.SetActive(true);
            isFirst = true;
        }

        else if (isFirst && �����븮�κ���Ȯ��)
        {
            isFirst = false;
            ����2.SetActive(false);
            ����3.SetActive(true);
        }
    }

    public void OnLawCheckBoxClick()
    {
        �����븮�κ���Ȯ��üũ.gameObject.SetActive(!�����븮�κ���Ȯ��üũ.gameObject.activeSelf);
        �����븮�κ���Ȯ�� = !�����븮�κ���Ȯ��;
    }

    public void OnLawAgreeCheckBoxClick()
    {
        �����븮�ε���üũ.gameObject.SetActive(!�����븮�ε���üũ.gameObject.activeSelf);
        �����븮�ε��� = !�����븮�ε���;

        if(�����븮�ε���)
        {
            ������ư.SetActive(true);
            ������ư.SetActive(false);
        }
    }

    public void OnFinishLawAgreeButtonClick()
    {
        ������ǹ׺�������.gameObject.SetActive(false);
        ��14��.gameObject.SetActive(false);
        �л�ȸ�������Է�.gameObject.SetActive(true);
    }

    public void OnSignInFailed()
    {
        if (_isTeacher)
        {
            �����̸�.text = "";
            �������̵�.text = "";
            ������й�ȣ.text = "";
            ������й�ȣȮ��.text = "";
            �����̸��Ͼ��ڸ�.text = "";
            �����̸��ϵ��ڸ�.text = "";
            ��������г�.text = "";
            ��������.text = "";
        }
        else
        {
            �л��̸�.text = "";
            �л����̵�.text = "";
            �л���й�ȣ.text = "";
            �л���й�ȣȮ��.text = "";
            �л��̸��Ͼ��ڸ�.text = "";
            �л��̸��ϵ��ڸ�.text = "";
            �л��г�.text = "";
            �л���.text = "";
            �л���ȣ.text = "";
        }

        failedSingUp.SetActive(true);
    }
}
