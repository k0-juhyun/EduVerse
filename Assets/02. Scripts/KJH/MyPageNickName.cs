using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyPageNickName : MonoBehaviour
{
    public Text myNickNameTxt;
    public string myNickName;

    Scene scene;
    void Start()
    {
        myNickNameTxt.text = PhotonNetwork.NickName;
    }

    private void Update()
    {
        if ((scene.buildIndex == 2 && scene.buildIndex == 3) == false)
            myNickNameTxt.transform.forward = Camera.main.transform.forward;
    }
}
