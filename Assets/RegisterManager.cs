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

    [Space(10)]
    [Header("3. 가입완료")]
    public GameObject 가입완료;

    [Space(10)]
    [Header("4. 만 14세 미만")]
    public GameObject 만14세;
    private bool isFirst;
    public GameObject 내용1;
    public GameObject 내용2;

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
    public TMP_InputField 학생학년;

    public bool IsTeacher
    {
        get { return _isTeacher; }
    }

    public void OnTeacherBtnClick()
    {
        if(_isTeacher == false)
            _isTeacher = true;

        isTorSBtn = !isTorSBtn;
        학교선생님체크.gameObject.SetActive(!학교선생님체크.gameObject.activeSelf);
        학생체크.gameObject.SetActive(false);
    }

    public void OnStudentBtnClick()
    {
        _isTeacher = false;
        isTorSBtn = !isTorSBtn;
        학교선생님체크.gameObject.SetActive(false);
        학생체크.gameObject.SetActive(!학생체크.gameObject.activeSelf);
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
        if (선생이름.text.Length > 0 )
        {
            가입완료.SetActive(true);
            FireAuth.instance.OnClickSingIn(선생이메일앞자리.text + "@" + 선생이메일뒷자리.text, 선생비밀번호.text);
            FireDatabase.instance.SaveUserInfo(new UserInfo(선생이름.text, true));
        }

        else if(학생이름.text.Length > 0 )
        {
            가입완료.SetActive(true);
            FireAuth.instance.OnClickSingIn(학생이메일앞자리.text + "@" + 학생이메일뒷자리.text, 학생비밀번호.text);
            FireDatabase.instance.SaveUserInfo(new UserInfo(학생이름.text, false));
        }
    }

    public void OnXButtonClick()
    {
        가입완료.SetActive(!가입완료.gameObject.activeSelf);
    }

    public void OnNextBtnClick()
    {
        print(선생이름.text + "+" + _isTeacher);
        DataBase.instance.SetMyInfo(new User(선생이름.text, _isTeacher));
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.NickName = 선생이름.text;
            PhotonNetwork.LoadLevel(1);
        }
    }

    public void OnBackBtnClick()
    {
        if (isFirst == false)
        {
            내용1.SetActive(false);
            내용2.SetActive(true);
            isFirst = true;
        }
        else
        {
            만14세.SetActive(false);
            isFirst = false;
            약관동의및본인인증.SetActive(false);
            학생회원정보입력.SetActive(true);
        }
    }
}
