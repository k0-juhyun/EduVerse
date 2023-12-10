using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ButtonManager : MonoBehaviourPun
{
    public void OnSaveBtnClick()
    {
        GameManager.Instance.SaveCharacterInfo();
    }

    public void OnLoadNextBtnClick()
    {
        if(!GameManager.Instance.iscustomScene_classroom && !GameManager.Instance.iscustomScene_ground)
        {
            if (DataBase.instance.user.isTeacher)
                PhotonNetwork.LoadLevel(2);

            if (DataBase.instance.user.isTeacher == false)
                PhotonNetwork.LoadLevel(3);
        }

        // ±≥Ω«
        if (GameManager.Instance.iscustomScene_classroom)
        {
            PhotonNetwork.LoadLevel("LoadingScene");
            NetworkManager.instance.JoinRoom("4.ClassRoomScene");
            NetworkManager.instance.enableChoose = false;
            GameManager.Instance.iscustomScene_classroom=false;
        }

        // ±§¿Â
        if (GameManager.Instance.iscustomScene_ground)
        {
            PhotonNetwork.LoadLevel("LoadingScene");
            NetworkManager.instance.JoinRoom("5.GroundScene");
            NetworkManager.instance.enableChoose = false;
            GameManager.Instance.iscustomScene_ground = false;
        }

        NetworkManager.instance.isCustom = false;
    }

    public void OnClassrommBtnClick()
    {
        PhotonNetwork.LoadLevel("LoadingScene");
        NetworkManager.instance.JoinRoom("4.ClassRoomScene"); 
        NetworkManager.instance.enableChoose = false;
    }

    public void OnGroundBtnClick()
    {
        PhotonNetwork.LoadLevel("LoadingScene");
        NetworkManager.instance.JoinRoom("5.GroundScene");
        NetworkManager.instance.enableChoose = false;
    }

    //public void OnTeachersRoomBtnClick()
    //{
    //    PhotonNetwork.LoadLevel("LoadingScene");
    //    NetworkManager.instance.JoinRoom("4.TeachersRoomScene"); 
    //    NetworkManager.instance.enableChoose = false;
    //}

    public void OnStudentMyPageBtnClick()
    {
        PhotonNetwork.LoadLevel("MyPage_Student");
    }

    public void OnClickBackToStartScene()
    {
        PhotonNetwork.LoadLevel(0);
    }
}
