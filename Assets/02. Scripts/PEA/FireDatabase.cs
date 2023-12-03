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

    // 선생님은 번호x
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

        //내정보 임시 입력

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

        // GetReference() -> USER_INFO 데이터가 저장될 위치를 가르키는 경로
        // SetRawJsonValueAsync 비동기로 데이터 저장

        // 나중에는 빈 인풋필드 확인하고 실행시켜줘야함.

        //myInfo.name = InputName.text;
        //myInfo.securitynumber = (InputSecurityNumber.text);
        //myInfo.nickname = InputNickName.text;
        //myInfo.password = InputPassword.text;
        //myInfo.email = InputEmail.text;

        var task = database.GetReference(path).SetRawJsonValueAsync(JsonUtility.ToJson(userInfo));
        
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception == null)
        {
            print("유저 정보 저장 성공");
            if (onComplete != null) onComplete();

            // 회원가입 후 바로 로그인 X  -> 로그인씬으로 감
        }
        else
        {
            print("유저 정보 저장 실패 : " + task.Exception);
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

            print("유저 정보 가져오기 성공");
            if (callback != null) callback();
        }
        else
        {
            print("유저 정보 가져오기 실패 : " + task.Exception);
            if (onFail != null) onFail(task.Exception.Message);
        }
    }
}