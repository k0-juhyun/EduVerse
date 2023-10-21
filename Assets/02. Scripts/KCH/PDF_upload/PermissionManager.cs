using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PermissionManager : MonoBehaviour
{
    void Start()
    {
        // READ_EXTERNAL_STORAGE과 WRITE_EXTERNAL_STORAGE 권한을 체크합니다.
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) ||
            !Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            // 권한이 없는 경우, 사용자에게 권한 요청 대화상자를 표시합니다.
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
    }

    void OnRequestPermissionsResult(bool isGranted, string permission)
    {
        if (isGranted)
        {
            Debug.Log("권한이 허용되었습니다: " + permission);
            // 이곳에 권한이 허용된 경우의 처리를 추가합니다.
        }
        else
        {
            Debug.Log("권한이 거부되었습니다: " + permission);
            // 이곳에 권한이 거부된 경우의 처리를 추가합니다.
        }
    }
}
