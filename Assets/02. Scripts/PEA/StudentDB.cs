using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;

public class StudentDB : MonoBehaviour
{
    private Dictionary<string, UserInfo> studentsDictionary;

    private FirebaseDatabase database;

    public Transform content;
    public GameObject studentDataItem;

    void Start()
    {
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
        var task = database.GetReference("USER_INFO").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            var a = task.Result.GetValue(false);
            DataSnapshot dataSnapshot = task.Result;

            var myClassNum = dataSnapshot.Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId.ToString()).Child("/classNum").Value;

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
}
