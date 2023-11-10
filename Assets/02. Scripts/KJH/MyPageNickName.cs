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
        myNickNameTxt.text = PhotonNetwork.NickName;
    }

}
