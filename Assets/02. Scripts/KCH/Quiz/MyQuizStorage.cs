using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Quiz_
{
    public string question;
    public string answer;
}
public class MyQuizStorage : MonoBehaviourPun
{
    public static MyQuizStorage Instance;
    public GameObject QuizPanel_student;
  
    public List<Quiz_> quizList;



    public delegate void Correctanswercheck(string answer);
    public Correctanswercheck correctAnswerCheck;

    public Correctanswercheck correctCheck;
    public Correctanswercheck incorrectCheck;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(Instance);
    }


    public void SelectQuiz(string question_, string answer_)
    {
        photonView.RPC(nameof(Photon_SelectQuiz), RpcTarget.All, question_, answer_);
    }

    [PunRPC]
    public void Photon_SelectQuiz(string question_, string answer_)
    {
        // questionText ���� ������ �ٲ�.
        // ���� ���� �� �ο� �� �޴´�.
        // ���� �ο� ���� â ����.

        if (DataBase.instance.userInfo.isTeacher == false)
        {
            Debug.Log("����");
            GameObject quizPanel = Instantiate(QuizPanel_student);

            quizPanel.GetComponent<QuizSubmit_student>().quizdata(question_, answer_);
            //quizPanel�� ������ ������ ������ش�.
        }

        
        // QuizState.Proceeding �� ������ Ǯ�� ���� ���� üũ�� �Ѵ�.

        // ������ �� Ǯ�� �Ǹ� QuizState.End�� �Ѿ.
        // QuizState.End���� �ڷ�ƾ���� ����ƴ� ��Ȳ�� ���� �ϴ� �Լ� ������ ��.
    }




    public void sendUserQuizData()
    {
        photonView.RPC(nameof(Photon_sendUserQuizData), RpcTarget.All, DataBase.instance.userInfo.name);
    }
    [PunRPC]
    public void Photon_sendUserQuizData(string name)
    {
        // ������ ������ �ް� ����
        if (DataBase.instance.userInfo.isTeacher)
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
        photonView.RPC(nameof(Photon_sendUserQuizData), RpcTarget.All, correct, DataBase.instance.userInfo.name);
    }
    [PunRPC]
    public void Photon_sendUserQuizData(bool correct,string name)
    {
        // ������ ������ �ް� ����
        if (DataBase.instance.userInfo.isTeacher)
        {
            // Ǭ �̸�.
            Debug.Log(name+ " ���� üũ : "+ correct);

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
