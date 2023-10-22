using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[System.Serializable]
public class CharacterInfo
{
    public string partName; // 각 부위 이름
    public string objName;
    public Mesh[] partList;
    public int meshIndex; // 각 부위의 메시 인덱스
}

[System.Serializable]
public class FriendInfo
{
    public List<CharacterInfo> data;
}

// 접속 -> 교사 / 확인
public class GameManager : MonoBehaviourPun
{
    public static GameManager Instance;

    [Header("캐릭터")]
    public GameObject Character;
    private Customization customization;

    [Space]
    [Header("버튼")]
    public Button Btn_Save;

    // 나의 정보
    public CharacterInfo myInfo;

    // 유저 정보를 여러개 가지고 있는 리스트
    public List<CharacterInfo> friendList = new List<CharacterInfo>();

    // 유저 리스트의 key 값을 만들어 주기 위한 구조체
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
            // 저장할 데이터 구조
            friendList.Clear();

            foreach (var part in customization.customParts)
            {
                myInfo = new CharacterInfo();

                myInfo.partName = part.partName; // 부위 이름 저장
                myInfo.objName = part.objName;
                myInfo.partList = part.partList;
                myInfo.meshIndex = part.currentIdx; // 메시 인덱스 저장

                friendList.Add(myInfo);
            }

            info.data = friendList;

            string jsonData = JsonUtility.ToJson(info, true);
            print(jsonData);

            // jsonData를 파일로 저장
            FileStream file = new FileStream(Application.dataPath + "/myInfo.txt", FileMode.Create);

            // json string 데이터를 byte 배열로 만든다.
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            // byteData를 file에 쓰자
            file.Write(byteData, 0, byteData.Length);

            file.Close();
        }
    }
}
