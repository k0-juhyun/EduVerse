using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    public static GameManager Instance;

    [Header("ĳ����")]
    public GameObject Character;
    private Customization customization;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            if (Character != null)
                customization = Character.GetComponent<Customization>();
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void SaveCharacterInfo()
    {
        if (photonView.IsMine)
        {
            DataBase.instance.savedData.myData.Clear();
            DataBase.instance.savedData.myData.Add(DataBase.instance.myInfo);

            SaveToJson(DataBase.instance.savedData, "/myInfo.txt");
        }
    }

    private void SaveToJson(object obj, string filePath)
    {
        string jsonData = JsonUtility.ToJson(obj, true);
        byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

#if UNITY_EDITOR
        string path = "C:\\CharacterData\\" + PhotonNetwork.NickName + ".txt";
#elif UNITY_ANDROID
        string path = Application.persistentDataPath +"/"+PhotonNetwork.NickName +".txt";
#endif
        using (FileStream file = new FileStream(path, FileMode.Create))
        {
            file.Write(byteData, 0, byteData.Length);
        }
    }
}
