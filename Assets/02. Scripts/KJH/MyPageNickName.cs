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
            myNickNameTxt.text = PhotonNetwork.NickName + "  ¼±»ý´Ô | ·Î±×¾Æ¿ô";
        else
            myNickNameTxt.text = PhotonNetwork.NickName + "  ÇÐ»ý | ·Î±×¾Æ¿ô";

    }

}
