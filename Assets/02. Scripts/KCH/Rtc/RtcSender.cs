using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.WebRTC;
using UnityEngine;

public class RtcSender : MonoBehaviourPun
{

    // 화면 공유 받을 유저의 view id 리스트
    public List<int> receiverList = new List<int>();

    // 공유하고 싶은 미디어를 담을 변수(화면, 음성 등)
    MediaStream mediaStream;

    // WebCam 실행하기 위한 변수
    WebCamTexture webCamTexture;

    // RTC 구성
    RTCConfiguration configuration = new RTCConfiguration
    {
        iceServers = new[] { new RTCIceServer { urls = new[] { "stun:stun.l.google.com:19302" } } }
    };

    // 나와 연결되는 유저들의 peer
    Dictionary<int, RTCPeerConnection> peerList = new Dictionary<int, RTCPeerConnection>();

    private void Awake()
    {
        // WebRtc 초기화 
        WebRTC.Initialize(false);
    }

    void Start()
    {

        // WebRtc 업데이트 
        StartCoroutine(WebRTC.Update());
    }
    private void OnDestroy()
    {
        foreach (var peer in peerList.Values)
        {
            peer.Close();
        }
        peerList.Clear();

        // WebRtc 해제 
        WebRTC.Dispose();
    }

    void Update()
    {
        // 만약에 공유가 시작되었으면 
        //if (mediaStream != null && receiverList.Count > 0)
        //{
        //    // 사람들과 연결을 시작하자 (한번씩 만)
        //    for(int i = 0; i < receiverList.Count; i++)
        //    {
        //        CreatePeer(receiverList[i]);
        //    }

        //    receiverList.Clear();
        //}

        if (mediaStream != null && receiverList.Count > 0)
        {
            foreach (int receiverId in new List<int>(receiverList))
            {
                if (!peerList.ContainsKey(receiverId))
                {
                    CreatePeer(receiverId);
                }
            }

            receiverList.Clear();
        }
    }

    public void AddReceiverList(int receiverId)
    {
        if (!receiverList.Contains(receiverId))
        {
            receiverList.Add(receiverId);
        }
    }

    public Camera shareCam;
    public void Setup()
    {
        shareCam = GameObject.Find("ShareCamera").GetComponent<Camera>(); //Camera.main;

        mediaStream = new MediaStream();

        // WebCam 사용시..
        // WebCam 목록 중 0번째를 담아 놓자 
        //WebCamDevice webCamDevice = WebCamTexture.devices[0];
        //// 선택한 WebCam 을 720x720 , 30 Frame 으로 출력되게 설
        //webCamTexture = new WebCamTexture(webCamDevice.name, 720, 720, 30);
        //// WebCam 을 실행하자 
        //webCamTexture.Play();

        //// WebCam 화면을 mediaStream 에 등록할 track 으로 만들기
        //VideoStreamTrack trackCam = new VideoStreamTrack(webCamTexture);

        // 카메라가 보고있는 화면을 mediaStream 에 등록할 track 으로 만들기
        VideoStreamTrack track = shareCam.CaptureStreamTrack(1280, 800);

        // track 을 mediaStream 에 추가한다.
        //mediaStream.AddTrack(trackCam);
        mediaStream.AddTrack(track);

        // 내 WebCam 화면을 그릴 RawIamge 만들고 WebCam 을 나오게 셋팅
        //GameMgr.instance.CreateMyWebCamView(webCamTexture);

        // 만약에 카메라의 화면을 공유하려면 webCamTexture 가 아닌 shareCam.targetTexture 을 설정 
        GameMgr.instance.CreateMyWebCamView(shareCam.targetTexture);
    }


