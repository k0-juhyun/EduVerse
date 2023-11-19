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
    // student_QuizData���� ����.
    // ���⼭ ���� �������� �ȴ� ����� 
    public QuizInfo studentQuizinfo;



    // �ܿ��� ���� ����.

    public List<titleinfo> LoadQuizData()
    {
        // 2�ܿ� Ʋ�� ������ŭ �ε�
        foreach (titleinfo titleinfos in studentQuizinfo.Unit_2.IncorrectAnswer)
        {
            Debug.Log("Ÿ��Ʋ : " + titleinfos.Title);
            Debug.Log("�� : " + titleinfos.Answer);
            Debug.Log("�ڸ�Ʈ : " + titleinfos.Commentary);
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
