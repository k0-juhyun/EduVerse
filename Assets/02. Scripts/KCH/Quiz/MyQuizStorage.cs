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

    // quizSubmit_student���� ���.
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
        // questionText ���� ������ �ٲ�.
        // ���� ���� �� �ο� �� �޴´�.
        // ���� �ο� ���� â ����.


        // ���� �̶��.
        if (DataBase.instance.user.isTeacher == false && SceneManager.GetActiveScene().name == "5.GroundScene")
        {
            Debug.Log("����");
            GameObject quizPanel = Instantiate(QuizPanel_student_Ground);

            quizPanel.GetComponent<RectTransform>().position = new Vector3(14, 5.6f, 8);
            quizPanel.GetComponent<QuizGroundPanel>().quizdata(question_, answer_, unit_ ,commentary_);
            //quizPanel�� ������ ������ ������ش�.
        }
        // �����̶��
        if (DataBase.instance.user.isTeacher == false && SceneManager.GetActiveScene().name != "5.GroundScene")
        {
            Debug.Log("����");
            GameObject quizPanel = Instantiate(QuizPanel_student);

            quizPanel.GetComponent<QuizSubmit_student>().quizdata(question_, answer_, unit_, commentary_);
            //quizPanel�� ������ ������ ������ش�.
        }

        // QuizState.Proceeding �� ������ Ǯ�� ���� ���� üũ�� �Ѵ�.

        // ������ �� Ǯ�� �Ǹ� QuizState.End�� �Ѿ.
        // QuizState.End���� �ڷ�ƾ���� ����ƴ� ��Ȳ�� ���� �ϴ� �Լ� ������ ��.
    }




    public void sendUserQuizData()
    {
        photonView.RPC(nameof(Photon_sendUserQuizData), RpcTarget.All, DataBase.instance.user.name);
    }
    [PunRPC]
    public void Photon_sendUserQuizData(string name)
    {
        // ������ ������ �ް� ����
        if (DataBase.instance.user.isTeacher)
        {
            // �� Ǭ �ο�
            Debug.Log(name);

            // TeacherQuizCanvas�� �߰��� �ؾ���
            // ��� �ϸ� ������?
            // �Լ��� ��Ƽ� �� �Լ� �����ϸ� �ǰڳ�

            // ��Ǭ �ο��� üũ.
            correctAnswerCheck(name);

        }
    }

    // sendUserQuizData �����ε�.
    public void sendUserQuizData(bool correct)
    {
        photonView.RPC(nameof(Photon_sendUserQuizData), RpcTarget.All, correct, DataBase.instance.user.name);
    }
    [PunRPC]
    public void Photon_sendUserQuizData(bool correct, string name)
    {
        // ������ ������ �ް� ����
        if (DataBase.instance.user.isTeacher)
        {
            // Ǭ �̸�.
            Debug.Log(name + " ���� üũ : " + correct);

            // �� Ǭ �ο��� ���ִ� ���� ������ �����
            if (correct)
                correctCheck(name);
            else
                incorrectCheck(name);
            // Ǭ �ο�( ���� ���� üũ) �� �� �� �´� ��ҿ� instantiate �Ѵ�.
            // text�� �ִ� �̸��� ������ �ϴ��� �ϴ°ɷ�.
        }
    }
}
