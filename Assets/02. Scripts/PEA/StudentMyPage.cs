using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using Firebase.Database;
using Photon.Pun;

public class StudentMyPage : MonoBehaviour
{
    [Header("학생 프로필")]
    public TMP_Text profile_Name;
    public TMP_Text profile_School;
    public TMP_Text profile_Grade;
    public TMP_Text profile_Class;

    [Space(20)]
    [Header ("학생 정보")]
    public TMP_Text id_Text;
    public TMP_InputField password_Input;
    public TMP_Text name_Text;
    public TMP_Text nickName_Text;
    public TMP_Text email_Text;
    //public TMP_InputField phoneNumber_Input;
    public TMP_InputField school_Input;

    [Space(20)]
    [Header("버튼")]
    public Button editInfoBtn;
    public Button logoutBtn;

    private UserInfo userInfo;

    void Start()
    {
        editInfoBtn.onClick.AddListener(EditUserInfo);
        logoutBtn.onClick.AddListener(LogOut);
        userInfo = DataBase.instance.userInfo;
        ShowStudentProfile();
        ShowStudentInfo();
    }

    public void ShowStudentProfile()
    {
        profile_Name.text = userInfo.name;
        profile_School.text = userInfo.school;
        profile_Grade.text = userInfo.grade.ToString();
        profile_Class.text = userInfo.classNum.ToString();
    }

    public void ShowStudentInfo()
    {
        id_Text.text = userInfo.email.Split('@')[0];
        password_Input.text = userInfo.password;
        name_Text.text = userInfo.name;
        nickName_Text.text = userInfo.name;
        email_Text.text = userInfo.email;
        school_Input.text = userInfo.school;
    }

    public void EditUserInfo()
    {
        userInfo.password = password_Input.text;

        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;

        if (user != null)
        {
            user.UpdatePasswordAsync(userInfo.password).ContinueWith(task => {
                if (task.IsCompletedSuccessfully)
                {
                    // 비번 변경 성공
                    return;
                }
            });
        }
    }

    public void LogOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
    }
}

