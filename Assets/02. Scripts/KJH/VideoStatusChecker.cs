using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class VideoStatusChecker : MonoBehaviour
{
    private string videoStatusCheckUrl = "http://221.163.19.218:5052/text_2_video/api/video_status";
    public float checkIntervalSeconds = 60f;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        // �ֱ����� ���� ���� Ȯ���� �����մϴ�.
        StartCoroutine(CheckVideoStatusRoutine());
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

            yield return new WaitForSeconds(checkIntervalSeconds); // ������ ���ݸ�ŭ ����մϴ�.
        }
    }
}
