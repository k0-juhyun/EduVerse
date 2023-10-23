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
public class CharacterData
{
    public List<CharacterInfo> myInfo;
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
    public CharacterData characterData;
}
