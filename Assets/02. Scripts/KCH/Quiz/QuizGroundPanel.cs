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
        string sendName = DataBase.instance.userInfo.name;

        // �� Ǭ �ο��� �߰�.
        MyQuizStorage.Instance.sendUserQuizData();

        // Don't destoryobject�Ǿ��ִ� MyQuizStorage�� ����.
        // ����� ���������� �����Ǿ��ְ� ���� �� �������״� �� ������Ʈ�� ���� ������
        // �л� json���� DB ���� �ϴ� ���߿� �ϴ°ɷ� �ϰ� �ϴ� 
        // ���� �гο� Ǭ �ο��� ��Ǭ �ο� ������ �ɷ� ����.

        // ��Ǭ �ο��� �߰�
        // Ǯ�� ��Ǭ �ο��� �ִ� ������ �����

    }
    IEnumerator quizPaneldelete()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
