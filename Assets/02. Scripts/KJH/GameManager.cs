using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


// ���� -> ���� / Ȯ��
[System.Serializable]
public class CharacterInfo
{
    public List<string> meshObjName;
    public List<int> meshIndex; // �� ������ �޽� �ε���
}

[System.Serializable]
public class FriendInfo
{
    public List<CharacterInfo> data;
}

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
    public CharacterInfo myInfo;

    // Ŀ���� ���� ����Ʈ
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

            // myInfo �� json ���·�
            string jsonData = JsonUtility.ToJson(info, true);
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