    void CreatePeer(int receiverId)
    {
        RTCPeerConnection peer = new RTCPeerConnection(ref configuration);

        // 나와 연결되는 유저와의 소통할 수 있게 해주는 정보가 만들어지면 호출되는 함수 등록
        peer.OnIceCandidate = (RTCIceCandidate candidate) => {
            RTCIceCandidateInit candidateInit = new RTCIceCandidateInit();
            candidateInit.candidate = candidate.Candidate;
            candidateInit.sdpMid = candidate.SdpMid;
            candidateInit.sdpMLineIndex = candidate.SdpMLineIndex;

            // 나와 연결되는 receiver 유저들에게 candidateInit 을 알려주자.
            string strCandidate = JsonUtility.ToJson(candidateInit);
            int senderId = GetComponent<PhotonView>().ViewID;
            //동시에 진행하게 되면 receiverId 가 다른 사람의 id 로 전달 될 수도 있다
            //정확한 receiverId 를 전달하게 바꿔야 한다 
            GameMgr.instance.AddIceCandidate(receiverId, senderId, strCandidate, false);
        };

        //나와 연결되는 유저와의 연결 상태가 변경될 때마다 호출되는 함수 등록
        peer.OnIceConnectionChange = (RTCIceConnectionState state) => {
            print("OnIceConnectionChange : " + state);
        };

        //공유하고자 하는 미디어들을 peer 를 이용해서 셋팅하자.
        List<RTCRtpSender> rTCRtpSenders = new List<RTCRtpSender>();
        foreach (MediaStreamTrack track in mediaStream.GetTracks())
        {
            RTCRtpSender s = peer.AddTrack(track, mediaStream);
            if (track.Kind == TrackKind.Video)
            {
                rTCRtpSenders.Add(s);
            }
        }


        //나와 연결되는 유저들의 peer 를 Dictionary 에 가지고 있자.
        peerList[receiverId] = peer;

        //Offer ?? 를 만들자
        StartCoroutine(CreateOffer(receiverId));
    }

    IEnumerator CreateOffer(int receiverId)
    {
        RTCPeerConnection p = peerList[receiverId];
        RTCSessionDescriptionAsyncOperation operation = p.CreateOffer();
        yield return operation;

        if (operation.IsError)
        {
            print("CreateOffer : " + operation.Error.message);
        }
        else
        {
            print("CreateOffer Success");
            // Offer 이 잘 만들어졌다면 SetLocalDescription 을 하자 
            yield return OnSuccessCreateOffer(receiverId, operation.Desc);
        }
    }

    IEnumerator OnSuccessCreateOffer(int receiverId, RTCSessionDescription sessionDescription)
    {
        RTCPeerConnection p = peerList[receiverId];
        RTCSetSessionDescriptionAsyncOperation operation = p.SetLocalDescription(ref sessionDescription);
        yield return operation;

        if (operation.IsError)
        {
            print("OnSuccessCreateOffer : " + operation.Error.message);
        }
        else
        {
            print("OnSuccessCreateOffer  Success");

            // sessionDescription JSON 형태로 만든다 
            string strDesc = JsonUtility.ToJson(sessionDescription);
            // 나의 photon view id 를 가져오자 
            int senderId = GetComponent<PhotonView>().ViewID;
            // 공유 받을 유저에서 peer 를 만들라고 요청 
            GameMgr.instance.CreateViewerPeer(receiverId, senderId, strDesc);
        }
    }

    public IEnumerator SetRemoteDescription(int receiverId, string strDesc)
    {
        RTCPeerConnection p = peerList[receiverId];
        RTCSessionDescription sessionDescription = JsonUtility.FromJson<RTCSessionDescription>(strDesc);
        RTCSetSessionDescriptionAsyncOperation operation = p.SetRemoteDescription(ref sessionDescription);
        yield return operation;

        if (operation.IsError)
        {
            print("SetRemoteDescription : " + operation.Error.message);
        }
        else
        {
            print("SetRemoteDescription  Success");
        }
    }

    public void AddIceCandidate(int receiverId, string strCandidate)
    {
        print("sender");
        RTCIceCandidateInit candidateInit = JsonUtility.FromJson<RTCIceCandidateInit>(strCandidate);
        if (peerList.ContainsKey(receiverId))
        {
            print("--- " + strCandidate + " ---");
            peerList[receiverId].AddIceCandidate(new RTCIceCandidate(candidateInit));
        }
    }
}