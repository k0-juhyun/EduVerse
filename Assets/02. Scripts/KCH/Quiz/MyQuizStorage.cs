using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct Quiz_
{
    public string question;
    public string answer;
    public string unit;
    public string commentary;
}
public class MyQuizStorage : MonoBehaviourPun
{
    public static MyQuizStorage Instance;
    public GameObject QuizPanel_student;
    public GameObject QuizPanel_student_Ground;

    public List<Quiz_> quizList;

    public delegate void Correctanswercheck(string answer);
    public Correctanswercheck correctAnswerCheck;

    public Correctanswercheck correctCheck;
    public Correctanswercheck incorrectCheck;

    bool isGroundCheck = false;

    // quizSubmit_student에서 사용.
    public string UnitCheck;
    public string Commentary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void SelectQuiz(string question_, string answer_,string unit_,string commentary_)
    {
        photonView.RPC(nameof(Photon_SelectQuiz), RpcTarget.All, question_, answer_, unit_, commentary_);
    }

    [PunRPC]
    public void Photon_SelectQuiz(string question_, string answer_, string unit_, string commentary_)
    {
        // questionText 현재 문서로 바꿈.
        // 현재 들어온 총 인원 값 받는다.
        // 들어온 인원 퀴즈 창 띄우기.


        // 광장 이라면.
        if (DataBase.instance.user.isTeacher == false && SceneManager.GetActiveScene().name == "5.GroundScene")
        {
            Debug.Log("실행");
            GameObject quizPanel = Instantiate(QuizPanel_student_Ground);

            quizPanel.GetComponent<RectTransform>().position = new Vector3(14, 5.6f, 8);
            quizPanel.GetComponent<QuizGroundPanel>().quizdata(question_, answer_, unit_ ,commentary_);
            //quizPanel의 문제와 정답을 등록해준다.
        }
        // 교실이라면
        if (DataBase.instance.user.isTeacher == false && SceneManager.GetActiveScene().name != "5.GroundScene")
        {
            Debug.Log("실행");
            GameObject quizPanel = Instantiate(QuizPanel_student);

            quizPanel.GetComponent<QuizSubmit_student>().quizdata(question_, answer_, unit_, commentary_);
            //quizPanel의 문제와 정답을 등록해준다.
        }

        // QuizState.Proceeding 중 문제를 풀어 정답 오답 체크를 한다.

        // 문제를 다 풀게 되면 QuizState.End로 넘어감.
        // QuizState.End에서 코루틴으로 진행됐던 상황들 리셋 하는 함수 만들어야 함.
    }




    public void sendUserQuizData()
    {
        photonView.RPC(nameof(Photon_sendUserQuizData), RpcTarget.All, DataBase.instance.user.name);
    }
    [PunRPC]
    public void Photon_sendUserQuizData(string name)
    {
        // 선생만 데이터 받게 설정
        if (DataBase.instance.user.isTeacher)
        {
            // 못 푼 인원
            Debug.Log(name);

            // TeacherQuizCanvas에 추가를 해야함
            // 어떻게 하면 좋을까?
            // 함수를 담아서 그 함수 실행하면 되겠네

            // 못푼 인원에 체크.
            correctAnswerCheck(name);

        }
    }

    // sendUserQuizData 오버로딩.
    public void sendUserQuizData(bool correct)
    {
        photonView.RPC(nameof(Photon_sendUserQuizData), RpcTarget.All, correct, DataBase.instance.user.name);
    }
    [PunRPC]
    public void Photon_sendUserQuizData(bool correct, string name)
    {
        // 선생만 데이터 받게 설정
        if (DataBase.instance.user.isTeacher)
        {
            // 푼 이름.
            Debug.Log(name + " 오답 체크 : " + correct);

            // 못 푼 인원에 들어가있는 유저 데이터 지우고
            if (correct)
                correctCheck(name);
            else
                incorrectCheck(name);
            // 푼 인원( 정답 오답 체크) 후 그 에 맞는 장소에 instantiate 한다.
            // text에 있는 이름을 가지고 일단은 하는걸로.
        }
    }
}
