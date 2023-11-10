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
        // 주기적인 비디오 상태 확인을 시작합니다.
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
                    // 상태에 따라 필요한 작업을 수행합니다.
                    if (status == "complete")
                    {
                        Debug.Log("Video processing is complete.");
                        // 비디오 다운로드 함수 호출 등
                        break;
                    }
                    else if (status == "processing")
                    {
                        Debug.Log("Video is still processing...");
                        // 추가적인 로직이 필요하다면 여기에 작성합니다.
                    }
                    else if (status == "error")
                    {
                        Debug.LogError("There was an error processing the video.");
                        // 오류 처리
                        break;
                    }
                }
                else
                {
                    Debug.LogError("Error checking video status: " + webRequest.error);
                    // 오류 처리
                    break;
                }
            }

            yield return new WaitForSeconds(checkIntervalSecondsForVideo); // 설정한 간격만큼 대기합니다.
        }
    }

    private IEnumerator CheckGifStatusRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkIntervalSecondsForGIF); // 설정한 간격만큼 대기합니다.
            using (UnityWebRequest webRequest = UnityWebRequest.Get(serverURL_GIF))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                Debug.Log(UnityWebRequest.Result.Success);
                    var status = webRequest.downloadHandler.text;
                    // 상태에 따라 필요한 작업을 수행합니다.
                    if (status == "complete")
                    {
                        byte[] videoData = webRequest.downloadHandler.data;
                        string videoPath = Application.persistentDataPath + "/GIF/" + Time.time + ".gif";  // 저장할 동영상 파일 경로

                        if (!Directory.Exists(Application.persistentDataPath + "/GIF/"))
                        {
                            Directory.CreateDirectory(Application.persistentDataPath + "/GIF/");
                        }

                        File.WriteAllBytes(videoPath, videoData);

                        Debug.Log("Video processing is complete.");
                        // 비디오 다운로드 함수 호출 등
                        break;
                    }
                    else if (status == "processing")
                    {
                        Debug.Log("Video is still processing...");
                        // 추가적인 로직이 필요하다면 여기에 작성합니다.
                    }
                    else if (status == "error")
                    {
                        Debug.LogError("There was an error processing the video.");
                        // 오류 처리
                        break;
                    }
                }
                else
                {
                    Debug.LogError("Error checking video status: " + webRequest.error);
                    // 오류 처리
                    break;
                }
            }

        }
    }
}
