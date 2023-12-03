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

    public GameObject retryGetUserInfoText;             // �α����� ���������� ���������� ����� �������� ������ �� �ߴ� �ؽ�Ʈ
    public GameObject failedLogInText;                  // �α��ο� ���н� �ߴ� �ؽ�Ʈ

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

        //�α��� ���� üũ �̺�Ʈ ���
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
        //���࿡ �������� ������
        if (auth.CurrentUser != null)
        {
            //FireDatabase.instance.myInfo.email = auth.CurrentUser.Email;
            //FireDatabase.instance.myInfo.fbId = auth.CurrentUser.UserId;

            print("�α��� ����");

            // ȸ������ �� �ٷ� �α���X
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

            //            print("���� ���� �������� ���� : " + task.Result.Value);

            //            // json�������� �����ͼ� UserInfo Ÿ������ �ٲ���
            //            UserInfo user = JsonUtility.FromJson<UserInfo>(snapshot.GetRawJsonValue());
            //        print(user.name);

            //            // ������ �������� ��Ƶα�
            //            DataBase.instance.SetMyInfo(new User(user.name, user.isteacher), user);
            //        if (PhotonNetwork.IsConnectedAndReady)
            //        {
            //            PhotonNetwork.NickName = user.name;
            //            PhotonNetwork.LoadLevel(1);
            //        }
            //    }
            //});

            
        }
        //�׷��� ������
        else
        {
            print("�α׾ƿ� ����");
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


    // �α��� ���µǸ�
    private IEnumerator OnLogIn()
    {
        // �α����� ������ ���� ��������
        var dataTask = database.GetReference("USER_INFO").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).GetValueAsync();
        print(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        yield return new WaitUntil(() => dataTask.IsCompleted);
        if (dataTask.IsCompletedSuccessfully)
        {
            DataSnapshot userInfo = dataTask.Result;

            if (userInfo == null)
            {
                print("������ �ȵ���");
                yield return new WaitForSeconds(2f);
                yield return OnLogIn();
            }

            print("���� ���� �������� ���� : " + dataTask.Result.ChildrenCount);

            // ������ ����� �ҷ����� ���ϸ� �ٽ� �α��� �õ� 
            if (dataTask.Result.ChildrenCount == 0)
            {
                print("ȸ������ �ε� ���� : " + dataTask.Result.Value);
                auth.SignOut();
                retryGetUserInfoText.SetActive(true);
            }
            else
            {
                // json�������� �����ͼ� UserInfo Ÿ������ �ٲ���
                UserInfo user = JsonUtility.FromJson<UserInfo>(userInfo.GetRawJsonValue());
                print(user.name);

                // ������ �������� ��Ƶα�
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
            print("ȸ������ �ε� ���� : " + dataTask.Result.Value);
            auth.SignOut();
            retryGetUserInfoText.SetActive(true);
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
        if (coroutine == null)
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
            //OnClickLogOut();
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
        //�α��� �õ�
        var task = auth.SignInWithEmailAndPasswordAsync(email, password);
        //����� �Ϸ�ɶ����� ��ٸ���
        yield return new WaitUntil(() => task.IsCompleted);
        //���࿡ Error�� ���ٸ�
        if (task.Exception == null)
        {
            print("�α��� ����");
            StartCoroutine(OnLogIn());
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

    public void LogOut()
    {
        //�α׾ƿ�
        auth.SignOut();
        Destroy(SoundManager.instance.gameObject);
        Destroy(GameManager.Instance.gameObject);
        Destroy(MyQuizStorage.Instance.gameObject);
        Destroy(MyQuizStorage.Instance.gameObject);
        PhotonNetwork.LoadLevel(0);
    }
}
