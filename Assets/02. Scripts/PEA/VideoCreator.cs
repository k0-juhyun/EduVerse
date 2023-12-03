using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections;
using System;
using UnityEngine.Video;
using DG.Tweening;

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
    public string Commentary;
}

[System.Serializable]
public struct QuizSaveData
{
    public string quiz;
    public string subject;
    public string question;
    public string answer;
    public string commentary;
}

public class VideoCreator : MonoBehaviour
{
    private string serverURL_GIF = "http://221.163.19.218:5052/text_2_video/sendvideo";

    private string serverURL_QUIZ = "http://221.163.19.218:5056/chat/quiz2";

    private string serverURL_Video = "http://221.163.19.218:5055/video_crafter/text_2_video";

    // �� ��� ��ó��
    public GameObject backBlur;

    // ĸ�ĺ� Ȯ��
    public GameObject captureResultTextObject;
    public Text captureResultText;

    // gif �̸�����
    public GameObject gifPreviewPanel;
    public GifLoad gifLoad;
    public Image gifPreviewImage;

    // ���� �̸�����
    public GameObject videoPreviewPanel;
    public RawImage videoRawImage;
    public VideoPlayer videoPlayer;

    // ��ư
    public Button gifCancelBtn;
    public Button gifSaveBtn;
    public Button videoCancelBtn;
    public Button videoSaveBtn;

    [Space (20)]
    [Header ("Quiz")]
    // Tag �����ͼ� jsonȭ ���Ѿ� ��.
    public GameObject TagToJson;

    // ���� Ȯ�ο� �г�
    public GameObject QuizPanel;
    // ���� Question, Answer
    public Text QuestionText;

    // ���÷� ������ quiz ������
    public List<QuizSaveData> quizsavedata;
    QuizSaveData quizsavedata_;

    string unit;
    string fileName;

    public Text Question;
    public GameObject incorrect;
    public GameObject wrong;

    private void Start()
    {
        gifCancelBtn.onClick.AddListener(OnClickGIFCancel);
        gifSaveBtn.onClick.AddListener(GIFSave);

        videoCancelBtn.onClick.AddListener(OnClickVideoCancel);
        videoSaveBtn.onClick.AddListener(VideoSave);
    }

    public void UploadImageAndDownload_GIF(byte[] imageBytes, System.Action action = null)
    {
        StartCoroutine(UploadAndDownloadCoroutine_GIF(imageBytes, action));
    }

    public void UploadImageAndDownloadQuiz()
    {
        //StartCoroutine(UploadAndDownloadCoroutine_Quiz());

        quizsavedata_.quiz = null;
        quizsavedata_.subject = null;
        quizsavedata_.question = null;
        quizsavedata_.answer = null;

        Text[] T = TagToJson.GetComponentsInChildren<Text>();
        List<string> textList = new List<string>();
        foreach (Text t in T)
        {
            if (t.text == "�Է�" || t.text == "") continue;

            // ������ T[] �迭 Jsonȭ ���Ѿ���.
            textList.Add(t.text);
        }

        // textList[2]�� �ܿ�.
        unit = T[2].text;
        // textList[3]�� Ÿ��Ʋ ����.
        fileName = T[3].text;

        ServerToJson wrapper = new ServerToJson();
        wrapper.quiz = T[0].text;
        wrapper.subject = T[1].text;

        // ���÷� ������ ������ �ֱ�.
        quizsavedata_.quiz = T[0].text;
        quizsavedata_.subject = T[1].text;

        string json = JsonUtility.ToJson(wrapper);

        Debug.Log(json);

        // ���� ����
        //StartCoroutine(UploadAndDownloadCoroutine_Quiz(json));

        // �ÿ� ����
        StartCoroutine(example());
    }

    IEnumerator example()
    {
        yield return new WaitForSeconds(3);

        QuestionText.text = "���°迡���� �����ڴ� ����ü�� ���⹰�� �����Ͽ� ������� ������ ������ �ϴµ�, �̴� ���°��� ��ȯ�� ���� �߿��� ������ �Ѵ�.";

        incorrect.SetActive(true);

    }

    public void UploadImageAndDownload_Video(byte[] imageBytes, System.Action action = null)
    {
        StartCoroutine(UploadAndDownloadCoroutine_Video(imageBytes));
    }

    IEnumerator UploadAndDownloadCoroutine_GIF(byte[] imageBytes, System.Action action = null)
    {
        // �̹��� ���ε�
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", imageBytes, "image.png", "image/png");
        using (UnityWebRequest imageUploadRequest = UnityWebRequest.Post(serverURL_GIF, form))
        {
            // ������ �� ������ ���.
            yield return imageUploadRequest.SendWebRequest();
            if (imageUploadRequest.result == UnityWebRequest.Result.Success)
            {
                print("����");
                byte[] videoData = imageUploadRequest.downloadHandler.data;
                (Sprite[], float) gifInfo = gifLoad.GetSpritesByFrame(videoData);
                gifLoad.Show(gifPreviewImage, gifInfo.Item1, gifInfo.Item2);
                action();
                gifPreviewPanel.SetActive(true);
                Blur(true);
            }
            else
            {
                Debug.Log(imageUploadRequest.error);

                captureResultTextObject.SetActive(true);
                captureResultText.text = "GIF ������ �����߽��ϴ�.";
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
                string quiz_question = System.Text.RegularExpressions.Regex.Replace(quizData.quiz, @"����: |\(O/X\)", "");
                QuestionText.text = quiz_question;
              
                string quiz_answer = System.Text.RegularExpressions.Regex.Replace(quizData.answer, @"answer: ", "");

                string quiz_commentary = System.Text.RegularExpressions.Regex.Replace(quizData.Commentary, @"�½��ϴ�. ", "");

                Debug.Log(quiz_commentary);
                if (quiz_answer == "O") incorrect.SetActive(true);               
                else if (quiz_answer == "X") wrong.SetActive(true);

                quizsavedata_.question = quiz_question;
                quizsavedata_.answer = quiz_answer;
                quizsavedata_.commentary = quizData.Commentary;

                // ����Ʈ�� �߰�.

                //"answer":"O"
            }
            else
            {
                Debug.LogError("JSON ������ ���� �� ���� �߻�: " + request.error);
            }
        }
    }

