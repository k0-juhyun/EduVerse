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
    // student_QuizData에서 참조.
    // 여기서 문제 가져오면 된다 찬희야 
    public QuizInfo studentQuizinfo;

    public GameObject IncorrectAnswerPrefab;
    public GameObject IncorrectAnswerPanelPrefab;

    public GameObject viewport;

    // 단원별 오답 정리.
    private void Start()
    {
        // 처음에 리셋값 받아줌.

    }


    public void LoadQuizData()
    {
        // 단원 틀린 개수만큼 로드
        foreach (titleinfo titleinfos in studentQuizinfo.Unit_1.IncorrectAnswer)
        {
            // viewport 자식에 추가.
            GameObject IncorrectPrefab = Instantiate(IncorrectAnswerPrefab,viewport.transform);
            IncorrectPrefab.GetComponent<IncorrectNote>().PutData("1단원",titleinfos.Title, titleinfos.Answer,titleinfos.Commentary);
        }
        foreach (titleinfo titleinfos in studentQuizinfo.Unit_2.IncorrectAnswer)
        {
            // viewport 자식에 추가.
            GameObject IncorrectPrefab = Instantiate(IncorrectAnswerPrefab, viewport.transform);
            IncorrectPrefab.GetComponent<IncorrectNote>().PutData("2단원", titleinfos.Title, titleinfos.Answer, titleinfos.Commentary);
        }
        foreach (titleinfo titleinfos in studentQuizinfo.Unit_3.IncorrectAnswer)
        {
            // viewport 자식에 추가.
            GameObject IncorrectPrefab = Instantiate(IncorrectAnswerPrefab, viewport.transform);
            IncorrectPrefab.GetComponent<IncorrectNote>().PutData("3단원", titleinfos.Title, titleinfos.Answer, titleinfos.Commentary);
        }
        foreach (titleinfo titleinfos in studentQuizinfo.Unit_4.IncorrectAnswer)
        {
            // viewport 자식에 추가.
            GameObject IncorrectPrefab = Instantiate(IncorrectAnswerPrefab, viewport.transform);
            IncorrectPrefab.GetComponent<IncorrectNote>().PutData("4단원", titleinfos.Title, titleinfos.Answer, titleinfos.Commentary);
        }
        foreach (titleinfo titleinfos in studentQuizinfo.Unit_5.IncorrectAnswer)
        {
            // viewport 자식에 추가.
            GameObject IncorrectPrefab = Instantiate(IncorrectAnswerPrefab, viewport.transform);
            IncorrectPrefab.GetComponent<IncorrectNote>().PutData("5단원", titleinfos.Title, titleinfos.Answer, titleinfos.Commentary);
        }
    }

    public void OnIncorrectNoteBtnClick()
    {
        IncorrectNotePanel.SetActive(true);
        // 오답 추가.
        // 값 한번만 추가.


        // 값 초기화해야됌
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
