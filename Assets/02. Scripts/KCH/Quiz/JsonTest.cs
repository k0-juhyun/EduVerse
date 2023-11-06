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
        wrapper.quiz = "���"; // ���� ��� �ؽ�Ʈ ����Ʈ�� �Ҵ�
        wrapper.subject = "����";

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
                Debug.Log("JSON �����Ͱ� ���������� ������ ���۵Ǿ����ϴ�.");
                Debug.Log("���� ����: " + request.downloadHandler.text);

                string Json = request.downloadHandler.text;
                QuizData quizData = JsonUtility.FromJson<QuizData>(Json);

                // Json ���·� local�� �����.

                Debug.Log("����: " + quizData.quiz);
                Debug.Log("����: " + quizData.answer);

            }
            else
            {
                Debug.LogError("JSON ������ ���� �� ���� �߻�: " + request.error);
            }
        }
    }

}
