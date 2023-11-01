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

[System.Serializable]
public class User
{
    public string name;
    public bool isTeacher;
    public int stuNum;

    public User(string name, bool isTeacher)
    {
        this.name = name;
        this.isTeacher = isTeacher;
    }
}

public class DataBase : MonoBehaviour
{
    public User userInfo;
    public static DataBase instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

    public void SetMyInfo(User user)
    {
        userInfo = user;
    }

    public void SetStudent(User user,int stuNum)
    {
        user.stuNum = stuNum;
    }
}
