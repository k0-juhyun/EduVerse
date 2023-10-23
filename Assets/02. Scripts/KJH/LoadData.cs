using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine.XR;


public class LoadData : MonoBehaviourPun
{
    [Header("캐릭터")]
    public GameObject Character;

    public List<GameObject> partObj;
    private void Start()
    {
        DataBase.instance.characterData = new CharacterData();

        for (int i = 0; i < GameManager.Instance.myInfo.meshObjName.Count; i++)
        {
            GameObject currentObject = i < 3
                ? Character.transform.GetChild(i).GetChild(0).gameObject
                : Character.transform.Find(GameManager.Instance.myInfo.meshObjName[i])?.gameObject;

            if (currentObject != null)
            {
                partObj.Add(currentObject);
            }
        }

        if (photonView.IsMine)
        {
            LoadCharacterInfo();
        }
    }

    private void LoadCharacterInfo()
    {
        string filePath = Application.streamingAssetsPath + "/myInfo.txt";

        if (File.Exists(filePath))
        {
            string jsonData;

            using (StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8)) // UTF-8 형식으로 명시적 지정
            {
                jsonData = streamReader.ReadToEnd();
            }

            FriendInfo loadedData = JsonUtility.FromJson<FriendInfo>(jsonData); // 역직렬화 구조 수정

            DataBase.instance.characterData.myInfo = loadedData.data;

            Debug.Log("Data loaded: 박은아 공주" + jsonData);
        }

        ApplyCharacterInfo();
    }

    private void ApplyCharacterInfo()
    {
        for (int i = 0; i < partObj.Count; i++)
        {
            print(i + "dd");
            SkinnedMeshRenderer skinnedMeshRenderer = partObj[i].GetComponent<SkinnedMeshRenderer>();
            skinnedMeshRenderer.sharedMesh = DataBase.instance.db[i].partListArray[DataBase.instance.characterData.myInfo[0].meshIndex[i]]; 
        }
    }

    [PunRPC]
    private void ApplyCharacterInfoRPC()
    {

    }
}
