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
    public List<int> meshIndex; // �� ������ �޽� �ε���
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
        // "Resources/3D_Models" ���� ���� ��� GameObject �������� �ҷ��ɴϴ�.
        GameObject[] prefabs = Resources.LoadAll<GameObject>("3D_Models/ModelDatas");

        // model.spawnPrefab ����Ʈ�� �ʱ�ȭ�մϴ�.
        model.spawnPrefab = new List<GameObject>(prefabs.Length);

        // �ҷ��� �����յ��� ����Ʈ�� �߰��մϴ�.
        foreach (var prefab in prefabs)
        {
            if (prefab.GetComponent<PhotonView>() == null) // �̹� PhotonView�� �پ��ִ��� Ȯ��
            {
                PhotonView view = prefab.AddComponent<PhotonView>();
            }
            model.spawnPrefab.Add(prefab);
        }
    }
}
