using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Student_QuizData : MonoBehaviour
{
    // 순서 
    // 1 student db에서 UID 버튼 등록함수 실행
    // 2. 그 버튼 눌렀을 때 QuizToBase에 있는 GetQuizData 함수 실행. (UID, obj) 전송
    // 3. 오브젝트를 전송해 그 오브젝트에 있는 studentQuizInfo 값을 바꿔줌.
    // 4. 그 데이터를 사용해 씬 업데이트.


    [HideInInspector]
    public QuizInfo StudentQuizInfo;

    [HideInInspector]
    public GameObject MygameObject;

    [HideInInspector]
    public string UID;

    public GameObject QuizdataCanvs;
    // Start is called before the first frame update
    void Start()
    {
        MygameObject = this.gameObject;

        // 학생 UID를 줘야함.

        //일단 테스트용
        //GetComponent<Button>().onClick.AddListener(() => QuizToFireBase.instance.GetQuizData("27KHHFa2SWcs9Yo5L4A8zKOEls52", MygameObject));
        GetComponent<Button>().onClick.AddListener(StudentQuizDataUpdateBtnClick);
    }


    // Update is called once per frame
    void Update()
    {

    }

    // 이 함수를 실행시켜줘야지.
    public void StudentQuizDataUpdateBtnClick()
    {

        StartCoroutine(QuizdataUpdate());
    }
    IEnumerator QuizdataUpdate()
    {
        // 임의로 1초 기다림
        // StudentQuizInfo 로드 시간을 위해.
        StudentDB.instance.OffStudentDB();
        yield return new WaitForSeconds(0.5f);

        // 씬에 있는 오브젝트들 값을 바꾸든지
        // 아님 생성해서 하든지. 어떻게 할까.

        // 오브젝트 이름으로 가져옴.
        GameObject quizCanvas = StudentDB.instance.AnalysisDB;

        Debug.Log("quizCanvas" + quizCanvas);


        // string 값이 null이 되면 NaN으로 뜸.
        // 퀴즈를 안푼 단원들은 다 널값으로 해줘야 함..
        // 단원별 평균

        // 복제한 오브젝트 StudentQuizDB 스크립트 가져옴.
        StudentQuizDB QuizDB = quizCanvas.GetComponent<StudentQuizDB>();
        Debug.Log("quizdb" + QuizDB);

        // 처음에 리셋을 해준다.
        #region 리셋
        //QuizDB.Unit_1.Reset_value();      QuizDB.Unit_2.Reset_value();       QuizDB.Unit_3.Reset_value();
        //QuizDB.Unit_4.Reset_value();        QuizDB.Unit_5.Reset_value();

        //QuizDB.Unit_1_Average.text = "0 / 0";
        //QuizDB.Unit_2_Average.text = "0 / 0";
        //QuizDB.Unit_3_Average.text = "0 / 0";
        //QuizDB.Unit_4_Average.text = "0 / 0";
        //QuizDB.Unit_5_Average.text = "0 / 0";

        //QuizDB.CorrectCnt_AnswerCnt.text = "0 / 0";

        //QuizDB.Average.text = "0";
        #endregion

        // 단원별 반원 그래프.
        QuizDB.Unit_1.semicircleTween(
            (average(StudentQuizInfo.Unit_1) * 0.5f));
        QuizDB.Unit_2.semicircleTween(
            (average(StudentQuizInfo.Unit_2) * 0.5f));
        QuizDB.Unit_3.semicircleTween(
            (average(StudentQuizInfo.Unit_3) * 0.5f));
        QuizDB.Unit_4.semicircleTween(
            (average(StudentQuizInfo.Unit_4) * 0.5f));
        QuizDB.Unit_5.semicircleTween(
            (average(StudentQuizInfo.Unit_5) * 0.5f));

        // 단원별 평균
        QuizDB.Unit_1_Average.text =
            StudentQuizInfo.Unit_1.CorrectAnswer.Count.ToString() + "/" +
            (StudentQuizInfo.Unit_1.CorrectAnswer.Count + StudentQuizInfo.Unit_1.IncorrectAnswer.Count).ToString();
        QuizDB.Unit_2_Average.text =
            StudentQuizInfo.Unit_2.CorrectAnswer.Count.ToString() + "/" +
            (StudentQuizInfo.Unit_2.CorrectAnswer.Count + StudentQuizInfo.Unit_2.IncorrectAnswer.Count).ToString();
        QuizDB.Unit_3_Average.text =
            StudentQuizInfo.Unit_3.CorrectAnswer.Count.ToString() + "/" +
            (StudentQuizInfo.Unit_3.CorrectAnswer.Count + StudentQuizInfo.Unit_3.IncorrectAnswer.Count).ToString();
        QuizDB.Unit_4_Average.text =
            StudentQuizInfo.Unit_4.CorrectAnswer.Count.ToString() + "/" +
            (StudentQuizInfo.Unit_4.CorrectAnswer.Count + StudentQuizInfo.Unit_4.IncorrectAnswer.Count).ToString();
        QuizDB.Unit_5_Average.text =
            StudentQuizInfo.Unit_5.CorrectAnswer.Count.ToString() + "/" +
            (StudentQuizInfo.Unit_5.CorrectAnswer.Count + StudentQuizInfo.Unit_5.IncorrectAnswer.Count).ToString();


        // 총 푼 개수.
        QuizDB.CorrectCnt_AnswerCnt.text = StudentQuizInfo.QuizCorrectAnswerCnt.ToString() + "/" + StudentQuizInfo.QuizAnswerCnt.ToString();

        // 전체 평균
        // 0으로 못 나누기 때문에
        if ((float)StudentQuizInfo.QuizAnswerCnt == 0)
        {
            QuizDB.Average.text = "0";
        }
        else
        {
            // 소수점 둘째 자리.
            QuizDB.Average.text =
                ((float)StudentQuizInfo.QuizCorrectAnswerCnt / ((float)StudentQuizInfo.QuizAnswerCnt) * 100).ToString("F2");
        }

        // 단원 평가.
        // 일단 문제를 안푼 단원은 체크를 안함.
        // 푼 단원끼리만 체크하게 짜줘야 함.
        // 
        // 일단 문제를 안푼 단원 제외 -1로 체크하자 
        // StudentQuizInfo.Unit_1.CorrectAnswer.Count 이거와 StudentQuizInfo.Unit_5.IncorrectAnswer.Count 이게 0이라면 -1로 체크 아니면 평균값을 낸다.

        float[] evaluation = new float[5];

        evaluation[0] =
            (StudentQuizInfo.Unit_1.CorrectAnswer.Count == 0 && StudentQuizInfo.Unit_1.IncorrectAnswer.Count == 0) ? -1 : average(StudentQuizInfo.Unit_1);
        evaluation[1] =                                                                                            
            (StudentQuizInfo.Unit_2.CorrectAnswer.Count == 0 && StudentQuizInfo.Unit_2.IncorrectAnswer.Count == 0) ? -1 : average(StudentQuizInfo.Unit_2);
        evaluation[2] =                                                                                            
            (StudentQuizInfo.Unit_3.CorrectAnswer.Count == 0 && StudentQuizInfo.Unit_3.IncorrectAnswer.Count == 0) ? -1 : average(StudentQuizInfo.Unit_3);
        evaluation[3] =                                                                                            
            (StudentQuizInfo.Unit_4.CorrectAnswer.Count == 0 && StudentQuizInfo.Unit_4.IncorrectAnswer.Count == 0) ? -1 : average(StudentQuizInfo.Unit_4);
        evaluation[4] =                                                                                            
            (StudentQuizInfo.Unit_5.CorrectAnswer.Count == 0 && StudentQuizInfo.Unit_5.IncorrectAnswer.Count == 0) ? -1 : average(StudentQuizInfo.Unit_5);

        float evaluation_ = 999;
        string result="";
        for (int i = 0; i < evaluation.Length; i++)
        {
            if (evaluation[i] == -1) continue;
            if (evaluation[i] < evaluation_)
            {                
                evaluation_ = evaluation[i];
                result = (i+1).ToString()+"단원";
            }
        }

        // 가장 평균이 낮은 단원 대입
        QuizDB.lowestUnit.text = result;
        Debug.Log(result);

        // 그전에 오답 QuizDB 이 컴포넌트에 quizinfo에 관한 데이터 넘겨줌.
        QuizDB.studentQuizinfo = StudentQuizInfo;

        yield return new WaitForSeconds(1f);
        // 계산하고 적용하고 이 오브젝트가 있는 패널 끄기.
        //StudentDB.instance.ShowPersonalDB();

    }

    // UID 받아 버튼 등록.
    public void StudentUIDSaveBtnClick(string uid)
    {
        MygameObject = this.gameObject;
        Debug.Log(uid);
        // 버튼을 눌렀을떄 사용자 UID와 오브젝트를 전달한다.
        // 내가보기엔 등록이 안됌.
        GetComponent<Button>().onClick.AddListener(() =>
        {
            StudentDB.instance.OffStudentDB();

            QuizToFireBase.instance.GetQuizData(uid, MygameObject);
        });
    }

    // 반원 평균내기.
    float average(answerinfo answerinfo)
    {
        float correctCount = (float)answerinfo.CorrectAnswer.Count;
        float totalCount = correctCount + (float)answerinfo.IncorrectAnswer.Count;

        float result = totalCount != 0 ? correctCount / totalCount : 0f;
        return result;
    }
}
