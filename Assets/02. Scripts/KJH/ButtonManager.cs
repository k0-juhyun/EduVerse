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

        NetworkManager.instance.isCustom = false;
    }

    public void OnClassrommBtnClick()
    {
        NetworkManager.instance.JoinRoom("4.ClassRoomScene");
        NetworkManager.instance.enableChoose = false;
    }

    public void OnTeachersRoomBtnClick()
    {
        NetworkManager.instance.JoinRoom("4.TeachersRoomScene");
        NetworkManager.instance.enableChoose = false;
    }
}
