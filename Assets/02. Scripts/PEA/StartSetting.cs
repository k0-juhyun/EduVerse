using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class StartSetting : MonoBehaviour
{
    public InputField nameInput;
    public Toggle isTeacherTpggle;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnCllickStartBtn()
    {
        if(nameInput.text.Length > 0)
        {
            PlayerManager.instance.SetMyInfo( new User(nameInput.text, isTeacherTpggle.isOn));
            if(PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.LoadLevel(1);
            }
        }
    }
}