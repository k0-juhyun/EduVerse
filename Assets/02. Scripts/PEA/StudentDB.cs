using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;
using DG.Tweening;

public class StudentDB : MonoBehaviour
{
    public static StudentDB instance = null;

    private FirebaseDatabase database;

    public Transform content;
    public GameObject studentDataItem;
    public GameObject studentsDB;
    public GameObject personalDB;
    public GameObject AnalysisDB;
    public Button backStudentBtn;
    public Button backMyPageBtn;

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
        //backStudentBtn.onClick.AddListener(OnClicGoBackStudentBtn);
        backMyPageBtn.onClick.AddListener(OnClickGoBackMyPageBtn);
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
                    studentItem.GetComponent<StudentDataItem>().uid = data.Key;
                    studentItem.GetComponent<Student_QuizData>().StudentUIDSaveBtnClick(data.Key);
                }
            }

            print("유저 정보 가져오기 성공");
        }
        else
        {
            print("유저 정보 가져오기 실패 : " + task.Exception);
        }
    }

    public void OffStudentDB()
    {
        personalDB.SetActive(true);
    }


    public void ShowPersonalDB()
    {
        studentsDB.SetActive(false);
    }

    public void OnClicGoBackStudentBtn()
    {
        //personalDB.transform.DOScale(0.1f, 0.5f).SetEase(Ease.InBack).OnComplete(() => personalDB.SetActive(false));
        studentsDB.SetActive(true);
    }

    public void OnClickGoBackMyPageBtn()
    {
        // 학생관리 첫 페이지면 MyPage로 넘어감.
        if (studentsDB.activeSelf&& !personalDB.activeSelf)
        {
            PhotonNetwork.LoadLevel(2);
        }
        else if(personalDB.activeSelf)
        {
            Debug.Log("d이거 실행");
            personalDB.transform.DOScale(0.1f, 0.5f).SetEase(Ease.InBack).OnComplete(() => personalDB.SetActive(false));
        }
    }
}
