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
    public List<CharacterInfo> myInfo = new List<CharacterInfo>();

    // ���� ������ ������ ������ �ִ� ����Ʈ
    //public List<CharacterInfo> friendList = new List<CharacterInfo>();

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
            myInfo.Clear();

            foreach (var part in customization.customParts)
            {
                CharacterInfo partInfo = new CharacterInfo();

                partInfo.partName = part.partName; // ���� �̸� ����
                partInfo.objName = part.objName;
                partInfo.partList = part.partList;
                partInfo.meshIndex = part.currentIdx; // �޽� �ε��� ����

                myInfo.Add(partInfo);
            }

            info.data = myInfo;

            string jsonData = JsonUtility.ToJson(info, true);

            // jsonData�� ���Ϸ� ����
            FileStream file = new FileStream(Application.streamingAssetsPath + "/myInfo.txt", FileMode.Create);

            print(jsonData);
            // json string �����͸� byte �迭�� �����.
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            // byteData�� file�� ����
            file.Write(byteData, 0, byteData.Length);

            file.Close();
        }
    }
}
