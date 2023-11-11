using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Voice;

public class Voice : MonoBehaviourPunCallbacks
{
    public static Voice instance = null;

    private Photon.Voice.PUN.PunVoiceClient punVoiceClient;
    private Photon.Voice.Unity.Recorder recorder;
    private Photon.Voice.Unity.Speaker speaker;

    public GameObject player;

    public Toggle mikeOnToggle;
    public Toggle listenToggle;
    public Toggle muteToggle;

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
        //punVoiceClient = GetComponent<Photon.Voice.PUN.PunVoiceClient>();
        //recorder = GetComponent<Photon.Voice.Unity.Recorder>();
        //mikeOnToggle.onValueChanged.AddListener((b) => OnMikeOnToggleValueChanged(b));
        //muteToggle.onValueChanged.AddListener((b) => OnMuteToggleValueChanged(b));

        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("Connected");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("Join Lobby");
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        print("Join Room");
        player = PhotonNetwork.Instantiate("Voice_TestPlayer", Vector3.zero, Quaternion.identity);
        //speaker = Instantiate(punVoiceClient.SpeakerPrefab, player.transform).GetComponent<Photon.Voice.Unity.Speaker>();
        listenToggle.onValueChanged.AddListener((b) => OnListenToggleValueChanged(b));
    }

    public void OnMikeOnToggleValueChanged(bool isMikeOn)
    {
        recorder.TransmitEnabled = isMikeOn;
    }

    public void OnListenToggleValueChanged(bool IsListen)
    {
        speaker.enabled = IsListen;
    }

    public void OnMuteToggleValueChanged(bool useStudentsMike)
    {
       player.GetComponent<VoiceTest_Player>().MuteStudents(useStudentsMike);
    }

    public void Mute(bool useMike)
    {
        recorder.TransmitEnabled = useMike;
    }
}
