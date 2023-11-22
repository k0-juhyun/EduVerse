using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneHandler : MonoBehaviour
{
    public GameObject 시작;
    public GameObject 회원가입;

    public void OnClickRegisterButton()
    {
        시작.SetActive(false);
        회원가입.SetActive(true);  
    }
}
