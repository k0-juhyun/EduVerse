using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommentaryPanel : MonoBehaviour
{
    public TextMeshProUGUI Question_text;
    public TextMeshProUGUI Answer_text;
    public TextMeshProUGUI Commentary_text;

    public void PutQuizData(string question_,string answer_, string commentary_)
    {
        Question_text.text = question_;
        Answer_text.text = "���� : " + answer_;
        Commentary_text.text = commentary_;
    }
    public void OffPanelBtnClick()
    {
        Destroy(gameObject);
    }
}
