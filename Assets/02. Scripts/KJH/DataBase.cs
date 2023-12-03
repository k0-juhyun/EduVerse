using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.IO.Compression;

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
    public User user;
    public Model model;
    public UserInfo userInfo;

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

    public void SetMyInfo(User user, UserInfo userInfo)
    {
        this.user = user;
        this.userInfo = userInfo;

        //if (user.isTeacher)
        //{
        //    Voice.instance.ActiveMuteAllBtn();
        //}
    }

    public void SetStudent(User user, int stuNum)
    {
        user.stuNum = stuNum;
    }

    private void LoadModelsFromResources()
    {
        // "Resources/3D_Models" 폴더 내의 모든 GameObject 프리팹을 불러옵니다.
#if UNITY_ANDROID
        string path = Application.persistentDataPath + "/3D_Models/ModelDatas/";
        //string path = Application.streamingAssetsPath + "/";
#elif UNITY_EDITOR
        string path = Application.persistentDataPath + "/3D_Models/ModelDatas/";
        //string path = Application.streamingAssetsPath+"/";
#elif UNITY_STANDALONE
        //string path = Application.streamingAssetsPath+"/";
         string path = Application.persistentDataPath + "/3D_Models/ModelDatas/";
#endif

        print("알집 경로: " + path);
        try
        {
            string[] objFiles = Directory.GetFiles(path, "*.obj");

            print(objFiles.Length);

            model.spawnPrefab = new List<GameObject>(objFiles.Length);

            foreach (string objFile in objFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(objFile);
                GameObject tempObject = new GameObject(fileName);
                model.spawnPrefab.Add(tempObject);
                print("오브젝트파일: "+ tempObject.name);

                // 씬에 추가하는 방법
                tempObject.transform.SetParent(this.transform);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error loading zip files: " + ex.Message);
        }
    }
}