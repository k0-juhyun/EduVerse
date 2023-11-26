using DG.Tweening;
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
    public TextMeshProUGUI lowestUnit;

    public TextMeshProUGUI Unit_1_Average;
    public TextMeshProUGUI Unit_2_Average;
    public TextMeshProUGUI Unit_3_Average;
    public TextMeshProUGUI Unit_4_Average;
    public TextMeshProUGUI Unit_5_Average;

    public GameObject IncorrectNotePanel;
    // student_QuizData���� ����.
    // ���⼭ ���� �������� �ȴ� ����� 
    public QuizInfo studentQuizinfo;

    public GameObject IncorrectAnswerPrefab;
    public GameObject IncorrectAnswerPanelPrefab;

    public GameObject viewport;

    // �ܿ��� ���� ����.
    private void Start()
    {
        // ó���� ���°� �޾���.

    }


    public void LoadQuizData()
    {
        // �ܿ� Ʋ�� ������ŭ �ε�
        foreach (titleinfo titleinfos in studentQuizinfo.Unit_1.IncorrectAnswer)
        {
            // viewport �ڽĿ� �߰�.
            GameObject IncorrectPrefab = Instantiate(IncorrectAnswerPrefab,viewport.transform);
            IncorrectPrefab.GetComponent<IncorrectNote>().PutData("1�ܿ�",titleinfos.Title, titleinfos.Answer,titleinfos.Commentary);
        }
        foreach (titleinfo titleinfos in studentQuizinfo.Unit_2.IncorrectAnswer)
        {
            // viewport �ڽĿ� �߰�.
            GameObject IncorrectPrefab = Instantiate(IncorrectAnswerPrefab, viewport.transform);
            IncorrectPrefab.GetComponent<IncorrectNote>().PutData("2�ܿ�", titleinfos.Title, titleinfos.Answer, titleinfos.Commentary);
        }
        foreach (titleinfo titleinfos in studentQuizinfo.Unit_3.IncorrectAnswer)
        {
            // viewport �ڽĿ� �߰�.
            GameObject IncorrectPrefab = Instantiate(IncorrectAnswerPrefab, viewport.transform);
            IncorrectPrefab.GetComponent<IncorrectNote>().PutData("3�ܿ�", titleinfos.Title, titleinfos.Answer, titleinfos.Commentary);
        }
        foreach (titleinfo titleinfos in studentQuizinfo.Unit_4.IncorrectAnswer)
        {
            // viewport �ڽĿ� �߰�.
            GameObject IncorrectPrefab = Instantiate(IncorrectAnswerPrefab, viewport.transform);
            IncorrectPrefab.GetComponent<IncorrectNote>().PutData("4�ܿ�", titleinfos.Title, titleinfos.Answer, titleinfos.Commentary);
        }
        foreach (titleinfo titleinfos in studentQuizinfo.Unit_5.IncorrectAnswer)
        {
            // viewport �ڽĿ� �߰�.
            GameObject IncorrectPrefab = Instantiate(IncorrectAnswerPrefab, viewport.transform);
            IncorrectPrefab.GetComponent<IncorrectNote>().PutData("5�ܿ�", titleinfos.Title, titleinfos.Answer, titleinfos.Commentary);
        }
    }

    public void OnIncorrectNoteBtnClick()
    {
        IncorrectNotePanel.SetActive(true);
        // ���� �߰�.
        // �� �ѹ��� �߰�.


        // �� �ʱ�ȭ�ؾ߉�
        int childCount = viewport.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(viewport.transform.GetChild(i).gameObject);
        }

        //if(viewport.transform.childCount == 0)
        LoadQuizData();
    }

    public void OffIncorrectNoteBtnClick()
    {
        IncorrectNotePanel.transform.DOScale(0.1f, 0.5f).SetEase(Ease.InBack)
            .OnComplete(() => IncorrectNotePanel.SetActive(false));
    }
}
