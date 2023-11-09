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

    public Camera cam;

    Scene scene;
    void Start()
    {
        cam = cam?.GetComponent<Camera>();
        myNickNameTxt.text = PhotonNetwork.NickName;
    }

    private void Update()
    {
        if ((scene.buildIndex == 2 && scene.buildIndex == 3) == false && cam != null)
            myNickNameTxt.transform.forward = cam.transform.forward;
    }
}
