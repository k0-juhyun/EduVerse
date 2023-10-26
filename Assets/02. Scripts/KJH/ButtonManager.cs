using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ButtonManager : MonoBehaviour
{
    public void OnSaveBtnClick()
    {
        GameManager.Instance.SaveCharacterInfo();
    }

    public void OnLoadNextBtnClick()
    {
        if (DataBase.instance.userInfo.isTeacher)
            PhotonNetwork.LoadLevel(2);

        if (DataBase.instance.userInfo.isTeacher == false)
            PhotonNetwork.LoadLevel(3);
    }

    public void OnClassrommBtnClick()
    {
        NetworkManager.instance.JoinRoom();
    }

    public void OnTeachersRoomBtnClick()
    {
        PhotonNetwork.LoadLevel(5);
    }
}
