using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System.Collections;

public class DataManager : MonoBehaviourPunCallbacks
{
    [Header("캐릭터")]
    public GameObject Character;

    public List<GameObject> partObj = new List<GameObject>();
    public List<int> partNum = new List<int>();

    private void Start()
    {
        LoadCharacterInfo();
    }


    private void LoadCharacterInfo()
    {
        if (!PhotonNetwork.IsConnected)
        {
            ApplyCharacterInfo(DataBase.instance.myInfo);
            return;
        }

        if (photonView.IsMine)
        {
#if UNITY_ANDROID
        string filePath =Application.persistentDataPath +"/"+ PhotonNetwork.NickName + ".txt";
#else 
        string filePath = "C:\\CharacterData\\" + PhotonNetwork.NickName + ".txt";
#endif

            if (File.Exists(filePath))
            {
                string jsonData = LoadJsonData(filePath);
                SavedCustomData loadedData = JsonUtility.FromJson<SavedCustomData>(jsonData);

                if (loadedData != null && loadedData.myData != null && loadedData.myData.Count > 0)
                {
                    DataBase.instance.myInfo = loadedData.myData[0];
                    partNum = DataBase.instance.myInfo.meshIndex;

                    ApplyCharacterInfo(DataBase.instance.myInfo);

                    // 자신의 메쉬 정보를 마스터 클라이언트에게 전송
                    photonView.RPC(nameof(SendMeshInfoToMaster), RpcTarget.OthersBuffered, photonView.OwnerActorNr, partNum.ToArray());
                }
            }
        }
    }

    [PunRPC]
    private void SendMeshInfoToMaster(int actorNumber, int[] receivedPartNum)
    {
        CharacterInfo charInfo = new CharacterInfo { meshIndex = new List<int>(receivedPartNum) };
        ApplyCharacterInfo(charInfo);
        return;
    }

    private string LoadJsonData(string filePath)
    {
        using (StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8))
        {
            return streamReader.ReadToEnd();
        }
    }

    private void ApplyCharacterInfo(CharacterInfo charInfo)
    {
        for (int i = 0; i < partObj.Count && i < charInfo.meshIndex.Count; i++)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = partObj[i].GetComponent<SkinnedMeshRenderer>();
            skinnedMeshRenderer.sharedMesh = DataBase.instance.db[i].partListArray[charInfo.meshIndex[i]];
        }
    }

    [PunRPC]
    private void ApplyCharacterInfoRPC(int actorNumber, string charInfoJson)
    {
        CharacterInfo charInfo = JsonUtility.FromJson<CharacterInfo>(charInfoJson);
        if (photonView.OwnerActorNr == actorNumber)
        {
            ApplyCharacterInfo(charInfo);
        }
    }
}
