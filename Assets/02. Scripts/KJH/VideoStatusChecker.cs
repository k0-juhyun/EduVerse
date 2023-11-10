using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;


public class VideoStatusChecker : MonoBehaviour
{
    private string videoStatusCheckUrl = "http://221.163.19.218:5052/text_2_video/api/video_status";
    private string serverURL_GIF = "http://221.163.19.218:5052/text_2_video/api/video_status";
    public float checkIntervalSecondsForVideo = 60f;
    public float checkIntervalSecondsForGIF = 5;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        // �ֱ����� ���� ���� Ȯ���� �����մϴ�.
        //StartCoroutine(CheckVideoStatusRoutine());
        StartCoroutine(CheckGifStatusRoutine());
    }

    private IEnumerator CheckVideoStatusRoutine()
    {
        while (true)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(videoStatusCheckUrl))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    var status = webRequest.downloadHandler.text;
                    // ���¿� ���� �ʿ��� �۾��� �����մϴ�.
                    if (status == "complete")
                    {
                        Debug.Log("Video processing is complete.");
                        // ���� �ٿ�ε� �Լ� ȣ�� ��
                        break;
                    }
                    else if (status == "processing")
                    {
                        Debug.Log("Video is still processing...");
                        // �߰����� ������ �ʿ��ϴٸ� ���⿡ �ۼ��մϴ�.
                    }
                    else if (status == "error")
                    {
                        Debug.LogError("There was an error processing the video.");
                        // ���� ó��
                        break;
                    }
                }
                else
                {
                    Debug.LogError("Error checking video status: " + webRequest.error);
                    // ���� ó��
                    break;
                }
            }

            yield return new WaitForSeconds(checkIntervalSecondsForVideo); // ������ ���ݸ�ŭ ����մϴ�.
        }
    }

    private IEnumerator CheckGifStatusRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkIntervalSecondsForGIF); // ������ ���ݸ�ŭ ����մϴ�.
            using (UnityWebRequest webRequest = UnityWebRequest.Get(serverURL_GIF))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                Debug.Log(UnityWebRequest.Result.Success);
                    var status = webRequest.downloadHandler.text;
                    // ���¿� ���� �ʿ��� �۾��� �����մϴ�.
                    if (status == "complete")
                    {
                        byte[] videoData = webRequest.downloadHandler.data;
                        string videoPath = Application.persistentDataPath + "/GIF/" + Time.time + ".gif";  // ������ ������ ���� ���

                        if (!Directory.Exists(Application.persistentDataPath + "/GIF/"))
                        {
                            Directory.CreateDirectory(Application.persistentDataPath + "/GIF/");
                        }

                        File.WriteAllBytes(videoPath, videoData);

                        Debug.Log("Video processing is complete.");
                        // ���� �ٿ�ε� �Լ� ȣ�� ��
                        break;
                    }
                    else if (status == "processing")
                    {
                        Debug.Log("Video is still processing...");
                        // �߰����� ������ �ʿ��ϴٸ� ���⿡ �ۼ��մϴ�.
                    }
                    else if (status == "error")
                    {
                        Debug.LogError("There was an error processing the video.");
                        // ���� ó��
                        break;
                    }
                }
                else
                {
                    Debug.LogError("Error checking video status: " + webRequest.error);
                    // ���� ó��
                    break;
                }
            }

        }
    }
}
