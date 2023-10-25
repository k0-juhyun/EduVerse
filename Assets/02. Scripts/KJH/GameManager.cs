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

    [Header("캐릭터")]
    public GameObject Character;
    private Customization customization;

    [Space]
    [Header("버튼")]
    public Button Btn_Save;

    // 나의 커스텀 정보
    //public CharacterInfo myInfo;

    // 커스텀 정보 리스트
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

            // myInfo 를 json 형태로
            string jsonData = JsonUtility.ToJson(DataBase.instance.info, true);
            print(jsonData);

            // json Data를 파일로 저장
            FileStream file = new FileStream(Application.streamingAssetsPath + "/myInfo.txt", FileMode.Create);

            // json string 데이터를 byte 배열로 만든다.
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            file.Write(byteData, 0, byteData.Length);

            file.Close();
        }
    }
}
