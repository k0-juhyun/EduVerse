using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.PUN;

[System.Serializable]
public class QuizInfo
{
    public int QuizAnswerCnt;
    public int QuizCorrectAnswerCnt;
    public answerinfo Unit_1;
    public answerinfo Unit_2;
    public answerinfo Unit_3;
    public answerinfo Unit_4;
    public answerinfo Unit_5;


    public QuizInfo(int answerCnt, int correctAnswerCnt, answerinfo unit1, answerinfo unit2, answerinfo unit3, answerinfo unit4, answerinfo unit5)
    {
        QuizAnswerCnt = answerCnt;
        QuizCorrectAnswerCnt = correctAnswerCnt;
        Unit_1 = unit1;
        Unit_2 = unit2;
        Unit_3 = unit3;
        Unit_4 = unit4;
        Unit_5 = unit5;
    }
}

[System.Serializable]
public class answerinfo
{
    public List<titleinfo> CorrectAnswer;
    public List<titleinfo> IncorrectAnswer;

    public answerinfo()
    {
        CorrectAnswer = new List<titleinfo>();
        IncorrectAnswer = new List<titleinfo>();
    }
}

[System.Serializable]
public class titleinfo
{
    public string Title;
    public string Answer;
    public string Commentary;
    public titleinfo(string Title_, string Answer_, string commentary)
    {
        this.Title = Title_;
        this.Answer = Answer_;
        Commentary = commentary;
    }
}

// 지금 테스트용으로 StudentDB에 넣어놈 나중에 뺴야됌.

public class QuizToFireBase : MonoBehaviour
{
    DatabaseReference reference;
    FirebaseDatabase database;

    static public QuizToFireBase instance;

    // 불러온 데이터 저장
    [HideInInspector]
    public QuizInfo LoadQuizInfo;

