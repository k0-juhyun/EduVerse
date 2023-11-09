using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClassRoomQuizLoad : MonoBehaviour
{   
    public List<Quiz_> quizs = new List<Quiz_>();

    public GameObject AddQuizViewport;
    public GameObject QuizPrefab;

    public TextMeshProUGUI questionText;

    [Space(20)]
    [Header("�ο�üũ")]
    public GameObject peopleCheckTextprefab;
    public GameObject �����ο�;
    public GameObject �����ο�;

    // ������
    // ����
    public enum QuizState
    {
        None,
        Proceeding,
        End
    }

    [Space(20)]
    public QuizState quizState = QuizState.None;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            quizs= MyQuizStorage.Instance.quizList;
            Debug.Log("����");
            for (int i = 0; i < MyQuizStorage.Instance.quizList.Count; i++)
            {
                Debug.Log(MyQuizStorage.Instance.quizList[i].question);
                Debug.Log(MyQuizStorage.Instance.quizList[i].answer);

                GameObject quiz_obj = Instantiate(QuizPrefab);
                quiz_obj.transform.parent = AddQuizViewport.transform;
                quiz_obj.GetComponent<LoadQuizPrefab>().Question_Answer(MyQuizStorage.Instance.quizList[i].question,
                    MyQuizStorage.Instance.quizList[i].answer);
            }
        }

        // ���⼭ ������ ����Ʈ �̾Ƽ� �����.
    }

     public void SelectQuiz(string question_, string answer_)
    {
        // questionText ���� ������ �ٲ�.
        questionText.text = question_;
        // ���� ���� �� �ο� �� �޴´�.
        // ���� �ο� ���� â ����.

        // QuizState.Proceeding �� ������ Ǯ�� ���� ���� üũ�� �Ѵ�.

        // ������ �� Ǯ�� �Ǹ� QuizState.End�� �Ѿ.
        // QuizState.End���� �ڷ�ƾ���� ����ƴ� ��Ȳ�� ���� �ϴ� �Լ� ������ ��.

    }



    // ����Ǵ� �Լ� 
    public void OnExitQuizBtnClick()
    {

    }
}
