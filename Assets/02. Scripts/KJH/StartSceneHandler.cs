using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneHandler : MonoBehaviour
{
    public GameObject ����;
    public GameObject ȸ������;

    public void OnClickRegisterButton()
    {
        ����.SetActive(false);
        ȸ������.SetActive(true);  
    }
}
