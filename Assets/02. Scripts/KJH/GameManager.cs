using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Firebase.Auth;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("캐릭터")]
    private GameObject Character;
    private Customization customization;

    public Queue<string> makeObjectTags = new Queue<string>();

    public FirebaseAuth auth;

    PhotonView Mypv;

    private void Awake()
    {
        if (Instance == null)
        {
            Screen.SetResolution(1280, 800, false);
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            GameManager[] gameManagers = FindObjectsOfType<GameManager>();
            if (gameManagers.Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            Character = GameObject.Find("Character");
            if (Character != null)
                customization = Character.GetComponent<Customization>();

            Mypv = GetComponent<PhotonView>();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        //print("현재 연결 상태: "+PhotonNetwork.NetworkClientState); 

    }
    public void SaveCharacterInfo()
    {
        //if (photonView.IsMine)
        //{
            DataBase.instance.savedData.myData.Clear();
            DataBase.instance.savedData.myData.Add(DataBase.instance.myInfo);

            SaveToJson(DataBase.instance.savedData, "/myInfo.txt");
        //}
    }

    private void SaveToJson(object obj, string filePath)
    {
        string jsonData = JsonUtility.ToJson(obj, true);
        byte[] byteData = Encoding.UTF8.GetBytes(jsonData);
        string path = "";
#if UNITY_ANDROID
        path = Application.persistentDataPath +"/"+ PhotonNetwork.NickName + ".txt";
#elif UNITY_EDITOR
        path = "C:\\CharacterData\\" + PhotonNetwork.NickName + ".txt";
#else
        path = "C:\\CharacterData\\" + PhotonNetwork.NickName + ".txt";
#endif
        string directoryPath = Path.GetDirectoryName(path);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        using (FileStream file = new FileStream(path, FileMode.Create))
        {
            file.Write(byteData, 0, byteData.Length);
        }
    }

    public void AddWaitMakeObjectTag(string tag)
    {
        makeObjectTags.Enqueue(tag);
    }

    public string GetMakeObejctTag()
    {
        return makeObjectTags.Dequeue();
    }

    // 로그아웃 photonview가 달려있는 오브젝트들은 씬으로 
    public void SingOut()
    {
        Debug.Log(auth.CurrentUser);
        auth.SignOut();
        Debug.Log(auth.CurrentUser);
        PhotonNetwork.LoadLevel(0);
        Destroy(gameObject);
        Destroy(MyQuizStorage.Instance.gameObject);
    }
}
