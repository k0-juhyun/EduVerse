using Firebase.Auth;
using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public class QuizSubmit_student : MonoBehaviourPun
{

    public TextMeshProUGUI questionText;

    public GameObject QuizPrefab;
    public GameObject correct;
    public GameObject incorrect;
    public GameObject CommentaryPrefab;

    public GameObject Panel;

    bool correctCheck;
    bool desoryPanelCheck =true;
    // ����
    string Question;
    // ��
    string Answer;
    // �ܿ�
    string Unit;
    // �ؼ�
    string Commentary;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            if (DataBase.instance.user.isTeacher == false)
            {
                Debug.Log("����");
                GameObject quizPanel = Instantiate(QuizPrefab);
            }
        }

    }

    // ��ư�� ������ �� correctCheck ������ ����ؼ� ���� ���� üũ�Ѵ�.

    void mydatabase()
    { 
        string name = DataBase.instance.user.name;
    }
    // ���� ����
    public void CorrectBtnClick()
    {
        // �� ���̵��� UID�� Ȯ���Ͽ� DB�� �����ؾ���.

        // �������� ������ ������ 
        // ���� �̸��� ����, ����
        if(correctCheck)
        {
            // ����
            Debug.Log("�����Դϴ�");

            MyQuizStorage.Instance.sendUserQuizData(true);
            correct.SetActive(true);

            StartCoroutine(quizPaneldelete(true));

            // ������ ����Ұ͵� 
            // �ܿ�. ���� �̸�, ��, �ؼ� , ������ ���� �¾Ҵ��� Ʋ�ȴ���.
            QuizToFireBase.instance.QuizDataSaveFun(Unit, Question, "O", Commentary, true);
        }
        else
        {
            Debug.Log("�����Դϴ�");
            MyQuizStorage.Instance.sendUserQuizData(false);
            
            // ���� �г� ���� �� �ȿ� ����̶� commentary �־��ֱ�.
            incorrect.SetActive(true);

            StartCoroutine(quizPaneldelete(false));

            QuizToFireBase.instance.QuizDataSaveFun(Unit, Question, "X", Commentary, false);

        }
    }

    // ���� ����.
    public void IncorrectBtnClick()
    {
        if (correctCheck)
        {
            MyQuizStorage.Instance.sendUserQuizData(false);
            incorrect.SetActive(true);

            Debug.Log("�����Դϴ�");
            StartCoroutine(quizPaneldelete(false));

            QuizToFireBase.instance.QuizDataSaveFun(Unit, Question, "X", Commentary, false);

        }
        else
        {
            MyQuizStorage.Instance.sendUserQuizData(true);
            // ����
            correct.SetActive(true);
            Debug.Log("�����Դϴ�");
            StartCoroutine(quizPaneldelete(true));

            QuizToFireBase.instance.QuizDataSaveFun(Unit, Question, "O", Commentary, true);
        }
    }


    public void quizdata(string question_, string answer_, string unit_, string commentary_)
    {
        // ������ ���� �г��� ����ְ�

        questionText.text = question_;
        Question = question_;
        Answer= answer_;
        Unit = unit_;
        Commentary = commentary_;
        // �������� �������� üũ
        if (answer_ == "O") correctCheck = true;
        else correctCheck = false;

        // �л��� ������ Ǯ������ �������� ���������� ���� �����͸� �ش�.

        // ���⼱ ���� ��Ǯ���ٴ� ������ �����.
        // �̸��� ���� �����.
        string sendName = DataBase.instance.user.name;

        // �� Ǭ �ο��� �߰�.
        MyQuizStorage.Instance.sendUserQuizData();

        // Don't destoryobject�Ǿ��ִ� MyQuizStorage�� ����.
        // ����� ���������� �����Ǿ��ְ� ���� �� �������״� �� ������Ʈ�� ���� ������
        // �л� json���� DB ���� �ϴ� ���߿� �ϴ°ɷ� �ϰ� �ϴ� 
        // ���� �гο� Ǭ �ο��� ��Ǭ �ο� ������ �ɷ� ����.

        // ��Ǭ �ο��� �߰�
        // Ǯ�� ��Ǭ �ο��� �ִ� ������ �����
        // 

        // ���� �ܿ�, ����
        //LoadQuizData(Question);
        // �л��� ���� ������ �Լ��� ������.
        //photonView.RPC(nameof(LoadQuizData),RpcTarget.All, Question);


        Debug.Log(question_+ " : " + answer_+ " : " + unit_+ " : " + commentary_);
    }
    IEnumerator quizPaneldelete(bool result)
    {
        if (result)
        {
            yield return new WaitForSeconds(3);
            Panel.transform.DOScale(new Vector3(0.1f,0.1f,0.1f),0.5f).SetEase(Ease.InBack).OnComplete(() => Destroy(gameObject));

        }
        else
        {
            // �ڸ�Ʈ �г��� ������� �Ȼ������.
            yield return new WaitForSeconds(3);
            if(desoryPanelCheck)

            Panel.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InBack).OnComplete(() => Destroy(gameObject));

        }
    }

    void LoadQuizData(string question_)
    {
        // ������ �ƴ϶��.
        // ���� ���ÿ� �ִ� MyQuizTitleData json ������ ������ �а�
        // ������ �ܿ�, ���� �̰� �ٸ� �л��鿡�� �������ش�.


        // ������ �ƴ϶�� ����.


        // ������� �̷������� �ϴ°� �ƴϰ�
        // ������ MyQuizTitleData.json �̰� �а� �л��鿡�� rpc�� ������� ��.
        // �ڱ� pc����  json ������ ���� ������ ������ �л��鿡�� �����͸� ������� �ϴ� ��Ȳ��.
        // �׷� �л��� �������� �ݹ��� �ϴ� �������� ����.

        // ������ �� ������  myQuizStorage�� ��´�. (���� �ܿ� �ؼ� �ڸ�Ʈ ����.)

        List<string> titles = SaveSystem.GetTitlesFromJson("MyQuizTitleData.json");

        if (titles != null)
        {
            foreach (string title in titles)
            {
                SaveData saveData = SaveSystem.Load(title);

                // �ܿ�
                string extracted = title.Substring(0, 3);
                // Ÿ��Ʋ.
                string titleSlice = title.Substring(4);

                // ����
                string test1 = saveData.question;

                // ��
                string test2 = saveData.answer;
                // ������ ������ �տ� �ִ� �ܿ� ������.
                Debug.Log(test1 + " : " + question_);

                //Debug....Log(test1 + " : " + test2);
                // ������ Ÿ��Ʋ ������ ���ٸ�
                if (test1 == question_)
                {
                    Debug.Log("�ܿ���������." + extracted);

                    // �ܿ� ���� ����.
                    // �л��鿡�� �ܿ��� �ڸ�Ʈ ����.

                    Unit = extracted;
                    Commentary = saveData.Commentary;
                    Debug.Log(Commentary + " : �ڸ�Ʈ");
                }

                // �ܿ� ���� ���
                // �ܿ� ���� ��°͵� rpc�� ��������.



                // �տ� �ܿ� ����

                // �������� �������� üũ�ϰ� �� ������Ʈ üũ.
                //photonView.RPC(nameof(SendUnit_Commentary), RpcTarget.All, Question);
            }
        }
    }

    // �ؼ� �ڸ�Ʈ �г� �����.
    public void OnCommentaryPanelBtnClick()
    {
        // 3�� �ڿ� ������°� ���ƾ���.

        // �ؼ� �г� ���� �� �ȿ� �� �־���.
        GameObject panel = Instantiate(CommentaryPrefab, transform);
        panel.GetComponent<ClassroomCommentary>().PutAnswer_Commentary(Answer,Commentary);
        panel.transform.parent = Panel.transform;
        desoryPanelCheck = false;
    }


}
