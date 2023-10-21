using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class LoadData : MonoBehaviour
{
    #region ����Ŭ����
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
        // myInfo.txt�� �о����
        FileStream file = new FileStream(Application.dataPath + "/myInfo.txt", FileMode.Open);

        // file�� ũ�⸸ŭ byte �迭�� �Ҵ��Ѵ�
        byte[] byteData = new byte[file.Length];

        // byteData �� file�� ������ �о�´�.
        file.Read(byteData, 0 ,byteData.Length);

        // ������ �ݾ�����
        file.Close();

        // byteData�� ���ڿ��� �ٲ���
        string jsonData = Encoding.UTF8.GetString(byteData);

        // ���ڿ��� �Ǿ��ִ� jsonData�� myInfo�� parsing�Ѵ�.

        myInfo = JsonUtility.FromJson<CharacterInfo>(jsonData);

        print(jsonData);
    }
}
