using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;

public class LoadData : MonoBehaviourPun
{
    [Header("캐릭터")]
    public GameObject Character;

    public List<GameObject> partObj;
    private void Start()
    {
        DataBase.instance.savedCustomData = new SavedCustomData();

        for (int i = 0; i < DataBase.instance.myInfo.meshObjName.Count; i++)
        {
            GameObject currentObject = i < 3
                ? Character.transform.GetChild(i).GetChild(0).gameObject
                : Character.transform.Find(DataBase.instance.myInfo.meshObjName[i])?.gameObject;

            if (currentObject != null)
            {
                partObj.Add(currentObject);
            }
        }
        
        LoadCharacterInfo();
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

            DataBase.instance.savedCustomData.myData = loadedData.data;

            Debug.Log(SceneManager.GetActiveScene().name +"데이터 불러옴" + jsonData);
        }

        ApplyCharacterInfo();

        if(photonView.IsMine)
        {
            photonView.RPC(nameof(ApplyCharacterInfoRPC),RpcTarget.All);
        }
    }

    private void ApplyCharacterInfo()
    {
        for (int i = 0; i < partObj.Count; i++)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = partObj[i].GetComponent<SkinnedMeshRenderer>();
            skinnedMeshRenderer.sharedMesh = DataBase.instance.db[i].partListArray[DataBase.instance.savedCustomData.myData[0].meshIndex[i]];
        }
    }

    [PunRPC]
    private void ApplyCharacterInfoRPC()
    {
        print("포톤");
        ApplyCharacterInfo();
    }
}