    IEnumerator UploadAndDownloadCoroutine_Video(byte[] imageBytes, System.Action action = null)
    {
        // �̹��� ���ε�
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", imageBytes, "image.png", "image/png");
        using (UnityWebRequest imageUploadRequest = UnityWebRequest.Post(serverURL_Video, form))
        {
            // ������ �� ������ ���.
            yield return imageUploadRequest.SendWebRequest();
            if (imageUploadRequest.result == UnityWebRequest.Result.Success)
            {
                print("����");
                byte[] videoData = imageUploadRequest.downloadHandler.data;

                if (!Directory.Exists(Application.persistentDataPath + "/Videos/"))
                {
                    Directory.CreateDirectory(Application.persistentDataPath + "/Videos/");
                }

                string videoPath = Application.persistentDataPath + "/Videos/" + DateTime.Now.ToString(("yyyy-MM-dd HH.mm.ss")) + ".mp4";
                File.WriteAllBytes(videoPath, videoData);
                videoPlayer.url = videoPath;
                videoPreviewPanel.SetActive(true);
                Blur(true);
            }
            else
            {
                Debug.Log(imageUploadRequest.error);

                captureResultTextObject.SetActive(true);
                captureResultText.text = "���� ������ �����߽��ϴ�.";
            }
        }
    }

    public void OnClickGIFCancel()
    {
        gifPreviewPanel.SetActive(false);
        Blur(false);
    }

    public void GIFSave()
    {
        string videoPath = Application.persistentDataPath + "/GIF/" + DateTime.Now.ToString(("yyyy-MM-dd HH.mm.ss")) + ".gif";  // ������ ������ ���� ���

        if (!Directory.Exists(Application.persistentDataPath + "/GIF/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/GIF/");
        }

        gifLoad.SaveGIF(videoPath, () =>
        {
            captureResultTextObject.SetActive(true);
            if (File.Exists(videoPath))
                captureResultText.text = "���������� �����߽��ϴ�.";
            else
                captureResultText.text = "���忡 �����߽��ϴ�.";
        });
    }

    public void OnClickVideoCancel()
    {
        videoPreviewPanel.SetActive(false);
        Blur(false);

        // ������ ���ڸ��� ���� -> ������ ���� ���� ����
        File.Delete(videoPlayer.url);
    }

    public void VideoSave()
    {
        // ������ ���� ����
        Item item = new Item(Item.ItemType.GIF, Path.GetFileNameWithoutExtension(videoPlayer.url), videoPlayer.url);

        MyItemsManager.instance.AddItem(item);
        //string json = "";
        //MyItems myItems = new MyItems();

        //if (File.Exists(Application.persistentDataPath + "/MyItems.txt"))
        //{
        //    byte[] jsonBytes = File.ReadAllBytes(Application.persistentDataPath + "/MyItems.txt");
        //    json = Encoding.UTF8.GetString(jsonBytes);
        //    myItems = JsonUtility.FromJson<MyItems>(json);
        //}
        //else
        //{
        //    myItems.data = new List<Item>();
        //}

        //myItems.data.Add(item);
        //json = JsonUtility.ToJson(myItems);
        //File.WriteAllText(Application.persistentDataPath + "/MyItems.txt", json);
    }

    // quiz panel On
    public void OnQuizPanelBtnClick()
    {
        QuizPanel.SetActive(true);
        Blur(true);
    }

    // quiz ���� ��ư
    public void OnQuizSaveBtnClick()
    {
        quizsavedata.Add(quizsavedata_);

        string filepath = Application.persistentDataPath + "/myQuizData.txt";

        // �ҷ��� ���� ����.
        SaveData quizData = new SaveData(quizsavedata_.question, quizsavedata_.answer,quizsavedata_.commentary);

        SaveSystem.Save(quizData, unit + " " + fileName);

        // MyQuizTitleData ���⿡ Title ������� ����. // �ܿ��� �̸�.
        SaveSystem.AppendTitleToJson("MyQuizTitleData", unit+ " " + fileName);
        Debug.Log(unit + " " + fileName);
        SaveData loadData = SaveSystem.Load(unit + " " + fileName);

        Debug.Log(loadData.question);
    }

    // quiz panel off
    // ���� ������ �г�
    public void OnQuizPanelCancelBtnClick()
    {
        Question.text = "������....";
        QuizPanel.transform.DOScale(new Vector3(0.1f,0.1f,0.1f), 0.5f).SetEase(Ease.InBack).OnComplete(() => QuizPanel.SetActive(false));

        incorrect.SetActive(false);
        wrong.SetActive(false);
        Blur(false);
    }

    private void Blur(bool isBlur)
    {
        backBlur.SetActive(isBlur);
    }
}