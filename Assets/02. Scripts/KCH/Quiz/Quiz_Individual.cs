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
        LoadQuizData(Quiz.instance.question);

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

    void LoadQuizData(string question_)
    {
        // 선생이 아니라면.
        // 선생 로컬에 있는 MyQuizTitleData json 파일을 가져와 읽고
        // 문제와 단원, 답을 뽑고 다른 학생들에게 전달해준다.


        // 선생이 아니라면 리턴.


        // 원래라면 이런식으로 하는게 아니고
        // 선생이 MyQuizTitleData.json 이걸 읽고 학생들에게 rpc로 보내줘야 함.
        List<string> titles = SaveSystem.GetTitlesFromJson("MyQuizTitleData.json");

        if (titles != null)
        {
            foreach (string title in titles)
            {
                SaveData saveData = SaveSystem.Load(title);

                // 단원
                string extracted = title.Substring(0, 3);
                // 타이틀.
                string titleSlice = title.Substring(4);

                // 문제
                string test1 = saveData.question;

                // 답
                string test2 = saveData.answer;
                // 문제를 가지고 앞에 있는 단원 가져옴.
                Debug.Log(test1 + " : " + question_);

                //Debug....Log(test1 + " : " + test2);
                // 문제와 타이틀 제목이 같다면
                if (test1 == question_)
                {
                    Debug.Log("단원가져오기." + extracted);

                    // 단원 정보 저장.
                    // 학생들에게 단원과 코멘트 전달.
                    Question = saveData.question;
                    Unit = extracted;
                    Commentary = saveData.Commentary;
                    Debug.Log(Commentary + " : 코멘트");
                }

                // 단원 정보 담기
                // 단원 정보 담는것도 rpc로 보내야함.



                // 앞에 단원 삭제

                // 정답인지 오답인지 체크하고 그 오브젝트 체크.
                //photonView.RPC(nameof(SendUnit_Commentary), RpcTarget.All, Question);
            }
        }
    }


    IEnumerator answer(GameObject gameObject)
    {
        Debug.Log("실행");
        GameObject answercanvas = Instantiate(gameObject);
        yield return new WaitForSeconds(3);
        Destroy(answercanvas );

    }
}
