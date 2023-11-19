using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class StudentQuizDB : MonoBehaviour
{
    public semicircleUI Unit_1;
    public semicircleUI Unit_2;
    public semicircleUI Unit_3;
    public semicircleUI Unit_4;
    public semicircleUI Unit_5;

    public TextMeshProUGUI CorrectCnt_AnswerCnt;
    public TextMeshProUGUI Average;

    public TextMeshProUGUI Unit_1_Average;
    public TextMeshProUGUI Unit_2_Average;
    public TextMeshProUGUI Unit_3_Average;
    public TextMeshProUGUI Unit_4_Average;
    public TextMeshProUGUI Unit_5_Average;

    // student_QuizData에서 참조.
    public QuizInfo studentQuizinfo;

    // 단원별 오답 정리.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {

            //오답의 리스트 개수만큼 titleinfo 리스트에 적용.
            string a = studentQuizinfo.Unit_5.CorrectAnswer[0].Title;

            // 단원별로 문제 정답 해설 적어두기.

            foreach (titleinfo titleinfos in studentQuizinfo.Unit_2.CorrectAnswer)
            {
                Debug.Log("타이틀 : "+titleinfos.Title);
                Debug.Log("답 : " + titleinfos.Answer);
                Debug.Log("코멘트 : " + titleinfos.Commentary);

            }

            Unit_1.semicircleTween(0.4f);
            Unit_2.semicircleTween(0.3f);

            Unit_3.semicircleTween(0.5f);

            Unit_4.semicircleTween(0.2f);

            Debug.Log(a);

        }
    }

}
