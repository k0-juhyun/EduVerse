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
        SceneManager.sceneLoaded += OnSceneLoaded; // �̺�Ʈ�� �޼��� �߰�
        PhotonNetwork.LoadLevel(2);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 2 && PhotonNetwork.IsConnected) // 2�� ���� �ε�ǰ� ������ ����Ǿ��� ��
        {
            PhotonNetwork.Instantiate("Character", spawnPos, spawnRot);
            SceneManager.sceneLoaded -= OnSceneLoaded; // �޼��带 �̺�Ʈ���� ���� (�ߺ� ȣ�� ����)
        }
    }

    public override void OnLeftRoom()
    {
        // ���� ���� �� �κ�� ���ư��ų� �ٸ� �۾� ����
        PhotonNetwork.LoadLevel(1);
    }
}
