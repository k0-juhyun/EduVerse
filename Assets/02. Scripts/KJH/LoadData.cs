using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class LoadData : MonoBehaviour
{
    public GameObject Character;

    [System.Serializable]
    public class CustomPart
    {
        public string partName;
        public GameObject partObj;
        public string objName;
        public Mesh[] partList;
        public int currentIdx;
        public SkinnedMeshRenderer customRenderer;
    }

    [Space]
    public List<CustomPart> customParts = new List<CustomPart>(); // 리스트로 변경

    [Space]
    public CharacterInfo[] myInfo = new CharacterInfo[0]; // 배열 초기화

    private void Start()
    {
        LoadCharacterInfo();
        ApplyCustomData();
    }

    private void LoadCharacterInfo()
    {
        // myInfo.txt를 읽어옴
        string filePath = Application.dataPath + "/myInfo.txt";

        if (File.Exists(filePath))
        {
            string jsonData;

            using (StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8)) // UTF-8 형식으로 명시적 지정
            {
                jsonData = streamReader.ReadToEnd();
            }

            FriendInfo loadedData = JsonUtility.FromJson<FriendInfo>(jsonData); // 역직렬화 구조 수정

            myInfo = loadedData.data.ToArray();

            Debug.Log("Data loaded: " + jsonData);
        }
        else
        {
            Debug.Log("File not found at path: " + filePath);
        }
    }

    private void ApplyCustomData()
    {
        customParts.Clear(); // 기존 요소를 지움

        for (int i = 0; i < myInfo.Length; i++)
        {
            CustomPart customPart = new CustomPart(); // 새 CustomPart 인스턴스 생성

            customPart.partName = myInfo[i].partName;
            customPart.objName = myInfo[i].objName;
            customPart.partList = myInfo[i].partList;
            customPart.currentIdx = myInfo[i].meshIndex;

            Transform characterChild = Character.transform.Find(myInfo[i].objName);

            if (characterChild != null)
            {
                customPart.partObj = characterChild.gameObject;
                customPart.customRenderer = characterChild.gameObject.GetComponent<SkinnedMeshRenderer>();
                customPart.customRenderer.sharedMesh = customPart.partList[customPart.currentIdx];
            }

            customParts.Add(customPart); // 리스트에 추가
        }
    }
}
