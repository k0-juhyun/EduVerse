using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class Quiz_Individual : MonoBehaviourPun
{
    // 서버에 문제와 정답인지 오답인지 체크해서 보내줌.
    public static Quiz_Individual instance;

    // 정답 체크.
    public GameObject Answer_O;
    public GameObject Answer_X;

    private void Awake()
    {
        instance = this;
    }

    // Quiz 스크립트에서 이벤트 구독함.

    //private void OnEnable()
    //{
    //    Quiz.instance.QuizEnded += OnQuizEnded; // 이벤트 구독
    //}

    //private void OnDisable()
    //{
    //    Quiz.instance.QuizEnded -= OnQuizEnded; // 이벤트 해지
    //}


    //public void CorrectBtnClick()
    //{
    //    // 이 아이디의 UID를 확인하여 DB에 저장해야함.

    //    // 선생한테 제출할 데이터 
    //    // 나의 이름과 문제, 정답
    //    if (correctCheck)
    //    {
    //        // 정답
    //        Debug.Log("정답입니당");

    //        MyQuizStorage.Instance.sendUserQuizData(true);
    //        correct.SetActive(true);

    //        StartCoroutine(quizPaneldelete());
    //    }
    //    else
    //    {
    //        Debug.Log("오답입니당");
    //        MyQuizStorage.Instance.sendUserQuizData(false);
    //        incorrect.SetActive(true);

    //        StartCoroutine(quizPaneldelete());

    //    }
    //}

    //// 오답 제출.
    //public void IncorrectBtnClick()
    //{
    //    if (correctCheck)
    //    {
    //        MyQuizStorage.Instance.sendUserQuizData(false);
    //        incorrect.SetActive(true);

    //        Debug.Log("오답입니당");
    //        StartCoroutine(quizPaneldelete());

    //    }
    //    else
    //    {
    //        MyQuizStorage.Instance.sendUserQuizData(true);
    //        // 정답
    //        correct.SetActive(true);
    //        Debug.Log("정답입니당");
    //        StartCoroutine(quizPaneldelete());

    //    }
    //}


    public void OnQuizEnded()
    {
        //photonView.RPC(nameof(OX_GroundCheck), RpcTarget.All);
        OX_GroundCheck();
    }
  
    public void OX_GroundCheck()
    {
        Debug.Log("OX실행");
        RaycastHit hit;
        float rayLength = 1.0f; // 레이 길이
        Vector3 rayOrigin = transform.position; // 플레이어의 현재 위치
        Debug.Log(DataBase.instance.userInfo.name);
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayLength)&&!DataBase.instance.userInfo.isTeacher)
        {

            Debug.Log("이 문제의 정답은 : " + Quiz.instance.answer);
            if (hit.collider.name == "O")
            {
                Debug.Log("O");
                // 정답인지 체크해서 서버에 보내주는 부분

                // if 정답이면 
                if(Quiz.instance.answer=="O")
                { 
                    MyQuizStorage.Instance.sendUserQuizData(true);
                    StartCoroutine(answer(Answer_O));
                    Debug.Log("1");
                }
                else
                {
                    MyQuizStorage.Instance.sendUserQuizData(false);
                    StartCoroutine( answer(Answer_X));
                    Debug.Log("2");

                }

                // 오답 이면

            }
            else if(hit.collider.name == "X")
            {
                Debug.Log("X");
                // 오답인지 체크해서 서버에 보내주는 부분  

                if (Quiz.instance.answer == "O")
                {
                    MyQuizStorage.Instance.sendUserQuizData(false);
                    StartCoroutine(answer(Answer_X));
                    Debug.Log("3");

                }
                else
                {
                    MyQuizStorage.Instance.sendUserQuizData(true);
                    StartCoroutine(answer(Answer_O));
                    Debug.Log("4");

                }
            }
            else
            {
                MyQuizStorage.Instance.sendUserQuizData(false);
                StartCoroutine(answer(Answer_X));
                Debug.Log("OX발판으로 들어가세요!.");
            }
        }
    }

    
    IEnumerator answer(GameObject gameObject)
    {
        Debug.Log("실행");
        GameObject answercanvas = Instantiate(gameObject);
        yield return new WaitForSeconds(3);
        Destroy(answercanvas );

    }
}
