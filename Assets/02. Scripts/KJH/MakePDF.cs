using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[System.Serializable]
public class TeachingData
{
    // ��ư�ϳ��� 

    public enum TeachingDataType
    {
        Quiz,
        Image,
        Gif,
        Vedio
    }
    // ��� �����
    public TeachingDataType dataType;
    public Button dataButton;
    public string dataName;
    public string dataPath;

    public string quizContents;
    public string quizAnswer;

    public Texture2D dataTexture2D;

    public byte[] dataByes;

    public Sprite[] dataSprites;

    // ����
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

    // ����
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
    public Dropdown dataTypeDropdown; // ������ Ÿ���� �����ϴ� ��Ӵٿ� �޴�

    // ��ư�� ���� �� ȣ��� �޼���
    public void OnClickCreateButton()
    {
        
    }

    // TeachingData�� ���� ��ư�� �����ϴ� �޼���
    void CreateButtonForTeachingData(TeachingData data)
    {
        GameObject buttonObject = new GameObject("TeachingDataButton");

        Button button = buttonObject.AddComponent<Button>();

        button.transform.SetParent(teachingDataStorage.transform);

        button.onClick.AddListener(() => DisplayTeachingData(data));
    }

    // TeachingData�� ǥ���ϴ� �޼���
    void DisplayTeachingData(TeachingData data)
    {
        // ������ Ÿ�Կ� ���� ������ ������ ǥ��
        switch (data.dataType)
        {
            case TeachingData.TeachingDataType.Quiz:
                // ���� ǥ�� ����
                break;
            case TeachingData.TeachingDataType.Image:
                // �̹��� ǥ�� ����
                break;
            case TeachingData.TeachingDataType.Gif:
                // GIF ǥ�� ����
                break;
            case TeachingData.TeachingDataType.Vedio:
                // ���� ǥ�� ����
                break;
        }
    }
}
