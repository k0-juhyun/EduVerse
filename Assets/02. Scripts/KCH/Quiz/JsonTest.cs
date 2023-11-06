using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public struct TextListWrapper
{
    public string quiz;
    public string subject;
}

[System.Serializable]
public struct QuizData
{
    public string quiz;
    public string answer;
}


public class JsonTest : MonoBehaviour
{
    public Text[] textArray;

    private string serverURL = "http://221.163.19.218:5051/chat/quiz";

    void Start()
    {
        TextListWrapper wrapper = new TextListWrapper();
        wrapper.quiz = "용암"; // 예를 들어 텍스트 리스트를 할당
        wrapper.subject = "과학";

        string json = JsonUtility.ToJson(wrapper);

        StartCoroutine(UploadJsonToServer(json));

        Debug.Log(json);
    }

    IEnumerator UploadJsonToServer(string json)
    {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(serverURL, "POST"))
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
