using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PermissionManager : MonoBehaviour
{
    void Start()
    {
        Permission.RequestUserPermission(Permission.ExternalStorageRead);
        Permission.RequestUserPermission(Permission.ExternalStorageWrite);


        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }
        else
        {
            Debug.Log("읽기 권한 있음.");
        }

        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
        else
        {
            Debug.Log("쓰기 권한 있음.");
        }

    }
 
}
