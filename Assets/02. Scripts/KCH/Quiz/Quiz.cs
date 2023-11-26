using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using static ClassRoomQuizLoad;

public class Quiz : MonoBehaviourPun
{

    #region �̱���
    static public Quiz instance;
    private void Awake()
    {
        instance = this; 
    }
    #endregion

    // �Լ� ���� ��������Ʈ
    public event System.Action QuizEnded;

    public float setTime;
    float originTime;

    // 
    public bool isQuiz = false;

    // ��� ���� ������ ����
    [HideInInspector] public string unit;
    [HideInInspector] public string question;
    [HideInInspector] public string answer;
    [HideInInspector] public string commentary;

    GameObject quizPanel;
    public TextMeshProUGUI quizTime;
    public TextMeshProUGUI quizTime2;
    public GameObject Quizplate;
    public GameObject Logo;

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
                // ���� ���� �̺�Ʈ

                QuizEnded?.Invoke();
            }
        }
        else
        {
            setTime = originTime;
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            // �������� ���� �г� ����
            OnQuizBtnClick();

        }

        // ���� ���� 
        if (isQuiz)
        {
            time_ = Mathf.FloorToInt(Quiz.instance.setTime);

            if(quizTime.transform.gameObject.activeSelf)
                quizTime.text = time_.ToString();

            if (quizTime2.transform.gameObject.activeSelf)
                quizTime2.text = time_.ToString();

            if (0.1f >= Quiz.instance.setTime)
            {
                quizTime.text = "�ð�����";
                quizTime2.text = "�ð�����";
            }
        }

    }


    public void startquiz()
    {
        photonView.RPC(nameof(startquizRPC), RpcTarget.All);
    }
    [PunRPC]
    public void startquizRPC()
    {
        isQuiz = true;

    }


    // �������׸� ���� �г� ����ֱ�.
    public void OnQuizBtnClick()
    {
        // ������ instantsiate �Ѵ� (�����ν��Ͻÿ���Ʈ X)
        // ������ ���� �г� ���� ��ư.
        quizPanel = PhotonNetwork.Instantiate("Teacher_QuizCanvas_Ground", Vector3.zero, Quaternion.identity);
        //GameObject quizPanel = Instantiate(QuizMenu, Vector3.zero, Quaternion.identity);
        photonView.RPC(nameof(DestroyOtherQuizPanels), RpcTarget.All);
    }
    
    // �л����� ���� �г� �����.
    [PunRPC]
    private void DestroyOtherQuizPanels()
    {

        if(DataBase.instance.userInfo.isteacher)
            quizTime2.gameObject.SetActive(true);
        else quizTime.gameObject.SetActive(true);

                // OX ���� ����ֱ�
                Quizplate.SetActive(true);
        Logo.SetActive(false);
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

    public void EndQuiz()
    {
        photonView.RPC(nameof(QuizPlateDestory), RpcTarget.All);
    }

    [PunRPC]
    public void QuizPlateDestory()
    {
        // �ٴ� �÷���Ʈ ���ֱ�.
        Quizplate.SetActive(false);
        Logo.SetActive(true);
    }
}
