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
    public string partName; // �� ���� �̸�
    public string objName;
    public Mesh[] partList;
    public int meshIndex; // �� ������ �޽� �ε���
}

[System.Serializable]
public class FriendInfo
{
    public List<CharacterInfo> data;
}

// ���� -> ���� / Ȯ��
public class GameManager : MonoBehaviourPun
{
    public static GameManager Instance;

    [Header("ĳ����")]
    public GameObject Character;
    private Customization customization;

    [Space]
    [Header("��ư")]
    public Button Btn_Save;

    // ���� ����
    public CharacterInfo myInfo;

    // ���� ������ ������ ������ �ִ� ����Ʈ
    public List<CharacterInfo> friendList = new List<CharacterInfo>();

    // ���� ����Ʈ�� key ���� ����� �ֱ� ���� ����ü
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
            // ������ ������ ����
            friendList.Clear();

            foreach (var part in customization.customParts)
            {
                myInfo = new CharacterInfo();

                myInfo.partName = part.partName; // ���� �̸� ����
                myInfo.objName = part.objName;
                myInfo.partList = part.partList;
                myInfo.meshIndex = part.currentIdx; // �޽� �ε��� ����

                friendList.Add(myInfo);
            }

            info.data = friendList;

            string jsonData = JsonUtility.ToJson(info, true);
            print(jsonData);

            // jsonData�� ���Ϸ� ����
            FileStream file = new FileStream(Application.dataPath + "/myInfo.txt", FileMode.Create);

            // json string �����͸� byte �迭�� �����.
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            // byteData�� file�� ����
            file.Write(byteData, 0, byteData.Length);

            file.Close();
        }
    }
}
