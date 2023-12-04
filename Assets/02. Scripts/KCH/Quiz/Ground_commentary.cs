using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ground_commentary : MonoBehaviour
{

    // ��ư�� ������ �� �ؼ��� ������.

    public GameObject CommentaryPanel;

    public string Question_ { get; set; }
    public string Answer_ { get; set; }
    public string Commentary_ { get; set; }

    [HideInInspector]
    public bool CommentatyPanelCheck=false;

    public void OnCommentaryBtnClick()
    {
        GameObject commentaryPanel_ = Instantiate(CommentaryPanel, transform);
        commentaryPanel_.GetComponent<ClassroomCommentary>().PutAnswer_Commentary(Answer_, Commentary_);

        CommentatyPanelCheck = true;
    }

}
