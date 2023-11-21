using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IncorrectNote : MonoBehaviour
{
    // student quiz DB���� student QuizInfo�� �����ͼ�
    // �ܿ��� Incorrect Answer ����Ʈ ���� �ִ´�.

    public TextMeshProUGUI Unit_Text;
    public TextMeshProUGUI Title_Text;

    string Answer;
    string Commentary;

    public GameObject CommentaryPanel;

    public TextMeshProUGUI Question_Text;
    public TextMeshProUGUI Answer_Text;
    public TextMeshProUGUI Commentary_Text;

    // ��ư�� ������ �� �г��� �������
    // ������ �� �ؼ��� ������� �����.

    public void PutData(string unit_,string title_,string answer_,string commentary_)
    {
        Unit_Text.text = unit_; 
        Title_Text.text = title_;
        Answer = answer_;
        Commentary = commentary_;
    }

    public void OnLoadIncorrectAnswerBtnClick()
    {
        // text �� �־���.

        // ���� �г� �ٽ� �����.

        // ���� ������Ʈ�� �߰� 
        GameObject Panel = Instantiate(CommentaryPanel,transform.root);
        Panel.GetComponent<CommentaryPanel>().PutQuizData(Title_Text.text, Answer, Commentary);
    }
}
