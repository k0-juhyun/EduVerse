using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizGroundPanel : MonoBehaviour
{

    public TextMeshProUGUI questionText;

    bool correctCheck;

    public string Question;
    public string Answer;
    public string Unit;
    public string Commentary;


    private void Update()
    {


    }

    // ��ư�� ������ �� correctCheck ������ ����ؼ� ���� ���� üũ�Ѵ�.

    public void quizdata(string question_, string answer_,string unit_, string commentary_)
    {
        // ������ ���� �г��� ����ְ�

        questionText.text = question_;

        Question = question_;
        Answer = answer_;
        Unit = unit_;
        Commentary = commentary_;

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
        Quiz.instance.unit = unit_;
        Quiz.instance.question = question_;
        Quiz.instance.answer = answer_;
        Quiz.instance.commentary = commentary_;
        // Quiz ��ũ��Ʈ�� �ؼ��κ� �� ������.
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
