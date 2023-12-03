using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Firebase.Auth;
using Firebase.Database;
using System;
using UnityEngine.UI;
using Firebase.Extensions;


public class FireAuth : MonoBehaviour
{
    public static FireAuth instance = null;

    FirebaseAuth auth;

    FirebaseDatabase database;

    private bool isSignIn = false;

    private Coroutine coroutine;

    public Button signInBtn;
    public Button lonInBtn;
    public Button logOutBtn;

    public RegisterManager registerManager;

    public InputField inputLoginEmail;
    public InputField inputLoginPassword;

    public GameObject retryGetUserInfoText;             // 로그인은 성공했으나 유저정보를 제대로 가져오지 못했을 때 뜨는 텍스트
    public GameObject failedLogInText;                  // 로그인에 실패시 뜨는 텍스트

    public GameObject loginPanel;
    public GameObject signInPanel;


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

        //로그인 상태 체크 이벤트 등록
        auth = FirebaseAuth.DefaultInstance;

        GameManager.Instance.auth = auth;

        auth.StateChanged += OnChangedAuthState;

        //signInBtn.onClick.AddListener(OnClickSingIn);
        lonInBtn?.onClick.AddListener(() => OnClickLogin(inputLoginEmail.text, inputLoginPassword.text));
        //logOutBtn.onClick.AddListener(OnClickLogOut);
    }

    private void OnDestroy()
    {
        auth.StateChanged -= OnChangedAuthState;
    }

    void OnChangedAuthState(object sender, EventArgs e)
    {
        //만약에 유저정보 있으면
        if (auth.CurrentUser != null)
        {
            //FireDatabase.instance.myInfo.email = auth.CurrentUser.Email;
            //FireDatabase.instance.myInfo.fbId = auth.CurrentUser.UserId;

            print("로그인 상태");

            // 회원가입 후 바로 로그인X
            if (isSignIn)
            {
                isSignIn = false;
                //loginPanel.SetActive(true);
                //signInPanel.SetActive(false);
                //OnClickLogOut();
                return;
            }

            //if (SceneManager.GetActiveScene().buildIndex == 7)
            //{
            //    auth.SignOut();
            //    return;
            //}

            //FirebaseDatabase.DefaultInstance.GetReference("Leaders").GetValueAsync().ContinueWithOnMainThread(task =>
            //{
            //    if (task.IsFaulted)
            //    {
            //            // Handle the error...
            //        }
            //    else if (task.IsCompleted)
            //    {
            //        DataSnapshot snapshot = task.Result;
            //            // Do something with snapshot...

            //            print("유저 정보 가져오기 성공 : " + task.Result.Value);

            //            // json형식으로 가져와서 UserInfo 타입으로 바꿔줌
            //            UserInfo user = JsonUtility.FromJson<UserInfo>(snapshot.GetRawJsonValue());
            //        print(user.name);

            //            // 가져온 유저정보 담아두기
            //            DataBase.instance.SetMyInfo(new User(user.name, user.isteacher), user);
            //        if (PhotonNetwork.IsConnectedAndReady)
            //        {
            //            PhotonNetwork.NickName = user.name;
            //            PhotonNetwork.LoadLevel(1);
            //        }
            //    }
            //});

            
        }
        //그렇지 않으면
        else
        {
            print("로그아웃 상태");
            int curSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
            if (curSceneBuildIndex != 0 && curSceneBuildIndex != 7)
            {
                //LogOut();
            }
        }
    }

    public void GoSignin()
    {
        isSignIn = true;
    }


    // 로그인 상태되면
    private IEnumerator OnLogIn()
    {
        // 로그인한 유저의 정보 가져오기
        var dataTask = database.GetReference("USER_INFO").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).GetValueAsync();
        print(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        yield return new WaitUntil(() => dataTask.IsCompleted);
        if (dataTask.IsCompletedSuccessfully)
        {
            DataSnapshot userInfo = dataTask.Result;

            if (userInfo == null)
            {
                print("데이터 안들어옴");
                yield return new WaitForSeconds(2f);
                yield return OnLogIn();
            }

            print("유저 정보 가져오기 성공 : " + dataTask.Result.ChildrenCount);

            // 정보를 제대로 불러오지 못하면 다시 로그인 시도 
            if (dataTask.Result.ChildrenCount == 0)
            {
                print("회원정보 로드 실패 : " + dataTask.Result.Value);
                auth.SignOut();
                retryGetUserInfoText.SetActive(true);
            }
            else
            {
                // json형식으로 가져와서 UserInfo 타입으로 바꿔줌
                UserInfo user = JsonUtility.FromJson<UserInfo>(userInfo.GetRawJsonValue());
                print(user.name);

                // 가져온 유저정보 담아두기
                DataBase.instance.SetMyInfo(new User(user.name, user.isteacher), user);
                if (PhotonNetwork.IsConnectedAndReady)
                {
                    PhotonNetwork.NickName = user.name;
                    PhotonNetwork.LoadLevel(1);
                }
            }
        }
        else
        {
            print("회원정보 로드 실패 : " + dataTask.Result.Value);
            auth.SignOut();
            retryGetUserInfoText.SetActive(true);
        }
    }

    public void OnClickSingIn(string email, string password, System.Action action = null)
    {
        //if (inputEmail.text.Length == 0 || inputPassword.text.Length == 0)
        //{
        //    print("정보를 다 입력해 주세요");
        //    return;
        //}
        //Debug.Log(inputEmail.text+" : "+inputPassword.text);
        if (coroutine == null)
        {
            coroutine = StartCoroutine(SingIn(email, password, action));
        }
        //StartCoroutine(SingIn(registerManager.IsTeacher? inputEmailTeacher.text : inputEmailStudent.text, registerManager.IsTeacher ? inputPasswordTTeacher.text:inputPasswordStudent.text));
    }

    IEnumerator SingIn(string email, string password, System.Action action = null)
    {
        print(email + ", " + password);

        //회원가입시도
        var task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        //통신이 완료될때까지 기다린다
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            print("회원가입 성공");
            action();
            //NetworkManager.instance.LoadScene(0);
            //OnClickLogOut();
        }
        else
        {
            registerManager.OnSignInFailed();
            print("회원가입 실패 : " + task.Exception);
        }

        coroutine = null;
    }

    public void OnClickLogin(string email, string password)
    {
        if (email.Length > 0 && password.Length > 0)
        {
            if (coroutine == null)
            {
                failedLogInText.gameObject.SetActive(false);
                retryGetUserInfoText.SetActive(false);
                coroutine = StartCoroutine(Login(email, password));
            }
        }
        //StartCoroutine(Login(inputLoginEmail.text, inputLoginPassword.text));
    }

    IEnumerator Login(string email, string password)
    {
        //로그인 시도
        var task = auth.SignInWithEmailAndPasswordAsync(email, password);
        //통신이 완료될때까지 기다린다
        yield return new WaitUntil(() => task.IsCompleted);
        //만약에 Error가 없다면
        if (task.Exception == null)
        {
            print("로그인 성공");
            StartCoroutine(OnLogIn());
        }
        else
        {
            print("로그인 실패 : " + task.Exception);
            inputLoginEmail.text = "";
            inputLoginPassword.text = "";
            OnFailedLogIn();
        }

        coroutine = null;
    }

    private void OnFailedLogIn()
    {
        failedLogInText.SetActive(true);
        //Invoke(nameof(ActiveFalseFailedLogIn), 1f);
    }

    private void ActiveFalseFailedLogIn()
    {
        failedLogInText.SetActive(false);
    }

    public void LogOut()
    {
        //로그아웃
        auth.SignOut();
        Destroy(SoundManager.instance.gameObject);
        Destroy(GameManager.Instance.gameObject);
        Destroy(MyQuizStorage.Instance.gameObject);
        Destroy(MyQuizStorage.Instance.gameObject);
        PhotonNetwork.LoadLevel(0);
    }
}
