using UnityEngine;
using UnityEngine.Android; // Android 권한을 사용하기 위한 네임스페이스
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

    // PDF 파일을 가져오는 함수
    public void GetPDFFile()
    {
        // READ_EXTERNAL_STORAGE 권한이 부여되어 있는지 확인
        if (!Permission.HasUserAuthorizedPermission(readPermission))
        {
            // 아직 권한이 없다면 사용자에게 권한을 요청
            Permission.RequestUserPermission(readPermission);
            return;
        }

        // 권한이 부여되었을 경우 다운로드 폴더로 이동
        string downloadFolderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Download");

        // 다운로드 폴더에 있는 PDF 파일 목록 가져오기
        string[] pdfFiles = Directory.GetFiles(downloadFolderPath, "*.pdf");

        // 가져온 파일 목록을 출력
        foreach (string pdfFilePath in pdfFiles)
        {
            Debug.Log("PDF 파일 경로: " + pdfFilePath);
        }

        // 이제 가져온 PDF 파일을 로드하거나 사용할 수 있습니다.
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

        intent.Call<AndroidJavaObject>("setType", "application/pdf"); // 이 부분에서 MIME 타입을 지정하세요.

        currentActivity.Call("startActivityForResult", intent, 0);

        
    }

    void onActivityResult(string result)
    {
        Debug.Log("Selected file path: " + result);
    }
}
