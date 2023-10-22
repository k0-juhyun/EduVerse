using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class User
{
    [SerializeField]
    private string name;

    [SerializeField]
    private bool isTeacher;

    public User(string name, bool isTeacher)
    {
        this.name = name;
        this.isTeacher = isTeacher;
    }
}

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance = null;

    [SerializeField]
    private User myInfo;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void SetMyInfo(User user)
    {
        myInfo = user;
        //NetworkManager.instance.JoinRoom();
    }
}
