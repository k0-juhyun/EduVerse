using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class SaveData
{
    public SaveData(string _question, string _answer, string commentary)
    {
        question = _question;
        answer = _answer;
        this.Commentary = commentary;
    }

    public string question;
    public string answer;
    public string Commentary;
}

[System.Serializable]
public class QuizTitle
{
    public List<string> title;

    public void AddTitle(string newTitle)
    {
        if (title == null)
        {
            title = new List<string>();
        }

        title.Add(newTitle);
    }
}

public static class SaveSystem
{
    private static string SavePath => Application.persistentDataPath + "/Quiz/";

    // 파일 이름으로 json 데이터 읽어오기.
    public static void Save(SaveData saveData, string saveFileName)
    {
        // 저장폴더가 없으면.
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }

        string saveJson = JsonUtility.ToJson(saveData);

        string saveFilePath = SavePath + saveFileName + ".json";
        File.WriteAllText(saveFilePath, saveJson);
        Debug.Log("Save Success: " + saveFilePath);

        // Title에 Json 제목 저장.

    }

    // json데이터 파일 이름으로 읽어오는것
    public static SaveData Load(string saveFileName)
    {
        string saveFilePath = SavePath + saveFileName + ".json";
        string saveFileTitlePath = SavePath + "MyQuizTitleData" + ".json";
        if (!File.Exists(saveFilePath))
        {
            Debug.LogError("No such saveFile exists");
            return null;
        }

        string saveFile = File.ReadAllText(saveFilePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(saveFile);
        return saveData;
    }

    // 파일 이름 저장.
    public static void SaveFileName(string fileName, string saveFileName)
    {
        string saveFilePath = SavePath + fileName + ".json";
        File.WriteAllText(saveFilePath, saveFileName);
        Debug.Log("Save Success: " + saveFilePath);
    }

    // 타이틀 추가
    public static void AppendTitleToJson(string fileName, string newTitle)
    {
        string saveFilePath = SavePath + fileName + ".json";

        QuizTitle quizTitle;

        if (File.Exists(saveFilePath))
        {
            string saveFile = File.ReadAllText(saveFilePath);
            quizTitle = JsonUtility.FromJson<QuizTitle>(saveFile);
        }
        else
        {
            quizTitle = new QuizTitle();
        }

        quizTitle.AddTitle(newTitle);

        string saveJson = JsonUtility.ToJson(quizTitle);
        File.WriteAllText(saveFilePath, saveJson);
        Debug.Log("Title Append Success: " + saveFilePath);
    }

    // MyQuizTitleData에서 데이터 가져와서 추가.
    public static List<string> GetTitlesFromJson(string fileName)
    {
        string saveFilePath = SavePath + "MyQuizTitleData.json";

        if (!File.Exists(saveFilePath))
        {
            Debug.LogError("No such saveFile exists");
            return null;
        }

        string saveFile = File.ReadAllText(saveFilePath);
        QuizTitle quizTitle = JsonUtility.FromJson<QuizTitle>(saveFile);

        if (quizTitle != null)
        {
            return quizTitle.title;
        }

        return null;


        List<string> titles = SaveSystem.GetTitlesFromJson("MyQuizTitleData.json");

        if (titles != null)
        {
            foreach (string title in titles)
            {
                Debug.Log("Title: " + title);
            }
        }
    }
}

public class QuizToJson : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            List<string> titles = SaveSystem.GetTitlesFromJson("MyQuizTitleData.json");

            if (titles != null)
            {
                foreach (string title in titles)
                {
                    Debug.Log("Title: " + title);

                   SaveData saveData = SaveSystem.Load(title);
                   Debug.Log(saveData.question);
                }
            }
        }
    }


}
