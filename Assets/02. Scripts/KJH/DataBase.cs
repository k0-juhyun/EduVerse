using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class CustomPartDB
{
    public string partsName;
    public Mesh[] partListArray;
}

[System.Serializable]
public class SavedCustomData
{
    public List<CharacterInfo> myData;
}

[System.Serializable]
public class CharacterInfo
{
    public List<string> meshObjName;
    public List<int> meshIndex; // 각 부위의 메시 인덱스
}

[System.Serializable]
public class UserDataList
{
    public PhotonView userPhotonView;
    public SavedCustomData userCustomData;
}


public class DataBase : MonoBehaviour
{
    public static DataBase instance;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
    }

    public CustomPartDB[] db;
    public CharacterInfo myInfo;
    public SavedCustomData savedData = new SavedCustomData();
    public List<UserDataList> userDataList = new List<UserDataList>();

    public void AddUserData(PhotonView view, SavedCustomData customData)
    {
        UserDataList newUser = new UserDataList { userPhotonView = view, userCustomData = customData };
        userDataList.Add(newUser);
    }
}
