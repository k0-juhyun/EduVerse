using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Toggle_kch : MonoBehaviour
{
    public Toggle multimedio_;

    public GameObject[] multimedioButton;
    public GameObject multimedioTitle;
    public GameObject mulitmedioexplain;

    public GameObject QuizButton;
    public GameObject QuizTitle;
    public GameObject Quizexplain;

    private void OnEnable()
    {
        ToggleClick(true);
        // ffd96a
        multimedio_.isOn = true;
    }

    public void ToggleClick(bool isOn)
    {
        if (isOn)
        {
            for (int i = 0; i < multimedioButton.Length; i++)
            {
                multimedioButton[i].SetActive(true);
            }
            QuizButton.SetActive(false);
            QuizTitle.SetActive(false);
            Quizexplain.SetActive(false);

            multimedioTitle.SetActive(true);
            mulitmedioexplain.SetActive(true);
        }
        else
        {

            for (int i = 0; i < multimedioButton.Length; i++)
            {
                multimedioButton[i].SetActive(false);
            }
            QuizButton.SetActive(true);
            QuizTitle.SetActive(true);
            Quizexplain.SetActive(true);

            multimedioTitle.SetActive(false);
            mulitmedioexplain.SetActive(false);

        }
    }

}
