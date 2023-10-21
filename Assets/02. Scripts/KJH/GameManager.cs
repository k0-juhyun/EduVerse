using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("캐릭터")]
    public GameObject Character;
    private Customization customization;

    [Space]
    [Header("버튼")]
    public Button Btn_Save;


    [System.Serializable]
    public class CharacterInfo
    {
        public string partName; // 각 부위 이름
        public string objName;
        public Mesh[] partList;
        public int meshIndex; // 각 부위의 메시 인덱스
    }

    // 내정보
    public CharacterInfo characterInfo;

    // 여러 정보
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
        // 저장할 데이터 구조
        List<CharacterInfo> savedInfoList = new List<CharacterInfo>();

        savedCharacterInfoList.Clear();

        foreach (var part in customization.customParts)
        {
            CharacterInfo savedInfo = new CharacterInfo();
            savedInfo.partName = part.partName; // 부위 이름 저장
            savedInfo.objName = part.objName;
            savedInfo.partList = part.partList;
            savedInfo.meshIndex = part.currentIdx; // 메시 인덱스 저장
            savedInfoList.Add(savedInfo);
        }

        // JSON 파일로 저장
        string json = JsonUtility.ToJson(savedInfoList);
        File.WriteAllText(Application.persistentDataPath + "/characterInfo.json", json);

        // 저장된 정보를 리스트에 추가
        savedCharacterInfoList.AddRange(savedInfoList);
    }
}
