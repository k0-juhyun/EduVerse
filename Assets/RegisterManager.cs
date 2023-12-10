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
    [Header("1. 약관 동의 및 본인인증")]
    public GameObject 약관동의및본인인증;
    public Button 학교선생님버튼;
    public Button 학생버튼;
    public RawImage 학교선생님체크;
    public RawImage 학생체크;
    public RawImage 모두체크;
    private bool _isTeacher;
    private bool isTorSBtn;
    [Header("--------------------------------")]
    public Button 서비스버튼;
    public Button 개인정보버튼;
    public RawImage 서비스체크;
    public RawImage 개인정보체크;
    private bool isService;
    private bool isIdentify;
    [Header("--------------------------------")]
    public Button 모두동의버튼;

    [Space(10)]
    [Header("2. 선생 회원정보 입력")]
    public GameObject 선생회원정보입력;
    public TMP_InputField teacherName;
    public TMP_InputField teacherID;
    public TMP_InputField teacherPassword;
    public TMP_InputField teacherPasswordCheck;
    public TMP_InputField teacherEmailFront;
    public TMP_InputField teacherEmailBack;
    public TMP_InputField teacherGrade;
    public TMP_InputField teacherClass;
    public TMP_Dropdown 선생emailDropDown;

    [Space(10)]
    [Header("3. 가입완료")]
    public GameObject teacherComplete;
    public GameObject studentComplete;

    [Space(10)]
    [Header("4. 만 14세 미만")]
    public GameObject 만14세;
    private bool isFirst;
    public GameObject 내용1;
    public GameObject 내용2;
    public GameObject 내용3;
    public RawImage 법정대리인본인확인체크;
    public RawImage 법정대리인동의체크;
    private bool 법정대리인본인확인;
    private bool 법정대리인동의;
    public GameObject 엑스버튼;
    public GameObject 다음버튼;

    [Space(10)]
    [Header("5. 학생 회원정보 입력")]
    public GameObject 학생회원정보입력;
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
    public TMP_Dropdown 학생emailDropDown;

    [Space(10)]
    [Header("6. 회원가입 실패 알림")]
    public GameObject failedSingUp;


    private StartSceneHandler startSceneHandler;

    public bool IsTeacher
    {
        get { return _isTeacher; }
    }

    void Start()
    {
        startSceneHandler = GetComponentInParent<StartSceneHandler>();
        // 드롭다운으로 이메일 뒷주소 선택하면 텍스트 바뀌게
        선생emailDropDown.onValueChanged.AddListener((i) =>
        {
            teacherEmailBack.text = 선생emailDropDown.options[i].text;
        });

        학생emailDropDown.onValueChanged.AddListener((i) =>
        {
            studentEmailBack.text = 학생emailDropDown.options[i].text;
        });
    }

    public void OnTeacherBtnClick()
    {
        //if(_isTeacher == false)
        //    _isTeacher = true;

        //isTorSBtn = !isTorSBtn;
        학교선생님체크.gameObject.SetActive(!학교선생님체크.gameObject.activeSelf);
        학생체크.gameObject.SetActive(false);
        _isTeacher = 학교선생님체크.gameObject.activeSelf;
        isTorSBtn = 학교선생님체크.gameObject.activeSelf;
    }

    public void OnStudentBtnClick()
    {
        _isTeacher = false;
        //isTorSBtn = !isTorSBtn;
        학교선생님체크.gameObject.SetActive(false);
        학생체크.gameObject.SetActive(!학생체크.gameObject.activeSelf);
        isTorSBtn = 학생체크.gameObject.activeSelf;
    }

    public void OnServiceBtnClick()
    {
        isService = !isService;
        서비스체크.gameObject.SetActive(isService);
    }

    public void OnIdentifyBtnClick()
    {
        isIdentify = !isIdentify;
        개인정보체크.gameObject.SetActive(isIdentify);
    }

    public void OnAgreeBtnClick()
    {
        모두체크.gameObject.SetActive(!모두체크.gameObject.activeSelf);

        if (isService && isIdentify && _isTeacher && isTorSBtn)
        {
            약관동의및본인인증.gameObject.SetActive(false);
            선생회원정보입력.gameObject.SetActive(true);
        }

        else if (isService && isIdentify && _isTeacher == false && isTorSBtn)
        {
            만14세.gameObject.SetActive(true);
        }
    }

    public void OnRegisterCompleteBtnClick()
    {
        string email;
        int grade, classNum, studentNum, studentBirth;

        // 선생님 회원 가입
        if (teacherName.text.Length > 0 && int.TryParse(teacherGrade.text, out grade) && int.TryParse(teacherClass.text, out classNum))
        {
            email = teacherEmailFront.text + "@" + teacherEmailBack.text;
            FireAuth.instance.OnClickSingIn(email, teacherPassword.text, () =>
                {
                    teacherComplete.SetActive(true);
                    FireDatabase.instance.SaveUserInfo(new UserInfo(teacherName.text, true, grade, classNum, email, teacherPassword.text));
                });
        }

        // 학생 회원 가입
        else if (studentName.text.Length > 0 && int.TryParse(studentGrade.text, out grade) && int.TryParse(studentClass.text, out classNum) && int.TryParse(studentNumber.text, out studentNum) && int.TryParse(studentBirthInput.text, out studentBirth))
        {
            email = studentEmailFront.text + "@" + studentEmailBack.text;
            FireAuth.instance.OnClickSingIn(email, studentPassword.text, () =>
            {
                studentComplete.SetActive(true);
                FireDatabase.instance.SaveUserInfo(new UserInfo(studentName.text, false, studentBirth, studentSchool.text, grade, classNum, studentNum, email, studentPassword.text));
            });
        }

        // 정보를 제대로 입력하지 않았을 경우 
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
        startSceneHandler.시작.SetActive(true);
        startSceneHandler.회원가입.SetActive(false);
    }

    public void OnNextContentBtnClick()
    {
        if (isFirst == false)
        {
            내용1.SetActive(false);
            내용2.SetActive(true);
            isFirst = true;
        }

        else if (isFirst && 법정대리인본인확인)
        {
            isFirst = false;
            내용2.SetActive(false);
            내용3.SetActive(true);
        }
    }

    public void OnLawCheckBoxClick()
    {
        법정대리인본인확인체크.gameObject.SetActive(!법정대리인본인확인체크.gameObject.activeSelf);
        법정대리인본인확인 = !법정대리인본인확인;
    }

    public void OnLawAgreeCheckBoxClick()
    {
        법정대리인동의체크.gameObject.SetActive(!법정대리인동의체크.gameObject.activeSelf);
        법정대리인동의 = !법정대리인동의;

        if(법정대리인동의)
        {
            엑스버튼.SetActive(true);
            다음버튼.SetActive(false);
        }
    }

    public void OnFinishLawAgreeButtonClick()
    {
        약관동의및본인인증.gameObject.SetActive(false);
        만14세.gameObject.SetActive(false);
        학생회원정보입력.gameObject.SetActive(true);
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
