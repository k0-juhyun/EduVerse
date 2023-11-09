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
    [Header("인원체크")]
    public GameObject peopleCheckTextprefab;
    public GameObject 정답인원;
    public GameObject 오답인원;

    // 진행중
    // 종료
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
            Debug.Log("실행");
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

        // 여기서 저장한 리스트 뽑아서 써야함.
    }

     public void SelectQuiz(string question_, string answer_)
    {
        // questionText 현재 문서로 바꿈.
        questionText.text = question_;
        // 현재 들어온 총 인원 값 받는다.
        // 들어온 인원 퀴즈 창 띄우기.

        // QuizState.Proceeding 중 문제를 풀어 정답 오답 체크를 한다.

        // 문제를 다 풀게 되면 QuizState.End로 넘어감.
        // QuizState.End에서 코루틴으로 진행됐던 상황들 리셋 하는 함수 만들어야 함.

    }



    // 종료되는 함수 
    public void OnExitQuizBtnClick()
    {

    }
}
