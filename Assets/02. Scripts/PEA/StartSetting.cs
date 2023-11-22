using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using static System.Net.Mime.MediaTypeNames;

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

    public void OnClickVerifyButton()
    {
        DataBase.instance.user.isTeacher = true;
        print("12");
    }
}
