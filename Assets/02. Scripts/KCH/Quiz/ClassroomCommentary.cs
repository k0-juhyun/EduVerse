using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClassroomCommentary : MonoBehaviour
{
    public TextMeshProUGUI Answer;
    public TextMeshProUGUI Commentary;

    public void PutAnswer_Commentary(string answer_,string commentary_)
    {
        Debug.Log(answer_+ " : " + commentary_);
        Answer.text = "Á¤´ä : " + answer_;
        Commentary.text = commentary_;
    }
    public void OndestoryPanel()
    {
        transform.parent.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InBack).OnComplete(() => Destroy(transform.root.gameObject));
    }
}
