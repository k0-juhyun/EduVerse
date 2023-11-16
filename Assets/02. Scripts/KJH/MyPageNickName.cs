using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MyPageNickName : MonoBehaviourPun
{
    public TMP_Text myNickNameText;
    public string myNickName;

    void Start()
    {
        if(DataBase.instance.user.isTeacher)
            myNickNameText.text = PhotonNetwork.NickName + "  ������ | �α׾ƿ�";
        else
            myNickNameText.text = PhotonNetwork.NickName + "  �л� | �α׾ƿ�";

    }

}
