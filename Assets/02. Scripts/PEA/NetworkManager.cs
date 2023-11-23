using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance = null;

    private Vector3 teacherSpawnPos = new Vector3(-6.7f, 0, 5.5f);
    private Vector3 studentSpawnPos = new Vector3(6, 0, 5);
    private Quaternion spawnRot = Quaternion.Euler(0, 180, 0);

    [HideInInspector] public bool isCustom = false;
    [HideInInspector] public bool enableChoose = false;

    public delegate void LoadSceneProgress(float progress);

    public event LoadSceneProgress OnLoadSceneProgress;

    private bool leaveFlag;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        PhotonNetwork.SerializationRate = 60;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAfterLeavingRoom(sceneIndex));
    }

    private IEnumerator LoadSceneAfterLeavingRoom(int sceneIndex)
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            while (PhotonNetwork.InRoom)
            {
                yield return null;
            }
        }

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            while (!PhotonNetwork.IsConnectedAndReady)
            {
                yield return null;
            }
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;  // �� Ȱ��ȭ�� �����մϴ�.

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log($"Loading progress: {progress}");
            OnLoadSceneProgress?.Invoke(progress);

            if (asyncLoad.progress >= 0.9f)
            {
                Debug.Log("Activating scene...");
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        OnLoadSceneProgress?.Invoke(1f);
    }


    public void JoinRoom(string sceneName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "Scene", sceneName } };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "Scene" };
        PhotonNetwork.JoinOrCreateRoom(sceneName + "Room", roomOptions, TypedLobby.Default);
    }

    public void ChangeRoom(string sceneName)
    {
        StartCoroutine(IChangeRoomCoroutine(sceneName));
    }

    private IEnumerator IChangeRoomCoroutine(string sceneName)
    {
        // Game ������ ����Ǿ� �ִ��� Ȯ���մϴ�.
        if (PhotonNetwork.InRoom)
        {
            leaveFlag = true;

            // ���� �������� �մϴ�.
            PhotonNetwork.LeaveRoom();
            print("���� ������ ��...");

            // ���� ���������� �������� Ȯ���մϴ�.
            yield return new WaitUntil(() => !PhotonNetwork.InRoom);
        }
        else
        {
            // �̹� ���� ���� ���, Master ������ ����Ǿ� ���� �� �ֽ��ϴ�.
            // Game ������ �翬���� �õ��մϴ�.
            PhotonNetwork.JoinLobby();
            yield return new WaitUntil(() => PhotonNetwork.InLobby);
        }

        // �ӽ� �ε� ������ �̵��մϴ�.
        PhotonNetwork.JoinLobby();
        PhotonNetwork.LoadLevel("LoadingScene");
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "LoadingScene");
        print("���ο� ������ �̵� �غ� �Ϸ�");

        //yield return new WaitUntil(() => PhotonNetwork.InLobby);

        //yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);

        yield return new WaitUntil(() => PhotonNetwork.NetworkClientState == ClientState.JoinedLobby);

        print("�κ�κ�κ�");

        JoinRoom(sceneName);

        //RoomOptions roomOptions = new RoomOptions();
        //PhotonNetwork.JoinOrCreateRoom("5.GroundScene", roomOptions, TypedLobby.Default);
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("���������");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4; // �ִ� �÷��̾� �� ����
        PhotonNetwork.JoinRandomOrCreateRoom(null, 0, MatchmakingMode.FillRoom, null, null, "DefaultRoom", roomOptions);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("�κ�");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        print("pea babo1");
        PhotonNetwork.JoinLobby();
        print("pea babo2");
        if (isCustom)
        {
            LoadScene(1);
            isCustom = false;
        }
    }


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        print("������ : " + PhotonNetwork.CurrentRoom.Name);

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Scene", out object sceneName))
        {
            if (!string.IsNullOrEmpty((string)sceneName))
            {
                PhotonNetwork.LoadLevel((string)sceneName);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
        }
        else
        {
            print("33333333");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("�� �÷��̾ �濡 ���Խ��ϴ�: " + newPlayer.NickName);

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player != newPlayer)
            {

            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (PhotonNetwork.IsConnected)
        {
            Vector3 spawnPos = Vector3.zero;
            if (scene.buildIndex == 4)
            {
                spawnPos = (DataBase.instance.user.isTeacher ? teacherSpawnPos : studentSpawnPos);
                PhotonNetwork.Instantiate("Character", spawnPos, spawnRot).GetComponentInChildren<CharacterSound>().OnChangeScene(scene.buildIndex);
            }
            else if (scene.buildIndex == 5)
            {
                spawnPos = (DataBase.instance.user.isTeacher ? new Vector3(14, 1.4f, 10) : new Vector3(12.5f, 0, 17.5f));
                spawnRot = (DataBase.instance.user.isTeacher ? Quaternion.Euler(0, 0, 0) : spawnRot);
                PhotonNetwork.Instantiate("Character", spawnPos, spawnRot).GetComponentInChildren<CharacterSound>().OnChangeScene(scene.buildIndex);
            }

            //Voice.instance.OnSceneLoaded(scene.buildIndex);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnApplicationQuit()
    {
        Firebase.Auth.FirebaseAuth.DefaultInstance.SignOut();
    }
}
