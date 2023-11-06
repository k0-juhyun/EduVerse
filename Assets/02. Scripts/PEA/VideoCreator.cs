using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections;

public class VideoCreator : MonoBehaviour
{
    private string serverURL_GIF = "http://221.163.19.218:5052/text_2_video/sendvideo";

    private string serverURL_QUIZ = "http://221.163.19.218:5051/chat/quiz";


    public GameObject capturePreview;
    public GameObject captureResultText;

    private void Start()
    {

    }

    public void UploadImageAndDownloadVideo(string imagePath)
    {
        StartCoroutine(UploadAndDownloadCoroutine(imagePath));
    }

    public void UploadImageAndDownloadQuiz()
    {
        //StartCoroutine(UploadAndDownloadCoroutine_Quiz());
    }

    IEnumerator UploadAndDownloadCoroutine(string imagePath)
    {
        //string imagePath = "Assets/2.jpg";  // 업로드할 이미지 파일 경로
        //string videoId = "your_video_id";  // 동영상 ID

        // 이미지 업로드
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", File.ReadAllBytes(imagePath), "image.png", "image/png");
        using (UnityWebRequest imageUploadRequest = UnityWebRequest.Post(serverURL_QUIZ, form))
        {
            // 응답이 올 때까지 대기.
            yield return imageUploadRequest.SendWebRequest();
            if (imageUploadRequest.result == UnityWebRequest.Result.Success)
            {
                byte[] videoData = imageUploadRequest.downloadHandler.data;
                string videoPath = Application.persistentDataPath + "/GIF/" + Time.time + ".gif";  // 저장할 동영상 파일 경로

                if(!Directory.Exists(Application.persistentDataPath + "/GIF/"))
                {
                    Directory.CreateDirectory(Application.persistentDataPath + "/GIF/");
                }

                File.WriteAllBytes(videoPath, videoData);

                // 아이템 정보 저장
                Item item = new Item(Item.ItemType.Video, Time.time.ToString(), videoPath);
                string json = "";
                MyItems myItems = new MyItems();

                if(File.Exists(Application.persistentDataPath + "/MyItems.txt"))
                {
                    byte[] jsonBytes = File.ReadAllBytes(Application.persistentDataPath + "/MyItems.txt");
                    json = Encoding.UTF8.GetString(jsonBytes);
                    myItems = JsonUtility.FromJson<MyItems>(json);
                }
                else
                {
                    myItems.data = new List<Item>();
                }
                            
                myItems.data.Add(item);
                json = JsonUtility.ToJson(myItems);
                File.WriteAllText(Application.persistentDataPath + "/MyItems.txt", json);

                capturePreview.SetActive(false);
                captureResultText.GetComponent<Text>().text = "성공적으로 저장했습니다.";
                System.Action action = () => { captureResultText.SetActive(false); };
                Invoke(nameof(action), 0.5f);
            }
            else
            {
                Debug.Log(imageUploadRequest.error);

                capturePreview.SetActive(false);
                captureResultText.GetComponent<Text>().text = "저장에 실패했습니다.";
                System.Action action = () => { captureResultText.SetActive(false); };
                Invoke(nameof(action), 0.5f);
            }
        }
    }

    IEnumerator UploadAndDownloadCoroutine_Quiz(string json)
    {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(serverURL_QUIZ, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("JSON 데이터가 성공적으로 서버로 전송되었습니다.");
                Debug.Log("서버 응답: " + request.downloadHandler.text);

                string Json = request.downloadHandler.text;
                QuizData quizData = JsonUtility.FromJson<QuizData>(Json);

                // Json 형태로 local로 저장됌.

                Debug.Log("퀴즈: " + quizData.quiz);
                Debug.Log("정답: " + quizData.answer);

            }
            else
            {
                Debug.LogError("JSON 데이터 전송 중 오류 발생: " + request.error);
            }
        }
    }
}