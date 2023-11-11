using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetZipFile : MonoBehaviour
{
    string sendImageUrl = "http://221.163.19.218:5054/one_click_process/send3d";
    string getObjectUrl = "http://221.163.19.218:5054/one_click_process/get3d";

    public Texture2D texture2D;

    void Start()
    {
        //GetObject();

        UploadImage(texture2D);
    }

    void Update()
    {
        
    }

    public void UploadImage(Texture2D texture)
    {
        StartCoroutine(IUploadImage(texture.EncodeToPNG()));
    }

    IEnumerator IUploadImage(byte[] sendImageBytes)
    {
        // 이미지 업로드
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", sendImageBytes, "image.png", "image/png");
        using (UnityWebRequest imageUploadRequest = UnityWebRequest.Post(sendImageUrl, form))
        {
            // 응답이 올 때까지 대기.
            yield return imageUploadRequest.SendWebRequest();
            if (imageUploadRequest.result == UnityWebRequest.Result.Success)
            {
                print("성공");
                print(imageUploadRequest.downloadHandler.data);
            }
            else
            {
                print(imageUploadRequest.error);
            }
        }
    }

    public void GetObject()
    {
        StartCoroutine(IGetObject());
    }

    IEnumerator IGetObject()
    {
        WWWForm form = new WWWForm();
        form.AddField("text", "dolphin");
        using (UnityWebRequest getObjectRequest = UnityWebRequest.Post(getObjectUrl, form))
        {

            // 응답이 올 때까지 대기.
            yield return getObjectRequest.SendWebRequest();
            if (getObjectRequest.result == UnityWebRequest.Result.Success)
            {
                print("성공");
                print(getObjectRequest.downloadHandler.data);
                if(!Directory.Exists(Application.persistentDataPath + "/3D_Models/ModelDatas/"))
                {
                    Directory.CreateDirectory(Application.persistentDataPath + "/3D_Models/ModelDatas/");
                }

                File.WriteAllBytes(Application.persistentDataPath + "/3D_Models/ModelDatas/" + Time.time + ".zip", getObjectRequest.downloadHandler.data);
            }
            else
            {
                print(getObjectRequest.error);
            }
        }
    }
}
