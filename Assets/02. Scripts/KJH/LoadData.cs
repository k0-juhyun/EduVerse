using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadData : MonoBehaviour
{
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

    private void Start()
    {
        LoadCharacterInfo();
    }

    private void LoadCharacterInfo()
    {
        string filePath = Application.persistentDataPath + "/characterInfo.json";
        if (File.Exists(filePath))
        {
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                string dataAsJson = streamReader.ReadToEnd();
                List<GameManager.CharacterInfo> savedInfoList = JsonUtility.FromJson<List<GameManager.CharacterInfo>>(dataAsJson);

                foreach (var savedInfo in savedInfoList)
                {
                    foreach (var part in customParts)
                    {
                        if (part.objName == savedInfo.objName)
                        {
                            part.currentIdx = savedInfo.meshIndex;
                            part.customRenderer.sharedMesh = part.partList[part.currentIdx];
                            Debug.Log("1");
                        }
                        Debug.Log("2");
                    }
                    Debug.Log("3");
                }
                Debug.Log("4");
            }
        }
        else
        {
            Debug.Log("No saved character info available.");
        }
    }
}
