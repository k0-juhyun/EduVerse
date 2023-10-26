using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenAssetMarket : MonoBehaviour
{
    public Button Btn_Open;

    private void OnTriggerStay(Collider other)
    {
        if (DataBase.instance.userInfo.isTeacher)
        {

        }
    }
}
