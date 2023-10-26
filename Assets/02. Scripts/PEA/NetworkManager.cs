using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance = null;

    private Vector3 teacherSpawnPos = new Vector3(-6.7f, 0, 5.5f);
    private Vector3 studentSpawnPos = new Vector3(6, 0, 5);
    private Quaternion spawnRot = Quaternion.identity;

    private bool shouldJoinNewRoom = false;
    private string newRoomName = "";

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
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("Joined Lobby");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        shouldJoinNewRoom = true;
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
            Vector3 spawnPos = (DataBase.instance.userInfo.isTeacher ? teacherSpawnPos : studentSpawnPos);
            PhotonNetwork.Instantiate("Character", spawnPos, spawnRot);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void Update()
    {
        if (shouldJoinNewRoom && PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby)
        {
            JoinRoom(newRoomName);
            shouldJoinNewRoom = false;
        }
    }
}
