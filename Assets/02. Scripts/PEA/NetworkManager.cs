using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance = null;

    private Vector3 spawnPos = new Vector3(-4, 0, 6);
    private Quaternion spawnRot = Quaternion.identity;

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

    void Update()
    {

    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinOrCreateRoom("roomName", new RoomOptions(), TypedLobby.Default);
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

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        SceneManager.sceneLoaded += OnSceneLoaded; // 이벤트에 메서드 추가
        PhotonNetwork.LoadLevel(2);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 2 && PhotonNetwork.IsConnected) // 2번 씬이 로드되고 포톤이 연결되었을 때
        {
            PhotonNetwork.Instantiate("Character", spawnPos, spawnRot);
            SceneManager.sceneLoaded -= OnSceneLoaded; // 메서드를 이벤트에서 제거 (중복 호출 방지)
        }
    }

    public override void OnLeftRoom()
    {
        // 방을 나온 후 로비로 돌아가거나 다른 작업 수행
        PhotonNetwork.LoadLevel(1);
    }
}
