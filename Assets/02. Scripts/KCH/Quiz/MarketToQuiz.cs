using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MarketToQuiz : MonoBehaviour
{
    public Text[] text;

    // 여기서 가져갈 퀴즈를 고름
    // 퀴즈 선택
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

                    // 앞에 단원 삭제
                    text[0].text = title.Substring(4);
                    Debug.Log(saveData.question); 
                    string a = saveData.question.Substring(4);
                }
            }
        }
    }

}

