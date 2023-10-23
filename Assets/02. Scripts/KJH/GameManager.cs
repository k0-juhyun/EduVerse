using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


// 접속 -> 교사 / 확인
[System.Serializable]
public class CharacterInfo
{
    public List<string> meshObjName;
    public List<int> meshIndex; // 각 부위의 메시 인덱스
}

[System.Serializable]
public class FriendInfo
{
    public List<CharacterInfo> data;
}

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
    public CharacterInfo myInfo;

    // 커스텀 정보 리스트
    public List<CharacterInfo> friendList = new List<CharacterInfo>();

    FriendInfo info = new FriendInfo();

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
            friendList.Add(myInfo);

            info.data = friendList;

            // myInfo 를 json 형태로
            string jsonData = JsonUtility.ToJson(info, true);
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
