using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance = null;

    private Vector3 teacherSpawnPos = new Vector3(-6.7f, 0, 5.5f);
    private Vector3 studentSpawnPos = new Vector3(6, 0, 5);
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
        PhotonNetwork.LoadLevel(4);
        SceneManager.sceneLoaded += OnSceneLoaded; // �̺�Ʈ�� �޼��� �߰�
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if ((scene.buildIndex == 4 || scene.buildIndex ==5) && PhotonNetwork.IsConnected) // 2�� ���� �ε�ǰ� ������ ����Ǿ��� ��
        {
            Vector3 spawnPos = (DataBase.instance.userInfo.isTeacher ? teacherSpawnPos : studentSpawnPos);
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
