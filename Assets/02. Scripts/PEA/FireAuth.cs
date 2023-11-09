using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using System;
using UnityEngine.UI;

public class FireAuth : MonoBehaviour
{
    public static FireAuth instance;

    FirebaseAuth auth;

    public Button signInBtn;
    public Button lonInBtn;
    public Button logOutBtn;

    public RegisterManager registerManager;

    //email, password inputfield
    public InputField inputEmailTeacher;
    public InputField inputEmailStudent;
    public InputField inputPasswordTTeacher;
    public InputField inputPasswordStudent;

    public InputField inputLoginEmail;
    public InputField inputLoginPassword;

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
        //로그인 상태 체크 이벤트 등록
        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += OnChangedAuthState;

        //signInBtn.onClick.AddListener(OnClickSingIn);
        //lonInBtn.onClick.AddListener(OnClickLogin);
        //logOutBtn.onClick.AddListener(OnClickLogOut);
    }

    private void OnDestroy()
    {
        auth.StateChanged -= OnChangedAuthState;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    OnClickSingIn();
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    OnClickLogin();
        //}
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnClickLogOut();
        }
    }

    void OnChangedAuthState(object sender, EventArgs e)
    {
        //만약에 유저정보 있으면
        if (auth.CurrentUser != null)
        {
            print("Email : " + auth.CurrentUser.Email);
            print("UserId : " + auth.CurrentUser.UserId);

            //FireDatabase.instance.myInfo.email = auth.CurrentUser.Email;
            //FireDatabase.instance.myInfo.fbId = auth.CurrentUser.UserId;

            print("로그인 상태");
        }
        //그렇지 않으면
        else
        {
            print("로그아웃 상태");
        }
    }

    public void OnClickSingIn(string email, string password)
    {
        //if (inputEmail.text.Length == 0 || inputPassword.text.Length == 0)
        //{
        //    print("정보를 다 입력해 주세요");
        //    return;
        //}
        //Debug.Log(inputEmail.text+" : "+inputPassword.text);

        StartCoroutine(SingIn(email, password));
        //StartCoroutine(SingIn(registerManager.IsTeacher? inputEmailTeacher.text : inputEmailStudent.text, registerManager.IsTeacher ? inputPasswordTTeacher.text:inputPasswordStudent.text));
    }

    IEnumerator SingIn(string email, string password)
    {
        print(email + ", " + password);

        //회원가입시도
        var task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        //통신이 완료될때까지 기다린다
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            print("회원가입 성공");

        }
        else
        {
            print("회원가입 실패 : " + task.Exception);
        }
    }

    public void OnClickLogin(string email, string password)
    {
        StartCoroutine(Login(email, password));
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
        }
        else
        {
            print("로그인 실패 : " + task.Exception);
        }
    }

    public void OnClickLogOut()
    {
        //로그아웃
        auth.SignOut();
    }
}