using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance = null;
    public Text photontext;
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
        //if (PhotonNetwork.IsConnectedAndReady)
        //{
        //    NetworkManager.instance.photontext.text = "준비완료";
        //}
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinOrCreateRoom("roomName", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnConnectedToMaster()
    {
        photontext.text = "OnConnectedToMaster";
        print("OnConnectedToMaster");

        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        photontext.text = "Joined Lobby";
        print("Joined Lobby");
       
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        photontext.text = "Joined Room";

        print("Joined Room");
        
        PhotonNetwork.LoadLevel(2);
    }
}
