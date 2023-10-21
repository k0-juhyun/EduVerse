using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class LoadData : MonoBehaviour
{
    #region 파츠클래스
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

    public CustomPart[] customParts;

    //private void Start()
    //{
    //    LoadCharacterInfo();
    //}

    //private void LoadCharacterInfo()
    //{
    //    string filePath = Application.dataPath + "/myInfo.txt";
    //    if (File.Exists(filePath))
    //    {
    //        byte[] byteData = File.ReadAllBytes(filePath);
    //        string jsonData = Encoding.UTF8.GetString(byteData);
    //        GameManager.FriendInfo loadedInfo = new GameManager.FriendInfo();
    //        loadedInfo.data = JsonUtility.FromJson<List<GameManager.CharacterInfo>>(jsonData);

    //        foreach (var savedInfo in loadedInfo.data)
    //        {
    //            foreach (var part in customParts)
    //            {
    //                if (part.objName == savedInfo.objName)
    //                {
    //                    part.currentIdx = savedInfo.meshIndex;
    //                    part.customRenderer.sharedMesh = part.partList[part.currentIdx];
    //                    Debug.Log("1");
    //                }
    //                Debug.Log("2");
    //            }
    //            Debug.Log("3");
    //        }
    //        Debug.Log("4");
    //    }
    //    else
    //    {
    //        Debug.Log("No saved character info available.");
    //    }
    //}
    #endregion

    public CharacterInfo myInfo;

    private void Start()
    {
        LoadCharacterInfo();
    }
    private void LoadCharacterInfo()
    {
        // myInfo.txt를 읽어오고
        FileStream file = new FileStream(Application.dataPath + "/myInfo.txt", FileMode.Open);

        // file의 크기만큼 byte 배열을 할당한다
        byte[] byteData = new byte[file.Length];

        // byteData 에 file의 내용을 읽어온다.
        file.Read(byteData, 0 ,byteData.Length);

        // 파일을 닫아주자
        file.Close();

        // byteData를 문자열로 바꾸자
        string jsonData = Encoding.UTF8.GetString(byteData);

        // 문자열로 되어있는 jsonData를 myInfo에 parsing한다.

        myInfo = JsonUtility.FromJson<CharacterInfo>(jsonData);

        print(jsonData);
    }
}
