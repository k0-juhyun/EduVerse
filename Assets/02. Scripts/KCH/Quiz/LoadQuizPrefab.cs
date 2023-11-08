using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadQuizPrefab : MonoBehaviour
{

    
    public Text question;

    // O/X 체크
    public GameObject O_object;
    public GameObject X_object;

    [Space (20)]
    // 담기
    public GameObject CheckOn;
    public GameObject CheckOff;

    bool save = false;



    public void Question_Answer(string title, string answer)
    {
        question.text = title;
        if (answer=="O") O_object.SetActive(true);
        else X_object.SetActive(true);
    }

    public void SaveQuestion()
    {
        if (CheckOff.activeSelf)
        {
            CheckOn.SetActive(true);
            CheckOff.SetActive(false);
        }
        else if (!CheckOff.activeSelf)
        {
            CheckOn.SetActive(false);
            CheckOff.SetActive(true);
        }
    }

}


