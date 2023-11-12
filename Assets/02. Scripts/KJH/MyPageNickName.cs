using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyPageNickName : MonoBehaviourPun
{
    public Text myNickNameTxt;
    public string myNickName;

    void Start()
    {
        if(DataBase.instance.userInfo.isTeacher)
            myNickNameTxt.text = PhotonNetwork.NickName + "  ������ | �α׾ƿ�";
        else
            myNickNameTxt.text = PhotonNetwork.NickName + "  �л� | �α׾ƿ�";

    }

}
