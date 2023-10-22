using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections;

public class VideoCreator : MonoBehaviour
{
    public string serverURL = "http://221.163.19.218:5051/image_to_video/send";

    private void Start()
    {

    }

    public void UploadImageAndDownloadVideo(string imagePath)
    {
        StartCoroutine(UploadAndDownloadCoroutine(imagePath));
    }

    IEnumerator UploadAndDownloadCoroutine(string imagePath)
    {
        //string imagePath = "Assets/2.jpg";  // ���ε��� �̹��� ���� ���
        //string videoId = "your_video_id";  // ������ ID

        // �̹��� ���ε�
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", File.ReadAllBytes(imagePath), "image.png", "image/png");
        using (UnityWebRequest imageUploadRequest = UnityWebRequest.Post(serverURL, form))
        {
            yield return imageUploadRequest.SendWebRequest();
            if (imageUploadRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("�̹��� ���ε� ����");

                //print(imageUploadRequest.downloadHandler.data);
                //byte[] videoData = imageUploadRequest.downloadHandler.data;
                //string videoPath = Application.dataPath + "/Resources/a.mp4";  // ������ ������ ���� ���
                //File.WriteAllBytes(videoPath, videoData);
                //Debug.Log("������ �ٿ�ε� ����");

                //�̹��� ���ε� ���� �� ������ �ٿ�ε�
                string videoURL = "http://221.163.19.218:5051/image_to_video/getvideo";
                using (UnityWebRequest videoDownloadRequest = UnityWebRequest.Get(videoURL))
                {
                    yield return videoDownloadRequest.SendWebRequest();
                    if (videoDownloadRequest.result == UnityWebRequest.Result.Success)
                    {
                        byte[] videoData = videoDownloadRequest.downloadHandler.data;
                        string videoPath = Application.dataPath + "/Resources/MyItems_Videos/" + Time.time + ".mp4";  // ������ ������ ���� ���
                        File.WriteAllBytes(videoPath, videoData);
                        Debug.Log("������ �ٿ�ε� ����");
                    }
                    else
                    {
                        Debug.Log(videoDownloadRequest.error);
                    }
                }
            }
            else
            {
                Debug.Log(imageUploadRequest.error);                
            }
        }
    }
}