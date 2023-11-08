using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MarketToQuiz : MonoBehaviour
{
    public Text[] text;

    // ���⼭ ������ ��� ��
    // ���� ����
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            List<string> titles = SaveSystem.GetTitlesFromJson("MyQuizTitleData.json");

            if (titles != null)
            {
                foreach (string title in titles)
                {
                    Debug.Log("Title: " + title);
                    SaveData saveData = SaveSystem.Load(title);

                    // �տ� �ܿ� ����
                    text[0].text = title.Substring(4);
                    Debug.Log(saveData.question); 
                    string a = saveData.question.Substring(4);
                }
            }
        }
    }

}

