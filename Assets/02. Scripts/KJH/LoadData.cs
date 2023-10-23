using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
using System.Text;
using Unity.VisualScripting;

public class LoadData : MonoBehaviourPun
{
    [Header("ĳ����")]
    public GameObject Character;

    [System.Serializable]
    public class CustomPart
    {
        public string partName;
        public GameObject partObj;
        public string objName;
        public Mesh[] partList;
        public int currentIdx;
        public SkinnedMeshRenderer customRenderer;
    }

    public CustomPart customPart;
    [Space]
    //[HideInInspector]
    public List<CustomPart> customParts = new List<CustomPart>(); // ����Ʈ�� ����

    [Space]
    public CharacterInfo[] myInfo = new CharacterInfo[0]; // �迭 �ʱ�ȭ

    private void Start()
    {
        if (photonView.IsMine)
        {
            LoadCharacterInfo();
        }
    }

    private void LoadCharacterInfo()
    {
        // myInfo.txt�� �о��
        string filePath = Application.streamingAssetsPath + "/myInfo.txt";

        if (File.Exists(filePath))
        {
            string jsonData;

            using (StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8)) // UTF-8 �������� ����� ����
            {
                jsonData = streamReader.ReadToEnd();
            }

            FriendInfo loadedData = JsonUtility.FromJson<FriendInfo>(jsonData); // ������ȭ ���� ����

            myInfo = loadedData.data.ToArray();

            Debug.Log("Data loaded: ������ ����" + jsonData);
        }
        else
        {
            Debug.Log("File not found at path: " + filePath);
        }

        ApplyCustomData();
    }

    private void ApplyCustomData()
    {
        customParts.Clear(); // ���� ��Ҹ� ����

        for (int i = 0; i < myInfo.Length; i++)
        {
            print("ApplyCustomData");

            customPart.partName = myInfo[i].partName;
            customPart.objName = myInfo[i].objName;
            customPart.partList = myInfo[i].partList;
            customPart.currentIdx = myInfo[i].meshIndex;

            Transform characterChild;

            characterChild = (i < 2 ? Character.transform.GetChild(i).GetChild(0) : Character.transform.Find(myInfo[i].objName));

            if (characterChild != null)
            {
                customPart.partObj = characterChild.gameObject;
                customPart.customRenderer = characterChild.gameObject.GetComponent<SkinnedMeshRenderer>();
                customPart.customRenderer.sharedMesh = customPart.partList[customPart.currentIdx];
                photonView.RPC("ApplyMesh", RpcTarget.Others ,customPart.currentIdx);
            }

            customParts.Add(customPart); // ����Ʈ�� �߰�
        }

    }

    [PunRPC]
    void ApplyMesh(int curIndex)
    {
        print("------ " + curIndex);
        customPart.customRenderer.sharedMesh = customPart.partList[curIndex];
    }
}

