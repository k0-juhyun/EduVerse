using UnityEngine;
using UnityEngine.Android; // Android ������ ����ϱ� ���� ���ӽ����̽�
using System.IO;
using System;
using Paroxe.PdfRenderer;

public class FilePicker : MonoBehaviour
{
    private const string readPermission = Permission.ExternalStorageRead;

    private void Start()
    {
        //GetPDFFile();
        PickFile();
    }

    // PDF ������ �������� �Լ�
    public void GetPDFFile()
    {
        // READ_EXTERNAL_STORAGE ������ �ο��Ǿ� �ִ��� Ȯ��
        if (!Permission.HasUserAuthorizedPermission(readPermission))
        {
            // ���� ������ ���ٸ� ����ڿ��� ������ ��û
            Permission.RequestUserPermission(readPermission);
            return;
        }

        // ������ �ο��Ǿ��� ��� �ٿ�ε� ������ �̵�
        string downloadFolderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Download");

        // �ٿ�ε� ������ �ִ� PDF ���� ��� ��������
        string[] pdfFiles = Directory.GetFiles(downloadFolderPath, "*.pdf");

        // ������ ���� ����� ���
        foreach (string pdfFilePath in pdfFiles)
        {
            Debug.Log("PDF ���� ���: " + pdfFilePath);
        }

        // ���� ������ PDF ������ �ε��ϰų� ����� �� �ֽ��ϴ�.
    }

    public void OpenFile(string filePath)
    {
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

        // Set action to view
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_VIEW"));

        // Set data and type
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + filePath);
        intentObject.Call<AndroidJavaObject>("setDataAndType", uriObject, "application/pdf");

        // Get current activity and start intent
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("startActivity", intentObject);
    }

    public void PickFile()
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        string action = intentClass.GetStatic<string>("ACTION_GET_CONTENT");
        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", action);

        intent.Call<AndroidJavaObject>("setType", "application/pdf"); // �� �κп��� MIME Ÿ���� �����ϼ���.

        currentActivity.Call("startActivityForResult", intent, 0);

        
    }

    void onActivityResult(string result)
    {
        Debug.Log("Selected file path: " + result);
    }
}
