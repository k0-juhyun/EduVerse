using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPageNickName : MonoBehaviour
{
    public Text myNickNameTxt;
    public string myNickName;

    void Start()
    {
        myNickNameTxt.text = PhotonNetwork.NickName + " ¼±»ý´Ô | ·Î±×¾Æ¿ô";
    }
}
