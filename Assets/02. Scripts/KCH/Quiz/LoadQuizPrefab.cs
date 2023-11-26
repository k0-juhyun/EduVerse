using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LoadQuizPrefab : MonoBehaviourPun
{
  
    public Text question;

    // O/X üũ
    public GameObject O_object;
    public GameObject X_object;

    [Space (20)]
    // ���
    public GameObject CheckOn;
    public GameObject CheckOff;

    bool save = false;

    [Space(20)]
    public GameObject CommentaryPanel;
 

    // MyQuizStorage�� ���� ���
    Quiz_ quiz = new Quiz_();

    public void Question_Answer(string title, string answer,string unit,string commentary)
    {
        // ���� ���
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
        // ���� ���.
        if (CheckOff.activeSelf)
        {
            CheckOn.SetActive(true);
            CheckOff.SetActive(false);
            Debug.Log(quiz.commentary);
            // ���� ���
            MyQuizStorage.Instance.quizList.Add(quiz);
        }
        // ���� ��� ����
        else if (!CheckOff.activeSelf)
        {
            CheckOn.SetActive(false);
            CheckOff.SetActive(true);

            // ���� ��� ����
            MyQuizStorage.Instance.quizList.Remove(quiz);
        }
    }

    // �ڸ�Ʈ �����.
    public void OnCommentaryBtnClick()
    {
        GameObject panel = Instantiate(CommentaryPanel,transform.root);
        panel.GetComponent<CommentaryPanel>().PutQuizData(quiz.question, quiz.answer, quiz.commentary);
    }

    // ���忡���� ����.
    public void SelectQuizBtnClick()
    {

        // �θ� ������Ʈ�� ClassRoomQuizLoad ��ũ��Ʈ�� �� ����.
        // �ʿ��� �� ���� ���� string �� string

        // �θ� ������Ʈ�� ClassRoomQuizLoad�� ���� �κ� question text ����
        Transform parentobj = FindRootParent(transform.gameObject);
        parentobj.gameObject.GetComponent<ClassRoomQuizLoad>().SelectQuiz(quiz.question, quiz.answer);

        // quiz ����ҿ��� ������ ��
        MyQuizStorage.Instance.SelectQuiz(quiz.question, quiz.answer,quiz.unit,quiz.commentary);
        Debug.Log(quiz.question + " Load QuizPrefab " + quiz.answer);

    }

    // �θ� ������Ʈ ã�� �Լ�.
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


