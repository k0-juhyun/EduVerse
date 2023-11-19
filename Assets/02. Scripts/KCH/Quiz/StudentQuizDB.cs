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

    public GameObject IncorrectNotePanel;
    // student_QuizData에서 참조.
    // 여기서 문제 가져오면 된다 찬희야 
    public QuizInfo studentQuizinfo;



    // 단원별 오답 정리.

    public List<titleinfo> LoadQuizData()
    {
        // 2단원 틀린 개수만큼 로드
        foreach (titleinfo titleinfos in studentQuizinfo.Unit_2.IncorrectAnswer)
        {
            Debug.Log("타이틀 : " + titleinfos.Title);
            Debug.Log("답 : " + titleinfos.Answer);
            Debug.Log("코멘트 : " + titleinfos.Commentary);
        }
    }

    public void OnIncorrectNoteBtnClick()
    {
        IncorrectNotePanel.SetActive(true);
    }
    public void OffIncorrectNoteBtnClick()
    {
        IncorrectNotePanel.SetActive(false);
    }
}
