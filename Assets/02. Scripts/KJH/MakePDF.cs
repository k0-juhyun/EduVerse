using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[System.Serializable]
public class TeachingData
{
    // 버튼하나에 

    public enum TeachingDataType
    {
        Quiz,
        Image,
        Gif,
        Vedio
    }
    // 퀴즈를 담던지
    public TeachingDataType dataType;
    public Button dataButton;
    public string dataName;
    public string dataPath;

    public string quizContents;
    public string quizAnswer;

    public Texture2D dataTexture2D;

    public byte[] dataByes;

    public Sprite[] dataSprites;

    // 비디오
    public TeachingData(TeachingDataType dataType, Button dataButton ,string dataName, string dataPath ="")
    {
        this.dataType = dataType;
        this.dataButton = dataButton;
        this.dataName = dataName;
        this.dataPath = dataPath;
    }

    // GIF
    public TeachingData (TeachingDataType dataType, Button dataButton, string dataName, Texture2D dataTexture2D)
    {
        this.dataType = dataType;
        this.dataButton = dataButton;
        this.dataName = dataName;
        this.dataTexture2D = dataTexture2D;
    }

    // GIF
    public TeachingData (TeachingDataType dataType, Button dataButton, string dataName, Sprite[] dataSprites) 
    {
        this.dataType = dataType;
        this.dataButton = dataButton;
        this.dataName= dataName;
        this.dataSprites = dataSprites;
    }

    // 퀴즈
    public TeachingData (TeachingDataType dataType, Button dataButton, string dataName, string quizContents, string quisAnswer)
    {
        this.dataType = dataType;
        this.dataButton = dataButton;
        this.dataName = dataName;
        this.quizContents = quizContents;
        this.quizAnswer = quisAnswer;
    }
}

public class MakePDF : MonoBehaviour
{
    public List<TeachingData> teachingData;

    public GameObject teachingDataStorage;
    public Dropdown dataTypeDropdown; // 데이터 타입을 선택하는 드롭다운 메뉴

    // 버튼을 누를 때 호출될 메서드
    public void OnClickCreateButton()
    {
        
    }

    // TeachingData에 대한 버튼을 생성하는 메서드
    void CreateButtonForTeachingData(TeachingData data)
    {
        GameObject buttonObject = new GameObject("TeachingDataButton");

        Button button = buttonObject.AddComponent<Button>();

        button.transform.SetParent(teachingDataStorage.transform);

        button.onClick.AddListener(() => DisplayTeachingData(data));
    }

    // TeachingData를 표시하는 메서드
    void DisplayTeachingData(TeachingData data)
    {
        // 데이터 타입에 따라 적절한 콘텐츠 표시
        switch (data.dataType)
        {
            case TeachingData.TeachingDataType.Quiz:
                // 퀴즈 표시 로직
                break;
            case TeachingData.TeachingDataType.Image:
                // 이미지 표시 로직
                break;
            case TeachingData.TeachingDataType.Gif:
                // GIF 표시 로직
                break;
            case TeachingData.TeachingDataType.Vedio:
                // 비디오 표시 로직
                break;
        }
    }
}