    // 문제 데이터 저장.
    [HideInInspector] public string Unit;
    [HideInInspector] public string Question;
    [HideInInspector] public string Answer;
    [HideInInspector] public int submitQuizCnt;
    [HideInInspector] public int CorrectQuizCnt;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    TestAddData();
        //}
    }

    // 단원 문제 answer 정답인지 오답인지.
    public void QuizDataSaveFun(string unit_, string question_, string answer_, string commentary_, bool result_)
    {

        database = FirebaseDatabase.DefaultInstance;

        // 사용자의 ID 가져오기
        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        // 기존 데이터 가져오기
        string path = "Quiz_INFO/" + userId;
        StartCoroutine(ReadExistingData(path, unit_, question_, answer_, commentary_, result_));
    }

    // 현재 있는 데이터에 값 추가.
    IEnumerator ReadExistingData(string path, string unit_, string question_, string answer_, string commentary_, bool result_)
    {
        var task = database.GetReference(path).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.Log("데이터 읽기 실패: " + task.Exception);
            yield break;
        }

        DataSnapshot snapshot = task.Result;

        // 기존 데이터 불러오기
        QuizInfo existingQuizInfo = JsonUtility.FromJson<QuizInfo>(snapshot.GetRawJsonValue());

        // 만약 기존 데이터가 없으면, 경로를 생성한 후 데이터를 추가합니다.
        if (existingQuizInfo == null)
        {
            existingQuizInfo = new QuizInfo(0, 0, new answerinfo(), new answerinfo(), new answerinfo(), new answerinfo(), new answerinfo());
            var createTask = database.GetReference(path).SetRawJsonValueAsync(JsonUtility.ToJson(existingQuizInfo));

            yield return new WaitUntil(() => createTask.IsCompleted);

            if (createTask.Exception != null)
            {
                Debug.Log("데이터 생성 실패: " + createTask.Exception);
                yield break;
            }
        }

        Debug.Log("문제 "+question_);
        Debug.Log("unit_ " + unit_);

        Debug.Log("answer_ " + answer_);
        Debug.Log("commentary_ " + commentary_);

        // 이후에 새로운 데이터를 추가합니다.
        titleinfo newQuestion = new titleinfo(question_, answer_, commentary_);


        // 맞으면
        if (result_)
        {
            // 단원 별로 문제 추가.
            switch (unit_)
            {
                case "1단원":
                    existingQuizInfo.Unit_1.CorrectAnswer.Add(newQuestion);
                    break;
                case "2단원":
                    existingQuizInfo.Unit_2.CorrectAnswer.Add(newQuestion);
                    break;
                case "3단원":
                    existingQuizInfo.Unit_3.CorrectAnswer.Add(newQuestion);
                    break;
                case "4단원":
                    existingQuizInfo.Unit_4.CorrectAnswer.Add(newQuestion);
                    break;
                case "5단원":
                    existingQuizInfo.Unit_5.CorrectAnswer.Add(newQuestion);
                    break;
            }
            existingQuizInfo.QuizAnswerCnt++;
            existingQuizInfo.QuizCorrectAnswerCnt++;
        }
        //틀리면
        else
        {
            // 단원 별로 문제 추가.
            switch (unit_)
            {
                case "1단원":
                    existingQuizInfo.Unit_1.IncorrectAnswer.Add(newQuestion);
                    break;
                case "2단원":
                    existingQuizInfo.Unit_2.IncorrectAnswer.Add(newQuestion);
                    break;
                case "3단원":
                    existingQuizInfo.Unit_3.IncorrectAnswer.Add(newQuestion);
                    break;
                case "4단원":
                    existingQuizInfo.Unit_4.IncorrectAnswer.Add(newQuestion);
                    break;
                case "5단원":
                    existingQuizInfo.Unit_5.IncorrectAnswer.Add(newQuestion);
                    break;
            }
            existingQuizInfo.QuizAnswerCnt++;
        }

        // Firebase에 업데이트
        StartCoroutine(UpdateDataToFirebase(existingQuizInfo));
    }

    IEnumerator UpdateDataToFirebase(QuizInfo quizInfo)
    {
        string path = "Quiz_INFO/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        var task = database.GetReference(path).SetRawJsonValueAsync(JsonUtility.ToJson(quizInfo));
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception == null)
        {
            Debug.Log("퀴즈 정보 업데이트 성공");
        }
        else
        {
            Debug.Log("퀴즈 정보 업데이트 실패 : " + task.Exception);
        }
    }


    // 데이터 가져오기.
    // 학생관리 버튼을 누르게 되면 실행되게 하자 
    // 매개변수의 학생 UID를 넣어야함.
    public void GetQuizData(string str, GameObject obj)
    {
        database = FirebaseDatabase.DefaultInstance;

        //// 사용자 ID 가져오기
        //string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        // 가져올 경로 설정
        string path = "Quiz_INFO/" + str;
        print(str);

        // 해당 경로에서 데이터 가져오기
        StartCoroutine(FetchQuizData(path, obj));
    }

    // Firebase에서 데이터 가져오기
    IEnumerator FetchQuizData(string path, GameObject obj)
    {
        yield return new WaitForSeconds(0.1f);
        var task = database.GetReference(path).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.Log("데이터 읽기 실패: " + task.Exception);
            yield break;
        }

        DataSnapshot snapshot = task.Result;

        // 기존 데이터 불러오기
        QuizInfo LoadQuizInfo = JsonUtility.FromJson<QuizInfo>(snapshot.GetRawJsonValue());

        Debug.Log(LoadQuizInfo);
        // 퀴즈를 안푼 학생이 있다면 예외처리
        if(LoadQuizInfo == null)
        {
            yield return null;
        }
        else
        {

        // 데이터 저장.
        submitQuizCnt = LoadQuizInfo.QuizAnswerCnt;
        CorrectQuizCnt = LoadQuizInfo.QuizCorrectAnswerCnt;


        Debug.Log(submitQuizCnt);
        Debug.Log(CorrectQuizCnt);
        // 이게 아니지 
        // 이 함수를 호출시킨 오브젝트의 student_QuizData를 가져와야함.
        obj.GetComponent<Student_QuizData>().StudentQuizInfo = LoadQuizInfo;

        }
    }
}
