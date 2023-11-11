using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizSubmit_student : MonoBehaviour
{

    public TextMeshProUGUI questionText;

    public GameObject QuizPrefab;
    public GameObject correct;
    public GameObject incorrect;


    bool correctCheck;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            if (DataBase.instance.userInfo.isTeacher == false)
            {
                Debug.Log("����");
                GameObject quizPanel = Instantiate(QuizPrefab);
            }
        }
    }

    // ��ư�� ������ �� correctCheck ������ ����ؼ� ���� ���� üũ�Ѵ�.

    void mydatabase()
    { 
        string name = DataBase.instance.userInfo.name;
    }
    // ���� ����
    public void CorrectBtnClick()
    {
        // �� ���̵��� UID�� Ȯ���Ͽ� DB�� �����ؾ���.

        // �������� ������ ������ 
        // ���� �̸��� ����, ����
        if(correctCheck)
        {
            // ����
            Debug.Log("�����Դϴ�");

            MyQuizStorage.Instance.sendUserQuizData(true);
            correct.SetActive(true);

            StartCoroutine(quizPaneldelete());
        }
        else
        {
            Debug.Log("�����Դϴ�");
            MyQuizStorage.Instance.sendUserQuizData(false);
            incorrect.SetActive(true);

            StartCoroutine(quizPaneldelete());

        }
    }

    // ���� ����.
    public void IncorrectBtnClick()
    {
        if (correctCheck)
        {
            MyQuizStorage.Instance.sendUserQuizData(false);
            incorrect.SetActive(true);

            Debug.Log("�����Դϴ�");
            StartCoroutine(quizPaneldelete());

        }
        else
        {
            MyQuizStorage.Instance.sendUserQuizData(true);
            // ����
            correct.SetActive(true);
            Debug.Log("�����Դϴ�");
            StartCoroutine(quizPaneldelete());

        }
    }


    public void quizdata(string question_,string answer_)
    {
        // ������ ���� �г��� ����ְ�

        questionText.text = question_;

        // �������� �������� üũ
        if (answer_ == "O") correctCheck = true;
        else correctCheck = false;

        // �л��� ������ Ǯ������ �������� ���������� ���� �����͸� �ش�.

        // ���⼱ ���� ��Ǯ���ٴ� ������ �����.
        // �̸��� ���� �����.
        string sendName = DataBase.instance.userInfo.name;

        // �� Ǭ �ο��� �߰�.
        MyQuizStorage.Instance.sendUserQuizData();

        // Don't destoryobject�Ǿ��ִ� MyQuizStorage�� ����.
        // ����� ���������� �����Ǿ��ְ� ���� �� �������״� �� ������Ʈ�� ���� ������
        // �л� json���� DB ���� �ϴ� ���߿� �ϴ°ɷ� �ϰ� �ϴ� 
        // ���� �гο� Ǭ �ο��� ��Ǭ �ο� ������ �ɷ� ����.

        // ��Ǭ �ο��� �߰�
        // Ǯ�� ��Ǭ �ο��� �ִ� ������ �����
        // 


    }
    IEnumerator quizPaneldelete()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
