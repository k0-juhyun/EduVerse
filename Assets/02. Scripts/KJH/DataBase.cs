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

[System.Serializable]
public class Model
{
    public List<GameObject> spawnPrefab;
    public List<Sprite> spawnSprite;
}

public class DataBase : MonoBehaviour
{
    public static DataBase instance;
    public User userInfo;
    public Model model;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadModelsFromResources();
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

    public void SetStudent(User user, int stuNum)
    {
        user.stuNum = stuNum;
    }

    private void LoadModelsFromResources()
    {
        // "Resources/3D_Models" 폴더 내의 모든 GameObject 프리팹을 불러옵니다.
        GameObject[] prefabs = Resources.LoadAll<GameObject>("3D_Models/ModelDatas");

        // model.spawnPrefab 리스트를 초기화합니다.
        model.spawnPrefab = new List<GameObject>(prefabs.Length);

        // 불러온 프리팹들을 리스트에 추가합니다.
        foreach (var prefab in prefabs)
        {
            if (prefab.GetComponent<PhotonView>() == null) // 이미 PhotonView가 붙어있는지 확인
            {
                PhotonView view = prefab.AddComponent<PhotonView>();
            }
            model.spawnPrefab.Add(prefab);
        }
    }
}
