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
        //�α��� ���� üũ �̺�Ʈ ���
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

    public void OnClickSingIn(string email, string password)
    {
        //if (inputEmail.text.Length == 0 || inputPassword.text.Length == 0)
        //{
        //    print("������ �� �Է��� �ּ���");
        //    return;
        //}
        //Debug.Log(inputEmail.text+" : "+inputPassword.text);

        StartCoroutine(SingIn(email, password));
        //StartCoroutine(SingIn(registerManager.IsTeacher? inputEmailTeacher.text : inputEmailStudent.text, registerManager.IsTeacher ? inputPasswordTTeacher.text:inputPasswordStudent.text));
    }

    IEnumerator SingIn(string email, string password)
    {
        print(email + ", " + password);

        //ȸ�����Խõ�
        var task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        //����� �Ϸ�ɶ����� ��ٸ���
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            print("ȸ������ ����");

        }
        else
        {
            print("ȸ������ ���� : " + task.Exception);
        }
    }

    public void OnClickLogin(string email, string password)
    {
        StartCoroutine(Login(email, password));
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
        }
        else
        {
            print("�α��� ���� : " + task.Exception);
        }
    }

    public void OnClickLogOut()
    {
        //�α׾ƿ�
        auth.SignOut();
    }
}