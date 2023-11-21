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
        Destroy(transform.root.gameObject);
    }
}
