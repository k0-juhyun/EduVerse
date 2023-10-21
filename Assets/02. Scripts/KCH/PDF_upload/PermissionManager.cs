using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PermissionManager : MonoBehaviour
{
    void Start()
    {
        // READ_EXTERNAL_STORAGE�� WRITE_EXTERNAL_STORAGE ������ üũ�մϴ�.
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) ||
            !Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            // ������ ���� ���, ����ڿ��� ���� ��û ��ȭ���ڸ� ǥ���մϴ�.
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
    }

    void OnRequestPermissionsResult(bool isGranted, string permission)
    {
        if (isGranted)
        {
            Debug.Log("������ ���Ǿ����ϴ�: " + permission);
            // �̰��� ������ ���� ����� ó���� �߰��մϴ�.
        }
        else
        {
            Debug.Log("������ �źεǾ����ϴ�: " + permission);
            // �̰��� ������ �źε� ����� ó���� �߰��մϴ�.
        }
    }
}
