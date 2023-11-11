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

[System.Serializable]
public struct QuizSaveData
{
    public string quiz;
    public string subject;
    public string question;
    public string answer;
}

public class VideoCreator : MonoBehaviour
{
    private string serverURL_GIF = "http://221.163.19.218:5052/text_2_video/sendvideo";

    private string serverURL_QUIZ = "http://221.163.19.218:5051/chat/quiz";

    public GameObject captureResultTextObject;
    public Text captureResultText;

    public GameObject gifPreviewPanel;
    public GifLoad gifLoad;
    public Image gifPreviewImage;

    public Button gifCancelBtn;
    public Button gifSaveBtn;

    [Space (20)]
    [Header ("Quiz")]
    // Tag 가져와서 json화 시켜야 함.
    public GameObject TagToJson;

    // 문제 확인용 패널
    public GameObject QuizPanel;
    // 퀴즈 Question, Answer
    public Text QuestionText;

    // 로컬로 저장할 quiz 데이터
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
    }

    public void UploadImageAndDownloadVideo(string imagePath)
    {
        StartCoroutine(UploadAndDownloadCoroutine(imagePath));
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
            if (t.text == "입력" || t.text == "") continue;

            // 가져온 T[] 배열 Json화 시켜야함.
            textList.Add(t.text);
        }

        // textList[2]는 단원.
        unit = T[2].text;
        // textList[3]은 타이틀 제목.
        fileName = T[3].text;

        ServerToJson wrapper = new ServerToJson();
        wrapper.quiz = T[0].text;
        wrapper.subject = T[1].text;

        // 로컬로 저장할 데이터 넣기.
        quizsavedata_.quiz = T[0].text;
        quizsavedata_.subject = T[1].text;

        string json = JsonUtility.ToJson(wrapper);

        Debug.Log(json);
        StartCoroutine(UploadAndDownloadCoroutine_Quiz(json));
    }

    IEnumerator UploadAndDownloadCoroutine(string imagePath)
    {
        // 이미지 업로드
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", File.ReadAllBytes(imagePath), "image.png", "image/png");
        using (UnityWebRequest imageUploadRequest = UnityWebRequest.Post(serverURL_GIF, form))
        {
            // 응답이 올 때까지 대기.
            yield return imageUploadRequest.SendWebRequest();
            if (imageUploadRequest.result == UnityWebRequest.Result.Success)
            {
                byte[] videoData = imageUploadRequest.downloadHandler.data;
                gifLoad.Show(gifPreviewImage, gifLoad.GetSpritesByFrame(videoData));
                gifPreviewPanel.SetActive(true);
            }
            else
            {
                Debug.Log(imageUploadRequest.error);

                captureResultTextObject.SetActive(true);
                captureResultText.text = "GIF 생성에 실패했습니다.";
                System.Action action = () => captureResultTextObject.SetActive(false);
                Invoke(nameof(action), 0.3f);
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
                // 특정 문자열 제거
                string quiz_question = System.Text.RegularExpressions.Regex.Replace(quizData.quiz, @"퀴즈: |\(O/X\)", "");
                QuestionText.text = quiz_question;
              
                string quiz_answer = System.Text.RegularExpressions.Regex.Replace(quizData.answer, @"answer: ", "");

                if (quiz_answer == "O") incorrect.SetActive(true);               
                else if (quiz_answer == "X") wrong.SetActive(true);

                quizsavedata_.question = quiz_question;
                quizsavedata_.answer = quiz_answer;

                // 리스트에 추가.

                //"answer":"O"
            }
            else
            {
                Debug.LogError("JSON 데이터 전송 중 오류 발생: " + request.error);
            }
        }
    }

    public void OnClickGIFCancel()
    {
        gifPreviewPanel.SetActive(false);
    }

    public void GIFSave()
    {
        string videoPath = Application.persistentDataPath + "/GIF/" + Time.time + ".gif";  // 저장할 동영상 파일 경로

        if (!Directory.Exists(Application.persistentDataPath + "/GIF/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/GIF/");
        }

        gifLoad.SaveGIF(videoPath, () =>
        {
            captureResultTextObject.SetActive(true);
            if (File.Exists(videoPath))
                captureResultText.text = "성공적으로 저장했습니다.";
            else
                captureResultText.text = "저장에 실패했습니다.";
        });
    }

    // quiz panel On
    public void OnQuizPanelBtnClick()
    {
        QuizPanel.SetActive(true);
    }

    // quiz 저장 버튼
    public void OnQuizSaveBtnClick()
    {
        quizsavedata.Add(quizsavedata_);

        string filepath = Application.persistentDataPath + "/myQuizData.txt";

        SaveData quizData = new SaveData(quizsavedata_.question, quizsavedata_.answer);

        SaveSystem.Save(quizData, unit + " " + fileName);

        // MyQuizTitleData 여기에 Title 제목들을 저장. // 단원과 이름.
        SaveSystem.AppendTitleToJson("MyQuizTitleData", unit+ " " + fileName);
        Debug.Log(unit + " " + fileName);
        SaveData loadData = SaveSystem.Load(unit + " " + fileName);

        Debug.Log(loadData.question);
    }

    // quiz panel off
    public void OnQuizPanelCancelBtnClick()
    {
        Question.text = "생성중....";
        QuizPanel.SetActive(false);
    }
}