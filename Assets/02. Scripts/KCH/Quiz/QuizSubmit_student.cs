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
    // 문제
    string Question;
    // 답
    string Answer;
    // 단원
    string Unit;
    // 해설
    string Commentary;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            if (DataBase.instance.user.isTeacher == false)
            {
                Debug.Log("실행");
                GameObject quizPanel = Instantiate(QuizPrefab);
            }
        }

    }

    // 버튼을 눌렀을 때 correctCheck 변수를 사용해서 정답 오답 체크한다.

    void mydatabase()
    { 
        string name = DataBase.instance.user.name;
    }
    // 정답 제출
    public void CorrectBtnClick()
    {
        // 이 아이디의 UID를 확인하여 DB에 저장해야함.

        // 선생한테 제출할 데이터 
        // 나의 이름과 문제, 정답
        if(correctCheck)
        {
            // 정답
            Debug.Log("정답입니당");

            MyQuizStorage.Instance.sendUserQuizData(true);
            correct.SetActive(true);

            StartCoroutine(quizPaneldelete(true));

            // 데이터 줘야할것들 
            // 단원. 문제 이름, 답, 해설 , 문제에 대해 맞았는지 틀렸는지.
            QuizToFireBase.instance.QuizDataSaveFun(Unit, Question, "O", Commentary, true);
        }
        else
        {
            Debug.Log("오답입니당");
            MyQuizStorage.Instance.sendUserQuizData(false);
            
            // 오답 패널 띄우고 그 안에 답안이랑 commentary 넣어주기.
            incorrect.SetActive(true);

            StartCoroutine(quizPaneldelete(false));

            QuizToFireBase.instance.QuizDataSaveFun(Unit, Question, "X", Commentary, false);

        }
    }

    // 오답 제출.
    public void IncorrectBtnClick()
    {
        if (correctCheck)
        {
            MyQuizStorage.Instance.sendUserQuizData(false);
            incorrect.SetActive(true);

            Debug.Log("오답입니당");
            StartCoroutine(quizPaneldelete(false));

            QuizToFireBase.instance.QuizDataSaveFun(Unit, Question, "X", Commentary, false);

        }
        else
        {
            MyQuizStorage.Instance.sendUserQuizData(true);
            // 정답
            correct.SetActive(true);
            Debug.Log("정답입니당");
            StartCoroutine(quizPaneldelete(true));

            QuizToFireBase.instance.QuizDataSaveFun(Unit, Question, "O", Commentary, true);
        }
    }


    public void quizdata(string question_, string answer_, string unit_, string commentary_)
    {
        // 선생이 퀴즈 패널을 띄워주고

        questionText.text = question_;
        Question = question_;
        Answer= answer_;
        Unit = unit_;
        Commentary = commentary_;
        // 정답인지 오답인지 체크
        if (answer_ == "O") correctCheck = true;
        else correctCheck = false;

        // 학생은 문제를 풀었는지 오답인지 정답인지의 대한 데이터를 준다.

        // 여기선 아직 못풀었다는 정보를 줘야함.
        // 이름과 같이 줘야함.
        string sendName = DataBase.instance.user.name;

        // 못 푼 인원에 추가.
        MyQuizStorage.Instance.sendUserQuizData();

        // Don't destoryobject되어있는 MyQuizStorage에 넣자.
        // 여기는 개별적으로 생성되어있고 포톤 뷰 선생한테는 이 오브젝트가 없기 때문에
        // 학생 json으로 DB 저장 일단 나중에 하는걸로 하고 일단 
        // 선생 패널에 푼 인원과 못푼 인원 나누는 걸로 하자.

        // 못푼 인원에 추가
        // 풀면 못푼 인원에 있는 데이터 지우고
        // 

        // 문제 단원, 정답
        //LoadQuizData(Question);
        // 학생들 제외 선생만 함수를 보낸다.
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
            // 코멘트 패널이 띄워지면 안사라지게.
            yield return new WaitForSeconds(3);
            if(desoryPanelCheck)

            Panel.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InBack).OnComplete(() => Destroy(gameObject));

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
        // 자기 pc에는  json 파일이 없기 때문에 선생이 학생들에게 데이터를 보내줘야 하는 상황임.
        // 그럼 학생은 선생에게 콜백을 하는 형식으로 하자.

        // 선생은 낸 문제를  myQuizStorage에 담는다. (문제 단원 해설 코멘트 포함.)

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

    // 해설 코멘트 패널 띄워줌.
    public void OnCommentaryPanelBtnClick()
    {
        // 3초 뒤에 사라지는거 막아야함.

        // 해설 패널 띄우고 그 안에 값 넣어줌.
        GameObject panel = Instantiate(CommentaryPrefab, transform);
        panel.GetComponent<ClassroomCommentary>().PutAnswer_Commentary(Answer,Commentary);
        panel.transform.parent = Panel.transform;
        desoryPanelCheck = false;
    }


}
