using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using static ClassRoomQuizLoad;

public class Quiz : MonoBehaviourPun
{

    #region 싱글톤
    static public Quiz instance;
    private void Awake()
    {
        instance = this; 
    }
    #endregion

    // 함수 담을 델리게이트
    public event System.Action QuizEnded;

    public float setTime;
    float originTime;

    // 
    public bool isQuiz = false;

    // 퀴즈에 대한 문제와 정답
    [HideInInspector] public string question;
    [HideInInspector] public string answer;

    GameObject quizPanel;
    public TextMeshProUGUI quizTime;
    public GameObject Quizplate;

    int time_;

    private void Start()
    {
        originTime = setTime;
        QuizEnded += Quiz_Individual.instance.OnQuizEnded;
    }
    private void OnDisable()
    {
        QuizEnded -= Quiz_Individual.instance.OnQuizEnded;
    }

    void Update()
    {
        if (isQuiz)
        {
            setTime -= Time.deltaTime;
            if (setTime <= 0)
            {
                isQuiz = false;
                // 퀴즈 종료 이벤트

                QuizEnded?.Invoke();
            }
        }
        else
        {
            setTime = originTime;
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            // 선생님의 퀴즈 패널 띄우기
            OnQuizBtnClick();

        }

        // 퀴즈 시작 
        if (isQuiz)
        {
            time_ = Mathf.FloorToInt(Quiz.instance.setTime);
            quizTime.text = time_.ToString();
            if (0.1f >= Quiz.instance.setTime)
            {
                quizTime.text = "땡";
            }
        }
        if(Input.GetKeyDown (KeyCode.S))
        {
            photonView.RPC(nameof(startquiz), RpcTarget.All);
        }
    }

    [PunRPC]
    public void startquiz()
    {
        isQuiz = true;
    }


    // 선생한테만 퀴즈 패널 띄워주기.
    public void OnQuizBtnClick()
    {
        // 프리팹 instantsiate 한다 (포톤인스턴시에이트 X)
        // 선생이 퀴즈 패널 여는 버튼.
        quizPanel = PhotonNetwork.Instantiate("Teacher_QuizCanvas_Ground", Vector3.zero, Quaternion.identity);
        //GameObject quizPanel = Instantiate(QuizMenu, Vector3.zero, Quaternion.identity);
        photonView.RPC(nameof(DestroyOtherQuizPanels), RpcTarget.All);
    }
    
    // 학생들의 퀴즈 패널 지우기.
    [PunRPC]
    private void DestroyOtherQuizPanels()
    {
        // OX 발판 띄워주기
        Quizplate.SetActive(true);

        GameObject[] quizPanels = GameObject.FindGameObjectsWithTag("QuizPanel");

        foreach (GameObject panel in quizPanels)
        {
            if (!panel.GetPhotonView().IsMine)
            {
                //Destroy(panel);
                panel.GetComponent<Canvas>().enabled = false;
            }
        }
    }



}
