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
        LoadQuizData();

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

    void LoadQuizData()
    {
        Unit = Quiz.instance.unit; 
        Question = Quiz.instance.question;
        Answer = Quiz.instance.answer;
        Commentary = Quiz.instance.commentary;
    }


    IEnumerator answer(GameObject gameObject)
    {
        Debug.Log("����");
        GameObject answercanvas = Instantiate(gameObject);

        // ���忡 �ִ� �ؼ��гο� ���� ����.
        Ground_commentary G_c = answercanvas.GetComponent<Ground_commentary>();

        G_c.Question_ = Question;
        G_c.Answer_ = Answer;
        G_c.Commentary_ = Commentary;
        G_c.CommentatyPanelCheck = false;

        yield return new WaitForSeconds(3);

        // �ؼ� �г��� ���״ٸ�
        if(!G_c.CommentatyPanelCheck)
            Destroy(answercanvas );

    }
}
