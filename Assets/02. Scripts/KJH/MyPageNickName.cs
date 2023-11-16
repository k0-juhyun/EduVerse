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
            myNickNameText.text = PhotonNetwork.NickName + "  ¼±»ý´Ô | ·Î±×¾Æ¿ô";
        else
            myNickNameText.text = PhotonNetwork.NickName + "  ÇÐ»ý | ·Î±×¾Æ¿ô";

    }

}
