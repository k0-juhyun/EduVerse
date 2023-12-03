using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class Quiz_Individual : MonoBehaviourPun
{
    // 서버에 문제와 정답인지 오답인지 체크해서 보내줌.
    public static Quiz_Individual instance;

    // 정답 체크.
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
  
    // 퀴즈 시간이 끝났을 때
    public void OX_GroundCheck()
    {
        // 문제의 정보를 가져온다.
        LoadQuizData();

        Debug.Log("OX실행");
        RaycastHit hit;
        float rayLength = 1.0f; // 레이 길이
        Vector3 rayOrigin = transform.position; // 플레이어의 현재 위치
        Debug.Log(DataBase.instance.user.name);
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayLength)&&!DataBase.instance.user.isTeacher)
        {

            Debug.Log("이 문제의 정답은 : " + Quiz.instance.answer);
            if (hit.collider.name == "O")
            {

                // 정답인지 체크해서 서버에 보내주는 부분

                // if 정답이면 
                if(Quiz.instance.answer=="O")
                { 
                    MyQuizStorage.Instance.sendUserQuizData(true);
                    StartCoroutine(answer(Answer_O));

                    // 여기서 
                    //QuizToFireBase.instance.QuizDataSaveFun(Unit, Question, "X", Commentary, false);
                    // 해줘야함.

                    QuizToFireBase.instance.QuizDataSaveFun(Unit, Question, "O", Commentary, true);

                }
                else
                {
                    MyQuizStorage.Instance.sendUserQuizData(false);
                    StartCoroutine( answer(Answer_X));
                    QuizToFireBase.instance.QuizDataSaveFun(Unit, Question, "X", Commentary, false);

                }

                // 오답 이면

            }
            else if(hit.collider.name == "X")
            {
                Debug.Log("X");
                // 오답인지 체크해서 서버에 보내주는 부분  

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
                Debug.Log("OX발판으로 들어가세요!.");
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
        Debug.Log("실행");
        GameObject answercanvas = Instantiate(gameObject);

        // 광장에 있는 해설패널에 정보 대입.
        Ground_commentary G_c = answercanvas.GetComponent<Ground_commentary>();

        G_c.Question_ = Question;
        G_c.Answer_ = Answer;
        G_c.Commentary_ = Commentary;
        G_c.CommentatyPanelCheck = false;

        yield return new WaitForSeconds(3);

        // 해설 패널을 안켰다면
        if(!G_c.CommentatyPanelCheck)
            Destroy(answercanvas );

    }
}
