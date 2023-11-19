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

    // student_QuizData���� ����.
    public QuizInfo studentQuizinfo;

    // �ܿ��� ���� ����.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {

            //������ ����Ʈ ������ŭ titleinfo ����Ʈ�� ����.
            string a = studentQuizinfo.Unit_5.CorrectAnswer[0].Title;

            // �ܿ����� ���� ���� �ؼ� ����α�.

            foreach (titleinfo titleinfos in studentQuizinfo.Unit_2.CorrectAnswer)
            {
                Debug.Log("Ÿ��Ʋ : "+titleinfos.Title);
                Debug.Log("�� : " + titleinfos.Answer);
                Debug.Log("�ڸ�Ʈ : " + titleinfos.Commentary);

            }

            Unit_1.semicircleTween(0.4f);
            Unit_2.semicircleTween(0.3f);

            Unit_3.semicircleTween(0.5f);

            Unit_4.semicircleTween(0.2f);

            Debug.Log(a);

        }
    }

}
