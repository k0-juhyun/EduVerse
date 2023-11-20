using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IncorrectNote : MonoBehaviour
{
    // student quiz DB에서 student QuizInfo를 가져와서
    // 단원별 Incorrect Answer 리스트 값을 넣는다.

    public TextMeshProUGUI Unit_Text;
    public TextMeshProUGUI Title_Text;

    string Answer;
    string Commentary;

    public GameObject CommentaryPanel;

    public TextMeshProUGUI Question_Text;
    public TextMeshProUGUI Answer_Text;
    public TextMeshProUGUI Commentary_Text;

    // 버튼을 눌렀을 때 패널이 띄워지고
    // 문제와 답 해설이 띄워지게 만든다.

    public void PutData(string unit_,string title_,string answer_,string commentary_)
    {
        Unit_Text.text = unit_; 
        Title_Text.text = title_;
        Answer = answer_;
        Commentary = commentary_;
    }

    public void OnLoadIncorrectAnswerBtnClick()
    {
        // text 값 넣어줌.

        // 오답 패널 다시 띄워줌.

        // 상위 오브젝트에 추가 
        GameObject Panel = Instantiate(CommentaryPanel,transform.root);
        Panel.GetComponent<CommentaryPanel>().PutQuizData(Title_Text.text, Answer, Commentary);
    }
}
