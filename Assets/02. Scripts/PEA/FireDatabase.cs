using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;
using UnityEngine.UI;
using Firebase.Auth;

[System.Serializable]
public struct ItemInfo
{
    public int idx;
    public Vector3 postion;
    public Vector3 angle;
    public Vector3 scale;
}

[System.Serializable]
public class UserInfo
{
    public string name;
    public string securitynumber;
    public string nickname;
    public string password;
    public string email;
    public bool isteacher;
    public int[] clothindex;

    public UserInfo()
    {

    }

    public UserInfo(string name, bool isTeacher)
    {
        this.name = name;
        this.isteacher = isTeacher;
    }
}



public class FireDatabase : MonoBehaviour
{
    public static FireDatabase instance;

    FirebaseDatabase database;

    public UserInfo myInfo = new UserInfo();

    
    public Action onComplete;
    public Action<string> onFail;

    public InputField InputName;
    public InputField InputSecurityNumber;
    public InputField InputNickName;
    public InputField InputPassword;
    public InputField InputEmail;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        database = FirebaseDatabase.DefaultInstance;

        //������ �ӽ� �Է�

    }

    private void Update()
    {

    }

    public void SetItemInfo(List<GameObject> items)
    {
        //myInfo.items.Clear();
        //for (int i = 0; i < items.Count; i++)
        //{
        //    ItemInfo info = new ItemInfo();
        //    info.idx = items[i].GetComponent<Item>().idx;
        //    info.postion = items[i].transform.position;
        //    info.angle = items[i].transform.eulerAngles;
        //    info.scale = items[i].transform.localScale;
        //    myInfo.items.Add(info);
        //}
    }

    public void SaveUserInfo(UserInfo userInfo)
    {
        StartCoroutine(ISaveUserInfo(userInfo));
    }

    IEnumerator ISaveUserInfo(UserInfo userInfo)
    {
        string path = "USER_INFO/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        // GetReference() -> USER_INFO �����Ͱ� ����� ��ġ�� ����Ű�� ���
        // SetRawJsonValueAsync �񵿱�� ������ ����

        // ���߿��� �� ��ǲ�ʵ� Ȯ���ϰ� ������������.

        //myInfo.name = InputName.text;
        //myInfo.securitynumber = (InputSecurityNumber.text);
        //myInfo.nickname = InputNickName.text;
        //myInfo.password = InputPassword.text;
        //myInfo.email = InputEmail.text;

        var task = database.GetReference(path).SetRawJsonValueAsync(JsonUtility.ToJson(userInfo));
        
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception == null)
        {
            print("���� ���� ���� ����");
            if (onComplete != null) onComplete();
        }
        else
        {
            print("���� ���� ���� ���� : " + task.Exception);
            if (onFail != null) onFail(task.Exception.Message);
            
        }
    }


    public void LoadUserInfo()
    {
        StartCoroutine(ILoadUserInfo());
    }

    IEnumerator ILoadUserInfo()
    {

        string path = "USER_INFO/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        var task = database.GetReference(path).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            var a = task.Result.GetValue(false);
            print(a);

            myInfo = JsonUtility.FromJson<UserInfo>(task.Result.GetRawJsonValue());

            //print(myInfo.name);
            //print(myInfo.age);
            //print(myInfo.height);

            print("���� ���� �������� ����");
            if (onComplete != null) onComplete();
        }
        else
        {
            print("���� ���� �������� ���� : " + task.Exception);
            if (onFail != null) onFail(task.Exception.Message);
        }
    }
}