using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class Quiz_Individual : MonoBehaviourPun
{
    // ������ ������ �������� �������� üũ�ؼ� ������.
    public static Quiz_Individual instance;

    // ���� üũ.
    public GameObject Answer_O;
    public GameObject Answer_X;

    [HideInInspector] public string Unit;
    [HideInInspector] public string Question;
    [HideInInspector] public string Answer;
    [HideInInspector] public string Commentary;


    private void Awake()
    {
        instance = this;
    }


    public void OnQuizEnded()
    {
        //photonView.RPC(nameof(OX_GroundCheck), RpcTarget.All);
        OX_GroundCheck();
    }
  
    // ���� �ð��� ������ ��
    public void OX_GroundCheck()
    {
        // ������ ������ �����´�.
        LoadQuizData(Quiz.instance.question);

        Debug.Log("OX����");
        RaycastHit hit;
        float rayLength = 1.0f; // ���� ����
        Vector3 rayOrigin = transform.position; // �÷��̾��� ���� ��ġ
        Debug.Log(DataBase.instance.user.name);
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayLength)&&!DataBase.instance.user.isTeacher)
        {

            Debug.Log("�� ������ ������ : " + Quiz.instance.answer);
            if (hit.collider.name == "O")
            {

                // �������� üũ�ؼ� ������ �����ִ� �κ�

                // if �����̸� 
                if(Quiz.instance.answer=="O")
                { 
                    MyQuizStorage.Instance.sendUserQuizData(true);
                    StartCoroutine(answer(Answer_O));

                    // ���⼭ 
                    //QuizToFireBase.instance.QuizDataSaveFun(Unit, Question, "X", Commentary, false);
                    // �������.

                    QuizToFireBase.instance.QuizDataSaveFun(Unit, Question, "O", Commentary, true);

                }
                else
                {
                    MyQuizStorage.Instance.sendUserQuizData(false);
                    StartCoroutine( answer(Answer_X));
                    QuizToFireBase.instance.QuizDataSaveFun(Unit, Question, "X", Commentary, false);

                }

                // ���� �̸�

            }
            else if(hit.collider.name == "X")
            {
                Debug.Log("X");
                // �������� üũ�ؼ� ������ �����ִ� �κ�  

                if (Quiz.instance.answer == "O")
                {
                    MyQuizStorage.Instance.sendUserQuizData(false);
                    StartCoroutine(answer(Answer_X));
                    QuizToFireBase.instance.QuizDataSaveFun(Unit, Question, "X", Commentary, false);

                }
                else
                {
                    MyQuizStorage.Instance.sendUserQuizData(true);
                    StartCoroutine(answer(Answer_O));
                    QuizToFireBase.instance.QuizDataSaveFun(Unit, Question, "O", Commentary, true);

                }
            }
            else
            {
                MyQuizStorage.Instance.sendUserQuizData(false);
                StartCoroutine(answer(Answer_X));
                Debug.Log("OX�������� ������!.");
            }
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
                    Question = saveData.question;
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


    IEnumerator answer(GameObject gameObject)
    {
        Debug.Log("����");
        GameObject answercanvas = Instantiate(gameObject);
        yield return new WaitForSeconds(3);
        Destroy(answercanvas );

    }
}
