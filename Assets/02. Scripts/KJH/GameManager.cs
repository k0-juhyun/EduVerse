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

    [Space]
    [Header("��ư")]
    public Button Btn_Save;

    // ���� Ŀ���� ����
    //public CharacterInfo myInfo;

    // Ŀ���� ���� ����Ʈ
    //public List<CharacterInfo> friendList = new List<CharacterInfo>();

    //FriendInfo info = new FriendInfo();

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(this.gameObject);

        if (Character != null)
            customization = Character.GetComponent<Customization>();

        Btn_Save.onClick.AddListener(() => SaveCharacterInfo());
    }

    private void SaveCharacterInfo()
    {
        if (photonView.IsMine)
        {
            DataBase.instance.friendList.Clear();

            DataBase.instance.friendList.Add(DataBase.instance.myInfo);

            DataBase.instance.info.data = DataBase.instance.friendList;

            // myInfo �� json ���·�
            string jsonData = JsonUtility.ToJson(DataBase.instance.info, true);
            print(jsonData);

            // json Data�� ���Ϸ� ����
            FileStream file = new FileStream(Application.streamingAssetsPath + "/myInfo.txt", FileMode.Create);

            // json string �����͸� byte �迭�� �����.
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            file.Write(byteData, 0, byteData.Length);

            file.Close();
        }
    }
}
