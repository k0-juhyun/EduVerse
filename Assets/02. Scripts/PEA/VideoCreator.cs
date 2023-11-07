using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections;

[System.Serializable]
public struct ServerToJson
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

public class VideoCreator : MonoBehaviour
{
    private string serverURL_GIF = "http://221.163.19.218:5052/text_2_video/sendvideo";

    private string serverURL_QUIZ = "http://221.163.19.218:5051/chat/quiz";


    public GameObject capturePreview;
    public GameObject captureResultText;

    [Space (20)]
    [Header ("Quiz")]
    // Tag �����ͼ� jsonȭ ���Ѿ� ��.
    public GameObject TagToJson;

    // ���� Ȯ�ο� �г�
    public GameObject QuizPanel;
    // ���� Question, Answer
    public Text QuestionText;
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

        Text[] T = TagToJson.GetComponentsInChildren<Text>();
        List<string> textList = new List<string>();
        foreach (Text t in T)
        {
            if (t.text == "�Է�" || t.text == "") continue;

            Debug.Log(t.text);
            // ������ T[] �迭 Jsonȭ ���Ѿ���.
            textList.Add(t.text);
        }

        ServerToJson wrapper = new ServerToJson();
        wrapper.quiz = T[0].text;
        wrapper.subject = T[1].text;

        string json = JsonUtility.ToJson(wrapper);

        Debug.Log(json);
        StartCoroutine(UploadAndDownloadCoroutine_Quiz(json));
    }

    IEnumerator UploadAndDownloadCoroutine(string imagePath)
    {
        //string imagePath = "Assets/2.jpg";  // ���ε��� �̹��� ���� ���
        //string videoId = "your_video_id";  // ������ ID

        // �̹��� ���ε�
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", File.ReadAllBytes(imagePath), "image.png", "image/png");
        using (UnityWebRequest imageUploadRequest = UnityWebRequest.Post(serverURL_QUIZ, form))
        {
            // ������ �� ������ ���.
            yield return imageUploadRequest.SendWebRequest();
            if (imageUploadRequest.result == UnityWebRequest.Result.Success)
            {
                byte[] videoData = imageUploadRequest.downloadHandler.data;
                string videoPath = Application.persistentDataPath + "/GIF/" + Time.time + ".gif";  // ������ ������ ���� ���

                if(!Directory.Exists(Application.persistentDataPath + "/GIF/"))
                {
                    Directory.CreateDirectory(Application.persistentDataPath + "/GIF/");
                }

                File.WriteAllBytes(videoPath, videoData);

                // ������ ���� ����
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
                captureResultText.GetComponent<Text>().text = "���������� �����߽��ϴ�.";
                System.Action action = () => { captureResultText.SetActive(false); };
                Invoke(nameof(action), 0.5f);
            }
            else
            {
                Debug.Log(imageUploadRequest.error);

                capturePreview.SetActive(false);
                captureResultText.GetComponent<Text>().text = "���忡 �����߽��ϴ�.";
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
                Debug.Log("JSON �����Ͱ� ���������� ������ ���۵Ǿ����ϴ�.");
                Debug.Log("���� ����: " + request.downloadHandler.text);

                string Json = request.downloadHandler.text;
                QuizData quizData = JsonUtility.FromJson<QuizData>(Json);

                // Json ���·� local�� �����.

                Debug.Log("����: " + quizData.quiz);
                // Ư�� ���ڿ� ����
                QuestionText.text = System.Text.RegularExpressions.Regex.Replace(quizData.quiz, @"����: |\(O/X\)", "");
                Debug.Log("����: " + quizData.answer);

            }
            else
            {
                Debug.LogError("JSON ������ ���� �� ���� �߻�: " + request.error);
            }
        }
    }

    public void OnQuizPanelBtnClick()
    {
        QuizPanel.SetActive(true);
    }
}