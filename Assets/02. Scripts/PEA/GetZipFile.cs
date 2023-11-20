using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GetZipFile : MonoBehaviour
{
    public static GetZipFile instance = null;

    string sendImageUrl = "http://221.163.19.218:5054/one_click_process/send3d";
    string getObjectUrl = "http://221.163.19.218:5054/one_click_process/get3d";
    string imagePath;

    public Texture2D texture2D;
    private Unzip unzip;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        unzip = GetComponent<Unzip>();
        //GetObject();

        //imagePath = Application.dataPath + "/Resources/DOG.png";
        //UploadImage();
    }

    void Update()
    {
        
    }

    public void UploadImage(byte[] imageBytes, string imageTag)
    {
        StartCoroutine(IUploadImage(imageBytes, imageTag));
    }

    IEnumerator IUploadImage(byte[] imageBytes, string imageTag)
    {
        // 이미지 업로드
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", imageBytes, imageTag + ".png", "image/png");
        using (UnityWebRequest imageUploadRequest = UnityWebRequest.Post(sendImageUrl, form))
        {
            // 응답이 올 때까지 대기.
            yield return imageUploadRequest.SendWebRequest();
            if (imageUploadRequest.result == UnityWebRequest.Result.Success)
            {
                print("성공");
                print(imageUploadRequest.downloadHandler.data);
                GameManager.Instance.AddWaitMakeObjectTag(imageTag);
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
        //form.AddField("text", GameManager.Instance.GetMakeObejctTag());
        form.AddField("text", "chipmunk");
        using (UnityWebRequest getObjectRequest = UnityWebRequest.Post(getObjectUrl, form))
        {

            // 응답이 올 때까지 대기.
            yield return getObjectRequest.SendWebRequest();
            //if (getObjectRequest.result == UnityWebRequest.Result.Success)
            //{
            //    print("성공");
            //    print(getObjectRequest.downloadHandler.data);
            //    if(!Directory.Exists(Application.persistentDataPath + "/3D_Models/ModelDatas/"))
            //    {
            //        Directory.CreateDirectory(Application.persistentDataPath + "/3D_Models/ModelDatas/");
            //    }

            //    string zipPath = Application.persistentDataPath + "/3D_Models/ModelDatas/" + Time.time + ".zip";
            //    File.WriteAllBytes(zipPath, getObjectRequest.downloadHandler.data);
            //    unzip.RunZip(zipPath);
            //}
            if (getObjectRequest.result == UnityWebRequest.Result.Success)
            {
                print("성공");
                print(getObjectRequest.downloadHandler.data);
                if(!Directory.Exists(Application.persistentDataPath + "/3D_Models/ModelDatas/"))
                {
                    Directory.CreateDirectory(Application.persistentDataPath + "/3D_Models/ModelDatas/");
                }

                string zipPath = Application.persistentDataPath + "/3D_Models/ModelDatas/" + Time.time + ".zip";
                File.WriteAllBytes(zipPath, getObjectRequest.downloadHandler.data);
                unzip.RunZip(zipPath);
            }
            else
            {
                print(getObjectRequest.error);
            }
        }
    }
}
