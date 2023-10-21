using UnityEngine;
using System;
using UnityEngine.UI;

public class FileBrowser : MonoBehaviour
{


    public Text selectedFilePathText;
    // 로컬 파일 가져옴.
    public void OpenFileBrowser()
    {
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_OPEN_DOCUMENT"));
        intentObject.Call<AndroidJavaObject>("setType", "*/*");

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Select a file");
        currentActivity.Call("startActivityForResult", chooser, new FileSelectedCallback(selectedFilePathText));
    }

    // 파일 선택.
    private class FileSelectedCallback : AndroidJavaProxy
    {
        private Text selectedFilePathText;

        public FileSelectedCallback(Text selectedFilePathText) : base("android.content.DialogInterface$OnClickListener")
        {
            this.selectedFilePathText = selectedFilePathText;
        }

        public void onClick(AndroidJavaObject dialog, int which)
        {
            AndroidJavaObject result = dialog.Call<AndroidJavaObject>("getOwnerActivity");
            AndroidJavaObject intent = result.Call<AndroidJavaObject>("getIntent");
            AndroidJavaObject uri = intent.Call<AndroidJavaObject>("getData");
            string filePath = uri.Call<string>("getPath");
            selectedFilePathText.text = filePath;
        }
    }
}