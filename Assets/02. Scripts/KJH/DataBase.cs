using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class CustomPartDB
{
    public string partsName;
    public Mesh[] partListArray;
}

[System.Serializable]
public class SavedCustomData
{
    public List<CharacterInfo> myData;
}

[System.Serializable]
public class FriendInfo
{
    public List<CharacterInfo> data;
}
// ���� -> ���� / Ȯ��
[System.Serializable]
public class CharacterInfo
{
    public List<string> meshObjName;
    public List<int> meshIndex; // �� ������ �޽� �ε���
}


public class DataBase : MonoBehaviour
{

    public static DataBase instance;
    private void Awake()
    {
        DontDestroyOnLoad(this);

        instance = this;
    }

    public CustomPartDB[] db;

    public SavedCustomData savedCustomData;

    public CharacterInfo myInfo;

    public List<CharacterInfo> friendList = new List<CharacterInfo>();
    
    public FriendInfo info = new FriendInfo();

}
