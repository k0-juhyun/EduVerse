using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using static MyQuizStorage;
using DG.Tweening;

public class ClassRoomQuizLoad : MonoBehaviourPun
{


    public GameObject AddQuizViewport;
    public GameObject QuizPrefab;

    public TextMeshProUGUI questionText;

    [Space(20)]
    [Header("인원체크")]
    public GameObject peopleCheckTextprefab;
    public GameObject 정답인원;
    public GameObject 오답인원;
    public GameObject 못푼인원;

    [Space(20)]
    public GameObject 문제은행;
    public GameObject QuizIcon;


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
    public GameObject QuizPanel_student;

    // 문제와 답.
    string Question;
    string Answer;
    string Commentary;


    private void Start()
    {

        for (int i = 0; i < MyQuizStorage.Instance.quizList.Count; i++)
        {
            Debug.Log(MyQuizStorage.Instance.quizList[i].question);
            GameObject quiz_obj = Instantiate(QuizPrefab);
            quiz_obj.transform.parent = AddQuizViewport.transform;
            quiz_obj.transform.localScale = new Vector3(1, 1, 1);
            quiz_obj.GetComponent<LoadQuizPrefab>().Question_Answer(MyQuizStorage.Instance.quizList[i].question,
            MyQuizStorage.Instance.quizList[i].answer, MyQuizStorage.Instance.quizList[i].unit, MyQuizStorage.Instance.quizList[i].commentary);
        }
        // 여기서 저장한 리스트 뽑아서 써야함.




        // 인원 체크 델리게이트
        MyQuizStorage.Instance.correctAnswerCheck = QuizNotYetSolvePeopleCheck;
        MyQuizStorage.Instance.correctCheck = QuizSolvePeopleCheck_O;
        MyQuizStorage.Instance.incorrectCheck = QuizSolvePeopleCheck_X;

    }
    void Update()
    {
        // 퀴즈 다시 생성
        if (Input.GetKeyDown(KeyCode.M))
        {

            transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InBack);
        }
    }

    // 종료되는 함수 
    public void OnExitQuizBtnClick()
    {      
        photonView.RPC(nameof(deletePanel), RpcTarget.All);
        // 퀴즈 종료 버튼
        if(Quiz.instance !=null)
            Quiz.instance.EndQuiz();
    }

    [PunRPC]
    public void deletePanel()
    {
        문제은행.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InBack).OnComplete(() => {

            Debug.Log("이거실행됌?");

            Destroy(gameObject);
        });
    }

    // 광장에서만 쓰는 버튼
    public void OndisappearCanvasBtnClick()
    {
        // 다시 키게 만드는것도 만들어야함.
        문제은행.transform.DOScale(0,0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            QuizIcon.transform.DOScale(1, 0.5f).SetEase(Ease.OutCubic);
        });
    }

    public void OnappearCanvasBtnClick()
    {
        Debug.Log("실행");
        QuizIcon.transform.DOScale(0, 0.2f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            문제은행.transform.DOScale(1, 0.5f).SetEase(Ease.OutCubic);
        });
    }


    // 인원 체크 프리팹
    public void QuizNotYetSolvePeopleCheck(string str)
    {
        // 현재 문제 텍스트 띄워줌

        // 못푼 인원 체크.
        GameObject checkobj = Instantiate(peopleCheckTextprefab);
        checkobj.transform.parent = 못푼인원.transform;
        Debug.Log(str);

        checkobj.GetComponentInChildren<TextMeshProUGUI>().text = str;
    }

    // 정답 인원 체크
    // 체크 후 Firebase에 저장해야함.
    public void QuizSolvePeopleCheck_O(string str)
    {
        TextMeshProUGUI[] text = 못푼인원.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI item in text)
        {
            if (item.text == str)
            {
                Destroy(item.gameObject);
                break;
            }
        }

        // 못푼 인원 체크.
        GameObject checkobj = Instantiate(peopleCheckTextprefab);
        checkobj.transform.parent = 정답인원.transform;

        checkobj.GetComponentInChildren<TextMeshProUGUI>().text = str;

        // 체크 후 Firebase에 저장해야함.


    }
    public void QuizSolvePeopleCheck_X(string str)
    {

        TextMeshProUGUI[] text = 못푼인원.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI item in text)
        {
            if (item.text == str)
            {
                Destroy(item.gameObject);
                break;
            }
        }

        // 못푼 인원 체크.
        GameObject checkobj = Instantiate(peopleCheckTextprefab);
        checkobj.transform.parent = 오답인원.transform;

        checkobj.GetComponentInChildren<TextMeshProUGUI>().text = str;

        // 체크 후 Firebase에 저장해야함.

    }

    public void SelectQuiz(string question_, string answer_)
    {
        // 여기서 문제와 정답을 저장
        questionText.text = question_;

        // 만약 그 전에 문제를 풀었다면 그 정답자와 오답자 데이터 Json형태 또는 firebase로 저장

        // 그전에 문제를 풀었다면 정답자 오답자 체크 해제 해줌.

        // 그 문제와 답에 맞는 학생들 DB에 저장 하는 함수를 만들어야함.

        TextMeshProUGUI[] text = 정답인원.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI item in text)
        {
            Destroy(item.gameObject);
        }
        TextMeshProUGUI[] text_ = 오답인원.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI item in text_)
        {
            Destroy(item.gameObject);
        }

    }
    
}
