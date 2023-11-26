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

// ���� �׽�Ʈ������ StudentDB�� �־�� ���߿� ���߉�.

public class QuizToFireBase : MonoBehaviour
{
    DatabaseReference reference;
    FirebaseDatabase database;

    static public QuizToFireBase instance;

    // �ҷ��� ������ ����
    [HideInInspector]
    public QuizInfo LoadQuizInfo;

    // ���� ������ ����.
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

    // �ܿ� ���� answer �������� ��������.
    public void QuizDataSaveFun(string unit_, string question_, string answer_, string commentary_, bool result_)
    {

        database = FirebaseDatabase.DefaultInstance;

        // ������� ID ��������
        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        // ���� ������ ��������
        string path = "Quiz_INFO/" + userId;
        StartCoroutine(ReadExistingData(path, unit_, question_, answer_, commentary_, result_));
    }

    // ���� �ִ� �����Ϳ� �� �߰�.
    IEnumerator ReadExistingData(string path, string unit_, string question_, string answer_, string commentary_, bool result_)
    {
        var task = database.GetReference(path).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.Log("������ �б� ����: " + task.Exception);
            yield break;
        }

        DataSnapshot snapshot = task.Result;

        // ���� ������ �ҷ�����
        QuizInfo existingQuizInfo = JsonUtility.FromJson<QuizInfo>(snapshot.GetRawJsonValue());

        // ���� ���� �����Ͱ� ������, ��θ� ������ �� �����͸� �߰��մϴ�.
        if (existingQuizInfo == null)
        {
            existingQuizInfo = new QuizInfo(0, 0, new answerinfo(), new answerinfo(), new answerinfo(), new answerinfo(), new answerinfo());
            var createTask = database.GetReference(path).SetRawJsonValueAsync(JsonUtility.ToJson(existingQuizInfo));

            yield return new WaitUntil(() => createTask.IsCompleted);

            if (createTask.Exception != null)
            {
                Debug.Log("������ ���� ����: " + createTask.Exception);
                yield break;
            }
        }

        Debug.Log("���� "+question_);
        Debug.Log("unit_ " + unit_);

        Debug.Log("answer_ " + answer_);
        Debug.Log("commentary_ " + commentary_);

        // ���Ŀ� ���ο� �����͸� �߰��մϴ�.
        titleinfo newQuestion = new titleinfo(question_, answer_, commentary_);


        // ������
        if (result_)
        {
            // �ܿ� ���� ���� �߰�.
            switch (unit_)
            {
                case "1�ܿ�":
                    existingQuizInfo.Unit_1.CorrectAnswer.Add(newQuestion);
                    break;
                case "2�ܿ�":
                    existingQuizInfo.Unit_2.CorrectAnswer.Add(newQuestion);
                    break;
                case "3�ܿ�":
                    existingQuizInfo.Unit_3.CorrectAnswer.Add(newQuestion);
                    break;
                case "4�ܿ�":
                    existingQuizInfo.Unit_4.CorrectAnswer.Add(newQuestion);
                    break;
                case "5�ܿ�":
                    existingQuizInfo.Unit_5.CorrectAnswer.Add(newQuestion);
                    break;
            }
            existingQuizInfo.QuizAnswerCnt++;
            existingQuizInfo.QuizCorrectAnswerCnt++;
        }
        //Ʋ����
        else
        {
            // �ܿ� ���� ���� �߰�.
            switch (unit_)
            {
                case "1�ܿ�":
                    existingQuizInfo.Unit_1.IncorrectAnswer.Add(newQuestion);
                    break;
                case "2�ܿ�":
                    existingQuizInfo.Unit_2.IncorrectAnswer.Add(newQuestion);
                    break;
                case "3�ܿ�":
                    existingQuizInfo.Unit_3.IncorrectAnswer.Add(newQuestion);
                    break;
                case "4�ܿ�":
                    existingQuizInfo.Unit_4.IncorrectAnswer.Add(newQuestion);
                    break;
                case "5�ܿ�":
                    existingQuizInfo.Unit_5.IncorrectAnswer.Add(newQuestion);
                    break;
            }
            existingQuizInfo.QuizAnswerCnt++;
        }

        // Firebase�� ������Ʈ
        StartCoroutine(UpdateDataToFirebase(existingQuizInfo));
    }

    IEnumerator UpdateDataToFirebase(QuizInfo quizInfo)
    {
        string path = "Quiz_INFO/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        var task = database.GetReference(path).SetRawJsonValueAsync(JsonUtility.ToJson(quizInfo));
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception == null)
        {
            Debug.Log("���� ���� ������Ʈ ����");
        }
        else
        {
            Debug.Log("���� ���� ������Ʈ ���� : " + task.Exception);
        }
    }


    // ������ ��������.
    // �л����� ��ư�� ������ �Ǹ� ����ǰ� ���� 
    // �Ű������� �л� UID�� �־����.
    public void GetQuizData(string str, GameObject obj)
    {
        database = FirebaseDatabase.DefaultInstance;

        //// ����� ID ��������
        //string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        // ������ ��� ����
        string path = "Quiz_INFO/" + str;
        print(str);

        // �ش� ��ο��� ������ ��������
        StartCoroutine(FetchQuizData(path, obj));
    }

    // Firebase���� ������ ��������
    IEnumerator FetchQuizData(string path, GameObject obj)
    {
        yield return new WaitForSeconds(0.1f);
        var task = database.GetReference(path).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.Log("������ �б� ����: " + task.Exception);
            yield break;
        }

        DataSnapshot snapshot = task.Result;

        // ���� ������ �ҷ�����
        QuizInfo LoadQuizInfo = JsonUtility.FromJson<QuizInfo>(snapshot.GetRawJsonValue());

        Debug.Log(LoadQuizInfo);
        // ��� ��Ǭ �л��� �ִٸ� ����ó��
        if(LoadQuizInfo == null)
        {
            yield return null;
        }
        else
        {

        // ������ ����.
        submitQuizCnt = LoadQuizInfo.QuizAnswerCnt;
        CorrectQuizCnt = LoadQuizInfo.QuizCorrectAnswerCnt;


        Debug.Log(submitQuizCnt);
        Debug.Log(CorrectQuizCnt);
        // �̰� �ƴ��� 
        // �� �Լ��� ȣ���Ų ������Ʈ�� student_QuizData�� �����;���.
        obj.GetComponent<Student_QuizData>().StudentQuizInfo = LoadQuizInfo;

        }
    }
}
