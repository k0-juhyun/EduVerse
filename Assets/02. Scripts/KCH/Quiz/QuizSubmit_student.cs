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

    // 문제
    string Question;
    // 단원
    string Unit;
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

            StartCoroutine(quizPaneldelete());

            // 데이터 줘야할것들 
            // 단원. 문제 이름, 답, 문제에 대해 맞았는지 틀렸는지.
            QuizToFireBase.instance.TestAddData(Unit,Question, "O", true);
        }
        else
        {
            Debug.Log("오답입니당");
            MyQuizStorage.Instance.sendUserQuizData(false);
            incorrect.SetActive(true);

            StartCoroutine(quizPaneldelete());

            QuizToFireBase.instance.TestAddData(Unit, Question, "X", true);

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
            StartCoroutine(quizPaneldelete());

            QuizToFireBase.instance.TestAddData(Unit, Question, "X", false);

        }
        else
        {
            MyQuizStorage.Instance.sendUserQuizData(true);
            // 정답
            correct.SetActive(true);
            Debug.Log("정답입니당");
            StartCoroutine(quizPaneldelete());

            QuizToFireBase.instance.TestAddData(Unit, Question, "O", false);
        }
    }


    public void quizdata(string question_,string answer_)
    {
        // 선생이 퀴즈 패널을 띄워주고

        questionText.text = question_;
        Question = question_;
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

                // 단원
                string extracted = title.Substring(0, 3);
                // 타이틀.
                string titleSlice = title.Substring(4);

                // 문제
                string test1 = saveData.question;

                // 답
                string test2 = saveData.answer;
                // 문제를 가지고 앞에 있는 단원 가져옴.
                Debug.Log(extracted + " : " + titleSlice);

                //Debug....Log(test1 + " : " + test2);
                if (test1 == question_)
                {
                    Debug.Log("단원가져오기." + extracted);

                    // 단원 정보 저장.
                    Unit = extracted;
                }

                // 앞에 단원 삭제

                // 정답인지 오답인지 체크하고 그 오브젝트 체크.
            }
        }
    }
}
