using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class Quiz_Individual : MonoBehaviourPun
{
    // ������ ������ �������� �������� üũ�ؼ� ������.
    public static Quiz_Individual instance;

    // ���� üũ.
    public GameObject Answer_O;
    public GameObject Answer_X;

    private void Awake()
    {
        instance = this;
    }

    // Quiz ��ũ��Ʈ���� �̺�Ʈ ������.

    //private void OnEnable()
    //{
    //    Quiz.instance.QuizEnded += OnQuizEnded; // �̺�Ʈ ����
    //}

    //private void OnDisable()
    //{
    //    Quiz.instance.QuizEnded -= OnQuizEnded; // �̺�Ʈ ����
    //}


    //public void CorrectBtnClick()
    //{
    //    // �� ���̵��� UID�� Ȯ���Ͽ� DB�� �����ؾ���.

    //    // �������� ������ ������ 
    //    // ���� �̸��� ����, ����
    //    if (correctCheck)
    //    {
    //        // ����
    //        Debug.Log("�����Դϴ�");

    //        MyQuizStorage.Instance.sendUserQuizData(true);
    //        correct.SetActive(true);

    //        StartCoroutine(quizPaneldelete());
    //    }
    //    else
    //    {
    //        Debug.Log("�����Դϴ�");
    //        MyQuizStorage.Instance.sendUserQuizData(false);
    //        incorrect.SetActive(true);

    //        StartCoroutine(quizPaneldelete());

    //    }
    //}

    //// ���� ����.
    //public void IncorrectBtnClick()
    //{
    //    if (correctCheck)
    //    {
    //        MyQuizStorage.Instance.sendUserQuizData(false);
    //        incorrect.SetActive(true);

    //        Debug.Log("�����Դϴ�");
    //        StartCoroutine(quizPaneldelete());

    //    }
    //    else
    //    {
    //        MyQuizStorage.Instance.sendUserQuizData(true);
    //        // ����
    //        correct.SetActive(true);
    //        Debug.Log("�����Դϴ�");
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
        Debug.Log("OX����");
        RaycastHit hit;
        float rayLength = 1.0f; // ���� ����
        Vector3 rayOrigin = transform.position; // �÷��̾��� ���� ��ġ
        Debug.Log(DataBase.instance.userInfo.name);
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayLength)&&!DataBase.instance.userInfo.isTeacher)
        {

            Debug.Log("�� ������ ������ : " + Quiz.instance.answer);
            if (hit.collider.name == "O")
            {
                Debug.Log("O");
                // �������� üũ�ؼ� ������ �����ִ� �κ�

                // if �����̸� 
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

                // ���� �̸�

            }
            else if(hit.collider.name == "X")
            {
                Debug.Log("X");
                // �������� üũ�ؼ� ������ �����ִ� �κ�  

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
                Debug.Log("OX�������� ������!.");
            }
        }
    }

    
    IEnumerator answer(GameObject gameObject)
    {
        Debug.Log("����");
        GameObject answercanvas = Instantiate(gameObject);
        yield return new WaitForSeconds(3);
        Destroy(answercanvas );

    }
}
