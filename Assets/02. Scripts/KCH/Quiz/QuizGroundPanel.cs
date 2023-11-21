using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizGroundPanel : MonoBehaviour
{

    public TextMeshProUGUI questionText;

    bool correctCheck;

    private void Update()
    {


    }

    // ��ư�� ������ �� correctCheck ������ ����ؼ� ���� ���� üũ�Ѵ�.

    public void quizdata(string question_, string answer_)
    {
        // ������ ���� �г��� ����ְ�

        questionText.text = question_;

        // �������� �������� üũ
        if (answer_ == "O") correctCheck = true;
        else correctCheck = false;

        // �л��� ������ Ǯ������ �������� ���������� ���� �����͸� �ش�.

        // ���⼱ ���� ��Ǯ���ٴ� ������ �����.
        // �̸��� ���� �����.
        string sendName = DataBase.instance.user.name;

        // �� Ǭ �ο��� �߰�.
        MyQuizStorage.Instance.sendUserQuizData();

        // ���忡���� ���� ����
        Quiz.instance.startquiz();

        // ������ ���� quiz ��ũ��Ʈ�� ��´�
        Quiz.instance.question = question_;
        Quiz.instance.answer = answer_;
        StartCoroutine(quizPaneldelete());
    }




    // �ð��� �� ������ �гο� ���� �����.
    // �ƴϴ� ���������� ����ִ°ɷ� �ؾ߰ڴ� �װ� �� ���ϰڳ�.

    IEnumerator quizPaneldelete()
    {
        yield return new WaitForSeconds(13);
        Destroy(gameObject);

    }
}
