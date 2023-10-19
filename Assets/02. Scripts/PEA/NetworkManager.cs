using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

        print("Joined Room");

        PhotonNetwork.LoadLevel(1);
    }

}
