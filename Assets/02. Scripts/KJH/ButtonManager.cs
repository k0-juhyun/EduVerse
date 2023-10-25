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
        NetworkManager.instance.JoinRoom();
    }

    public void OnClassrommBtnClick()
    {
        PhotonNetwork.LoadLevel(4);
    }

    public void OnTeachersRoomBtnClick()
    {
        PhotonNetwork.LoadLevel(5);
    }
}
