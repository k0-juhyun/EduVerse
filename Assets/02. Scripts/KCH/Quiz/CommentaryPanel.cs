using DG.Tweening;
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
        Answer_text.text = "Á¤´ä : " + answer_;
        Commentary_text.text = commentary_;
    }
    public void OffPanelBtnClick()
    {
        transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InBack).OnComplete(() => Destroy(gameObject));


    }
}
