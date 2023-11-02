using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ButtonManager : MonoBehaviourPun
{
    private bool focusStudents;

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

    public void OnFocusBtnClick()
    {
        if (!DataBase.instance.userInfo.isTeacher)
            return;
    }

    private void OnTriggerEnter(Collider other)
    {
        focusStudents = !focusStudents;
    }

    private void OnTriggerExit(Collider other)
    {
        focusStudents = !focusStudents;
    }
}
