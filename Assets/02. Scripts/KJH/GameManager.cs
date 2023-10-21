using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("ĳ����")]
    public GameObject Character;
    private Customization customization;

    [Space]
    [Header("��ư")]
    public Button Btn_Save;


    [System.Serializable]
    public class CharacterInfo
    {
        public string partName; // �� ���� �̸�
        public string objName;
        public Mesh[] partList;
        public int meshIndex; // �� ������ �޽� �ε���
    }

    // ������
    public CharacterInfo characterInfo;

    // ���� ����
    public List<CharacterInfo> savedCharacterInfoList = new List<CharacterInfo>();
    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(this.gameObject);

        if (Character != null)
            customization = Character.GetComponent<Customization>();

        Btn_Save.onClick.AddListener(() => SaveCharacterInfo());
    }

    private void SaveCharacterInfo()
    {
        // ������ ������ ����
        List<CharacterInfo> savedInfoList = new List<CharacterInfo>();

        savedCharacterInfoList.Clear();

        foreach (var part in customization.customParts)
        {
            CharacterInfo savedInfo = new CharacterInfo();
            savedInfo.partName = part.partName; // ���� �̸� ����
            savedInfo.objName = part.objName;
            savedInfo.partList = part.partList;
            savedInfo.meshIndex = part.currentIdx; // �޽� �ε��� ����
            savedInfoList.Add(savedInfo);
        }

        // JSON ���Ϸ� ����
        string json = JsonUtility.ToJson(savedInfoList);
        File.WriteAllText(Application.persistentDataPath + "/characterInfo.json", json);

        // ����� ������ ����Ʈ�� �߰�
        savedCharacterInfoList.AddRange(savedInfoList);
    }
}
