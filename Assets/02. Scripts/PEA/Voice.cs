using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Voice;

public class Voice : MonoBehaviourPunCallbacks
{
    public static Voice instance = null;

    //private Photon.Voice.PUN.PunVoiceClient punVoiceClient;
    private Photon.Voice.Unity.Recorder recorder;
    private Photon.Voice.Unity.Speaker speaker;

    public GameObject player;

    public GameObject toggleCanvas;
    public Toggle mikeOnToggle;
    public Toggle listenToggle;
    public Toggle muteToggle;

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
        //punVoiceClient = GetComponent<Photon.Voice.PUN.PunVoiceClient>();
        recorder = GetComponent<Photon.Voice.Unity.Recorder>();
        //mikeOnToggle.onValueChanged.AddListener((b) => OnMikeOnToggleValueChanged(b));
        //muteToggle.onValueChanged.AddListener((b) => OnMuteToggleValueChanged(b));

        //PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        
    }

    //public override void OnConnectedToMaster()
    //{
    //    base.OnConnectedToMaster();
    //    print("Connected");
    //    PhotonNetwork.JoinLobby();
    //}

    //public override void OnJoinedLobby()
    //{
    //    base.OnJoinedLobby();
    //    print("Join Lobby");
    //    PhotonNetwork.JoinRandomOrCreateRoom();
    //}

    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();
    //    print("Join Room");
    //    player = PhotonNetwork.Instantiate("Voice_TestPlayer", Vector3.zero, Quaternion.identity);
    //    speaker = player.GetComponentInChildren<Photon.Voice.Unity.Speaker>();
    //    listenToggle.onValueChanged.AddListener((b) => OnListenToggleValueChanged(b));
    //}

    public void OnSceneLoaded(int loadedSceneBuildIndex)
    {
        switch (loadedSceneBuildIndex)
        {
            case 4: // ±≥Ω«
            case 6: // ±§¿Â
                toggleCanvas.SetActive(true);
                recorder.enabled = true;
                break;
            default:
                toggleCanvas.SetActive(false);
                recorder.enabled = false;
                break;
        }
    }

    public void OnMikeOnToggleValueChanged(bool isMikeOn)
    {
        recorder.TransmitEnabled = isMikeOn;
    }

    public void OnListenToggleValueChanged(bool isListen)
    {
        speaker.enabled = isListen;
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
