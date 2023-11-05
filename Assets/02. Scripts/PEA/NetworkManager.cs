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
    private Quaternion spawnRot = Quaternion.identity;

    private bool shouldJoinNewRoom = false;
    [HideInInspector] public bool isCustom = false;
    [HideInInspector] public bool enableChoose = false;

    private string newRoomName = "";

    public delegate void LoadSceneProgress(float progress);

    public event LoadSceneProgress OnLoadSceneProgress;

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
        asyncLoad.allowSceneActivation = false;  // 씬 활성화를 중지합니다.

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
        newRoomName = sceneName;
        StartCoroutine(ChangeRoomCoroutine());
    }

    private IEnumerator ChangeRoomCoroutine()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }

        SceneManager.LoadScene("LoadingScene"); // 로딩 씬 이름
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "LoadingScene");

        shouldJoinNewRoom = true;
    }


    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비들림");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.ConnectUsingSettings();
        shouldJoinNewRoom = true;
        if (isCustom)
        {
            LoadScene(1);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Scene", out object sceneName))
        {
            if (!string.IsNullOrEmpty((string)sceneName))
            {
                PhotonNetwork.LoadLevel((string)sceneName);
                SceneManager.sceneLoaded += OnSceneLoaded;
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
                spawnPos = (DataBase.instance.userInfo.isTeacher ? teacherSpawnPos : studentSpawnPos);
            }
            else if (scene.buildIndex == 5)
            {
                spawnPos = new Vector3(-5.75f, 0, -1.85f);
            }

            PhotonNetwork.Instantiate("Character", spawnPos, spawnRot);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void Update()
    {
        if (shouldJoinNewRoom && PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby && !isCustom && !enableChoose)
        {
            JoinRoom(newRoomName);
            shouldJoinNewRoom = false;
        }
    }
}
