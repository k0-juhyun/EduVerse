using Firebase.Auth;
using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizSubmit_student : MonoBehaviour
{

    public TextMeshProUGUI questionText;

    public GameObject QuizPrefab;
    public GameObject correct;
    public GameObject incorrect;

    bool correctCheck;

    // ����
    string Question;
    // �ܿ�
    string Unit;
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

            StartCoroutine(quizPaneldelete());

            // ������ ����Ұ͵� 
            // �ܿ�. ���� �̸�, ��, ������ ���� �¾Ҵ��� Ʋ�ȴ���.
            QuizToFireBase.instance.TestAddData(Unit,Question, "O", true);
        }
        else
        {
            Debug.Log("�����Դϴ�");
            MyQuizStorage.Instance.sendUserQuizData(false);
            incorrect.SetActive(true);

            StartCoroutine(quizPaneldelete());

            QuizToFireBase.instance.TestAddData(Unit, Question, "X", true);

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
            StartCoroutine(quizPaneldelete());

            QuizToFireBase.instance.TestAddData(Unit, Question, "X", false);

        }
        else
        {
            MyQuizStorage.Instance.sendUserQuizData(true);
            // ����
            correct.SetActive(true);
            Debug.Log("�����Դϴ�");
            StartCoroutine(quizPaneldelete());

            QuizToFireBase.instance.TestAddData(Unit, Question, "O", false);
        }
    }


    public void quizdata(string question_,string answer_)
    {
        // ������ ���� �г��� ����ְ�

        questionText.text = question_;
        Question = question_;
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
        test(Question);

    }
    IEnumerator quizPaneldelete()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }


    void test(string question_)
    {
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
                Debug.Log(extracted + " : " + titleSlice);

                //Debug....Log(test1 + " : " + test2);
                if (test1 == question_)
                {
                    Debug.Log("�ܿ���������." + extracted);

                    // �ܿ� ���� ����.
                    Unit = extracted;
                }

                // �տ� �ܿ� ����

                // �������� �������� üũ�ϰ� �� ������Ʈ üũ.
            }
        }
    }
}
