using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Firebase.Auth;
using Firebase.Database;
using System;
using UnityEngine.UI;

public class FireAuth : MonoBehaviour
{
    public static FireAuth instance = null;

    FirebaseAuth auth;
    FirebaseDatabase database;

    private Coroutine coroutine;

    public Button signInBtn;
    public Button lonInBtn;
    public Button logOutBtn;

    public RegisterManager registerManager;

    public InputField inputLoginEmail;
    public InputField inputLoginPassword;

    public GameObject failedLogInText;

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
        database = FirebaseDatabase.DefaultInstance;

       //�α��� ���� üũ �̺�Ʈ ���
       auth = FirebaseAuth.DefaultInstance;

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
        //���࿡ �������� ������
        if (auth.CurrentUser != null)
        {
            print("Email : " + auth.CurrentUser.Email);
            print("UserId : " + auth.CurrentUser.UserId);

            //FireDatabase.instance.myInfo.email = auth.CurrentUser.Email;
            //FireDatabase.instance.myInfo.fbId = auth.CurrentUser.UserId;

            print("�α��� ����");
        }
        //�׷��� ������
        else
        {
            print("�α׾ƿ� ����");
        }
    }

    public void OnClickSingIn(string email, string password, System.Action action = null)
    {
        //if (inputEmail.text.Length == 0 || inputPassword.text.Length == 0)
        //{
        //    print("������ �� �Է��� �ּ���");
        //    return;
        //}
        //Debug.Log(inputEmail.text+" : "+inputPassword.text);
        if(coroutine == null)
        {
            coroutine = StartCoroutine(SingIn(email, password, action));
        }
        //StartCoroutine(SingIn(registerManager.IsTeacher? inputEmailTeacher.text : inputEmailStudent.text, registerManager.IsTeacher ? inputPasswordTTeacher.text:inputPasswordStudent.text));
    }

    IEnumerator SingIn(string email, string password, System.Action action = null)
    {
        print(email + ", " + password);

        //ȸ�����Խõ�
        var task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        //����� �Ϸ�ɶ����� ��ٸ���
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            print("ȸ������ ����");
            action();
            //NetworkManager.instance.LoadScene(0);
        }
        else
        {
            registerManager.OnSignInFailed();
            print("ȸ������ ���� : " + task.Exception);
        }

        coroutine = null;
    }

    public void OnClickLogin(string email, string password)
    {
        if(email.Length > 0 && password.Length > 0)
        {
            if(coroutine == null)
            {
                coroutine = StartCoroutine(Login(email, password));
            }
        }
        //StartCoroutine(Login(inputLoginEmail.text, inputLoginPassword.text));
    }

    IEnumerator Login(string email, string password)
    {
        //�α��� �õ�
        var task = auth.SignInWithEmailAndPasswordAsync(email, password);
        //����� �Ϸ�ɶ����� ��ٸ���
        yield return new WaitUntil(() => task.IsCompleted);
        //���࿡ Error�� ���ٸ�
        if (task.Exception == null)
        {
            print("�α��� ����");

            // ȸ������ �� �ٷ� �α��� ����
            if(SceneManager.GetActiveScene().buildIndex == 8 )
            {
                auth.SignOut();
            }
            else
            {
                // �α����� ������ ���� ��������
                var dataTask = database.GetReference("USER_INFO").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).GetValueAsync();
                yield return new WaitUntil(() => dataTask.IsCompleted);
                if (dataTask.Exception == null)
                {
                    DataSnapshot user = dataTask.Result;

                    print("���� ���� �������� ����");
                    //print(user.)
                    string userName = user.Child("/name").Value.ToString();
                    DataBase.instance.SetMyInfo(new User(userName, (bool)user.Child("/isteacher").Value));
                    if (PhotonNetwork.IsConnectedAndReady)
                    {
                        PhotonNetwork.NickName = userName;
                        PhotonNetwork.LoadLevel(1);
                    }
                }
            }
        }
        else
        {
            print("�α��� ���� : " + task.Exception);
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

    public void OnClickLogOut()
    {
        //�α׾ƿ�
        auth.SignOut();
    }
}