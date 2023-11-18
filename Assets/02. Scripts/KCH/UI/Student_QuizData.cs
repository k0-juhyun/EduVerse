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
        if (Input.GetKeyDown(KeyCode.X))
        {
            StudentUIDSaveBtnClick("27KHHFa2SWcs9Yo5L4A8zKOEls52");
        }
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
        yield return new WaitForSeconds(1);

        GameObject quizCanvas = Instantiate(QuizdataCanvs);


        // string 값이 null이 되면 NaN으로 뜸.
        // 퀴즈를 안푼 단원들은 다 널값으로 해줘야 함..
        // 단원별 평균

        // 복제한 오브젝트 StudentQuizDB 스크립트 가져옴.
        StudentQuizDB QuizDB = quizCanvas.GetComponent<StudentQuizDB>();
        Debug.Log(QuizDB);
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

        QuizDB.Unit_1_Average.text = average(StudentQuizInfo.Unit_1).ToString();
        QuizDB.Unit_2_Average.text = average(StudentQuizInfo.Unit_2).ToString();
        QuizDB.Unit_3_Average.text = average(StudentQuizInfo.Unit_3).ToString();
        QuizDB.Unit_4_Average.text = average(StudentQuizInfo.Unit_4).ToString();
        QuizDB.Unit_5_Average.text = average(StudentQuizInfo.Unit_5).ToString();


        Debug.Log((float)StudentQuizInfo.Unit_1.CorrectAnswer.Count/0);

        // 총 푼 개수.
        QuizDB.CorrectCnt_AnswerCnt.text = StudentQuizInfo.QuizCorrectAnswerCnt.ToString() + "/" + StudentQuizInfo.QuizAnswerCnt.ToString();
        QuizDB.Average.text = 
            ((float)StudentQuizInfo.QuizCorrectAnswerCnt / ((float)StudentQuizInfo.QuizAnswerCnt)).ToString();

        // 전에 있던 패널 끄기
        StudentDB.instance.ShowPersonalDB();

    }

    // UID 받아 버튼 등록.
    public void StudentUIDSaveBtnClick(string uid)
    {
        MygameObject = this.gameObject;
        Debug.Log(uid);
        // 버튼을 눌렀을떄 사용자 UID와 오브젝트를 전달한다.
        // 내가보기엔 등록이 안됌.
        GetComponent<Button>().onClick.AddListener(() => {
            Debug.Log("Listener added for GetQuizData");
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
