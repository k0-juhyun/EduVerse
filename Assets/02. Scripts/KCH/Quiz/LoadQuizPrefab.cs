using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadQuizPrefab : MonoBehaviour
{
  
    public Text question;

    // O/X 체크
    public GameObject O_object;
    public GameObject X_object;

    [Space (20)]
    // 담기
    public GameObject CheckOn;
    public GameObject CheckOff;

    bool save = false;

    // MyQuizStorage에 퀴즈 담기
    Quiz_ quiz = new Quiz_();
    public void Question_Answer(string title, string answer)
    {
        // 퀴즈 담기
        quiz.question = title;
        quiz.answer = answer;

        question.text = title;
        if (answer=="O") O_object.SetActive(true);
        else X_object.SetActive(true);
    }

    public void SaveQuestion()
    {
        // 문제 담기.
        if (CheckOff.activeSelf)
        {
            CheckOn.SetActive(true);
            CheckOff.SetActive(false);

            // 퀴즈 담기
            MyQuizStorage.Instance.quizList.Add(quiz);
        }
        // 문제 담기 해제
        else if (!CheckOff.activeSelf)
        {
            CheckOn.SetActive(false);
            CheckOff.SetActive(true);

            // 퀴즈 담기 해제
            MyQuizStorage.Instance.quizList.Remove(quiz);
        }
    }

}


