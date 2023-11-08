using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using static System.Net.Mime.MediaTypeNames;

public class StartSetting : MonoBehaviour
{
    public InputField nameInput;
    public Toggle isTeacherTpggle;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void OnCllickStartBtn()
    {
        if(nameInput.text.Length > 0)
        {
            DataBase.instance.SetMyInfo(new User(nameInput.text, DataBase.instance.userInfo.isTeacher));
            if(PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.NickName = nameInput.text;
                PhotonNetwork.LoadLevel(1);
            }
        }
    }

    public void OnClickRegisterBtn()
    {
        PhotonNetwork.LoadLevel("RegisterScene");
    }

    public void OnClickVerifyButton()
    {
        DataBase.instance.userInfo.isTeacher = true;
        print("12");
    }
}
