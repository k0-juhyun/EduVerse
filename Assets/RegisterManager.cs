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
    public TMP_InputField teacherName;
    public TMP_InputField teacherID;
    public TMP_InputField teacherPassword;
    public TMP_InputField teacherPasswordCheck;
    public TMP_InputField teacherEmailFront;
    public TMP_InputField teacherEmailBack;
    public TMP_InputField teacherGrade;
    public TMP_InputField teacherClass;
    public TMP_Dropdown ����emailDropDown;

    [Space(10)]
    [Header("3. ���ԿϷ�")]
    public GameObject teacherComplete;
    public GameObject studentComplete;

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
    public TMP_InputField studentName;
    public TMP_InputField studentID;
    public TMP_InputField studentPassword;
    public TMP_InputField studentPasswordCheck;
    public TMP_InputField studentEmailFront;
    public TMP_InputField studentEmailBack;
    public TMP_InputField studentSchool;
    public TMP_InputField studentBirthInput;
    public TMP_InputField studentGrade;
    public TMP_InputField studentClass;
    public TMP_InputField studentNumber;
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
            teacherEmailBack.text = ����emailDropDown.options[i].text;
        });

        �л�emailDropDown.onValueChanged.AddListener((i) =>
        {
            studentEmailBack.text = �л�emailDropDown.options[i].text;
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

        // ������ ȸ�� ����
        if (teacherName.text.Length > 0 && int.TryParse(teacherGrade.text, out grade) && int.TryParse(teacherClass.text, out classNum))
        {
            email = teacherEmailFront.text + "@" + teacherEmailBack.text;
            FireAuth.instance.OnClickSingIn(email, teacherPassword.text, () =>
                {
                    teacherComplete.SetActive(true);
                    FireDatabase.instance.SaveUserInfo(new UserInfo(teacherName.text, true, grade, classNum, email, teacherPassword.text));
                });
        }

        // �л� ȸ�� ����
        else if (studentName.text.Length > 0 && int.TryParse(studentGrade.text, out grade) && int.TryParse(studentClass.text, out classNum) && int.TryParse(studentNumber.text, out studentNum) && int.TryParse(studentBirthInput.text, out studentBirth))
        {
            email = studentEmailFront.text + "@" + studentEmailBack.text;
            FireAuth.instance.OnClickSingIn(email, studentPassword.text, () =>
            {
                studentComplete.SetActive(true);
                FireDatabase.instance.SaveUserInfo(new UserInfo(studentName.text, false, studentBirth, studentSchool.text, grade, classNum, studentNum, email, studentPassword.text));
            });
        }

        // ������ ����� �Է����� �ʾ��� ��� 
        else
        {
            OnSignInFailed();
        }
    }

    public void OnTXButtonClick()
    {
        teacherComplete.SetActive(!teacherComplete.gameObject.activeSelf);
    }

    public void OnSTXButtonClick()
    {
        studentComplete.SetActive(!studentComplete.gameObject.activeSelf);
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
            teacherName.text = "";
            teacherID.text = "";
            teacherPassword.text = "";
            teacherPasswordCheck.text = "";
            teacherEmailFront.text = "";
            teacherEmailBack.text = "";
            teacherGrade.text = "";
            teacherClass.text = "";
        }
        else
        {
            studentName.text = "";
            studentID.text = "";
            studentPassword.text = "";
            studentPasswordCheck.text = "";
            studentEmailFront.text = "";
            studentEmailBack.text = "";
            studentGrade.text = "";
            studentClass.text = "";
            studentNumber.text = "";
        }

        failedSingUp.SetActive(true);
    }
}
