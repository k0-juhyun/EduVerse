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
    public int securitynumber;
    public int grade;
    public int classNum;
    public int studentNum;
    public string nickname;
    public string password;
    public string school;
    public string email;
    public bool isteacher;
    public int[] clothindex;

    public UserInfo()
    {

    }

    // �������� ��ȣx
    public UserInfo(string name, bool isTeacher, int grade, int classNum, string email, string password)
    {
        this.name = name;
        this.isteacher = isTeacher;
        this.grade = grade;
        this.classNum = classNum;
        this.email = email;
        this.password = password;
    }

    public UserInfo(string name, bool isTeacher, int studentBirth, string school, int grade, int classNum, int studentNum, string email, string password)
    {
        this.name = name;
        this.isteacher = isTeacher;
        securitynumber = studentBirth;
        this.school = school;
        this.grade = grade;
        this.classNum = classNum;
        this.studentNum = studentNum;
        this.email = email;
        this.password = password;
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

            // ȸ������ �� �ٷ� �α��� X  -> �α��ξ����� ��
        }
        else
        {
            print("���� ���� ���� ���� : " + task.Exception);
            if (onFail != null) onFail(task.Exception.Message);
        }

        //FireAuth.instance.LogOut();
        //NetworkManager.instance.LoadScene(0);
    }

    public void LoadUserInfo(System.Action callback = null)
    {
        StartCoroutine(ILoadUserInfo(callback));
    }

    IEnumerator ILoadUserInfo(System.Action callback = null)
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
            if (callback != null) callback();
        }
        else
        {
            print("���� ���� �������� ���� : " + task.Exception);
            if (onFail != null) onFail(task.Exception.Message);
        }
    }
}