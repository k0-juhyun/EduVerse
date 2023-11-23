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
    public TMP_InputField 선생이름;
    public TMP_InputField 선생아이디;
    public TMP_InputField 선생비밀번호;
    public TMP_InputField 선생비밀번호확인;
    public TMP_InputField 선생이메일앞자리;
    public TMP_InputField 선생이메일뒷자리;
    public TMP_InputField 선생담당학년;
    public TMP_InputField 선생담당반;
    public TMP_Dropdown 선생emailDropDown;

    [Space(10)]
    [Header("3. 가입완료")]
    public GameObject 선생가입완료;
    public GameObject 학생가입완료;

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
    public TMP_InputField 학생이름;
    public TMP_InputField 학생아이디;
    public TMP_InputField 학생비밀번호;
    public TMP_InputField 학생비밀번호확인;
    public TMP_InputField 학생이메일앞자리;
    public TMP_InputField 학생이메일뒷자리;
    public TMP_InputField 학생학교;
    public TMP_InputField 학생생년월일;
    public TMP_InputField 학생학년;
    public TMP_InputField 학생반;
    public TMP_InputField 학생번호;
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
            선생이메일뒷자리.text = 선생emailDropDown.options[i].text;
        });

        학생emailDropDown.onValueChanged.AddListener((i) =>
        {
            학생이메일뒷자리.text = 학생emailDropDown.options[i].text;
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
        if (선생이름.text.Length > 0 && int.TryParse(선생담당학년.text, out grade) && int.TryParse(선생담당반.text, out classNum))
        {
            email = 선생이메일앞자리.text + "@" + 선생이메일뒷자리.text;
            FireAuth.instance.OnClickSingIn(email, 선생비밀번호.text, () =>
                {
                    선생가입완료.SetActive(true);
                    FireDatabase.instance.SaveUserInfo(new UserInfo(선생이름.text, true, grade, classNum, email, 선생비밀번호.text));
                });
        }

        else if (학생이름.text.Length > 0 && int.TryParse(학생학년.text, out grade) && int.TryParse(학생반.text, out classNum) && int.TryParse(학생번호.text, out studentNum) && int.TryParse(학생생년월일.text, out studentBirth))
        {
            email = 학생이메일앞자리.text + "@" + 학생이메일뒷자리.text;
            FireAuth.instance.OnClickSingIn(email, 학생비밀번호.text, () =>
            {
                학생가입완료.SetActive(true);
                print(학생이름.text);
                print(studentBirth);
                print(학생학교.text);
                print(grade);
                print(classNum);
                print(email);
                print(학생비밀번호.text);
                FireDatabase.instance.SaveUserInfo(new UserInfo(학생이름.text, false, studentBirth, 학생학교.text, grade, classNum, studentNum, email, 학생비밀번호.text));
            });
        }
    }

    public void OnTXButtonClick()
    {
        선생가입완료.SetActive(!선생가입완료.gameObject.activeSelf);
    }

    public void OnSTXButtonClick()
    {
        학생가입완료.SetActive(!학생가입완료.gameObject.activeSelf);
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
            선생이름.text = "";
            선생아이디.text = "";
            선생비밀번호.text = "";
            선생비밀번호확인.text = "";
            선생이메일앞자리.text = "";
            선생이메일뒷자리.text = "";
            선생담당학년.text = "";
            선생담당반.text = "";
        }
        else
        {
            학생이름.text = "";
            학생아이디.text = "";
            학생비밀번호.text = "";
            학생비밀번호확인.text = "";
            학생이메일앞자리.text = "";
            학생이메일뒷자리.text = "";
            학생학년.text = "";
            학생반.text = "";
            학생번호.text = "";
        }

        failedSingUp.SetActive(true);
    }
}
