using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections;

public class VideoCreator : MonoBehaviour
{
    private string serverURL = "http://221.163.19.218:5052/text_2_video/sendvideo";

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
                //string videoURL = "http://221.163.19.218:5052/image_to_video/getvideo";
                //using (UnityWebRequest videoDownloadRequest = UnityWebRequest.Get(videoURL))
                //{
                //    yield return videoDownloadRequest.SendWebRequest();
                //    if (videoDownloadRequest.result == UnityWebRequest.Result.Success)
                //    {
                //    }
                //    else
                //    {
                //        Debug.Log(videoDownloadRequest.error);
                //    }
                //}
                byte[] videoData = imageUploadRequest.downloadHandler.data;
                string videoPath = Application.persistentDataPath + Time.time + ".gif";  // ������ ������ ���� ���
                File.WriteAllBytes(videoPath, videoData);
                Debug.Log("������ �ٿ�ε� ����");
            }
            else
            {
                Debug.Log(imageUploadRequest.error);                
            }
        }
    }
}