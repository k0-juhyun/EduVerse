using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LoadQuizPrefab : MonoBehaviourPun
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

    [Space(20)]
    public GameObject CommentaryPanel;
 

    // MyQuizStorage에 퀴즈 담기
    Quiz_ quiz = new Quiz_();

    public void Question_Answer(string title, string answer,string unit,string commentary)
    {
        // 퀴즈 담기
        quiz.question = title;
        quiz.answer = answer;
        quiz.unit = unit;
        quiz.commentary = commentary;
        question.text = title;
        if (answer=="O") O_object.SetActive(true);
        else X_object.SetActive(true);
    }

    public void SaveQuestion()
    {
        SoundManager.instance?.PlaySFX(SoundManager.SFXClip.Button2);
        // 문제 담기.
        if (CheckOff.activeSelf)
        {
            CheckOn.SetActive(true);
            CheckOff.SetActive(false);
            Debug.Log(quiz.commentary);
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

    // 코멘트 띄워줌.
    public void OnCommentaryBtnClick()
    {
        GameObject panel = Instantiate(CommentaryPanel,transform.root);
        panel.GetComponent<CommentaryPanel>().PutQuizData(quiz.question, quiz.answer, quiz.commentary);
    }

    // 광장에서의 퀴즈.
    public void SelectQuizBtnClick()
    {

        // 부모 오브젝트의 ClassRoomQuizLoad 스크립트에 값 전달.
        // 필요한 값 현재 문제 string 답 string

        // 부모 오브젝트에 ClassRoomQuizLoad의 문제 부분 question text 갱신
        Transform parentobj = FindRootParent(transform.gameObject);
        parentobj.gameObject.GetComponent<ClassRoomQuizLoad>().SelectQuiz(quiz.question, quiz.answer);

        // quiz 저장소에서 가져다 씀
        MyQuizStorage.Instance.SelectQuiz(quiz.question, quiz.answer,quiz.unit,quiz.commentary);
        Debug.Log(quiz.question + " Load QuizPrefab " + quiz.answer);

    }

    // 부모 오브젝트 찾는 함수.
    Transform FindRootParent(GameObject childObject)
    {
        Transform parent = childObject.transform.parent;
        while (parent.parent != null)
        {
            parent = parent.parent;
        }
        return parent;
    }

    
}


