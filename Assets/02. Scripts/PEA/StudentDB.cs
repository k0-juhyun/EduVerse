using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;

public class StudentDB : MonoBehaviour
{
    public static StudentDB instance = null;

    private FirebaseDatabase database;

    public Transform content;
    public GameObject studentDataItem;
    public GameObject studentsDB;
    public GameObject personalDB;
    public Button backBtn;

    private void Awake()
    {
        if(instance == null)
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
        backBtn.onClick.AddListener(OnClickBackBtn);
        database = FirebaseDatabase.DefaultInstance;
        GetUserDB();
    }

    void Update()
    {
        
    }

    private void GetUserDB()
    {
        StartCoroutine(IGetUserDB());
    }

    IEnumerator IGetUserDB()
    {
        // 학생들 번호순서대로 정렬
        var task = database.GetReference("USER_INFO").OrderByChild("/studentNum").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            var a = task.Result.GetValue(false);
            DataSnapshot dataSnapshot = task.Result;

           // var myClassNum = dataSnapshot.Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId.ToString()).Child("/classNum").Value;

            foreach (var data in dataSnapshot.Children)
            {
                if (!((bool)data.Child("/isteacher").Value))
                {
                    GameObject studentItem = Instantiate(studentDataItem, content);
                    studentItem.GetComponent<StudentDataItem>().SetStdentData(data.Child("/name").Value.ToString(), data.Child("/studentNum").Value.ToString());
                }
            }

            print("유저 정보 가져오기 성공");
        }
        else
        {
            print("유저 정보 가져오기 실패 : " + task.Exception);
        }
    }

    public void ShowPersonalDB()
    {
        personalDB.SetActive(true);
        studentsDB.SetActive(false);
    }

    public void OnClickBackBtn()
    {
        personalDB.SetActive(false);
        studentsDB.SetActive(true);
    }
}
