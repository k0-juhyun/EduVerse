using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.WebRTC;
using UnityEngine;
using UnityEngine.UI;

public class RtcReceiver : MonoBehaviour
{
    //RTC 구성
    RTCConfiguration configuration = new RTCConfiguration
    {
        iceServers = new[] { new RTCIceServer { urls = new[] { "stun:stun.l.google.com:19302" } } }
    };

    //나와 연결되는 유저들의 peer
    Dictionary<int, RTCPeerConnection> peerList = new Dictionary<int, RTCPeerConnection>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    
    public void CreatePeer(int senderId, string strDesc)
    {
        

        RTCPeerConnection peer = new RTCPeerConnection(ref configuration);

        //나와 연결되는 유저와의 소통할 수 있게 해주는 정보가 만들어지면 호출되는 함수 등록
        peer.OnIceCandidate = (RTCIceCandidate candidate) => {
            RTCIceCandidateInit candidateInit = new RTCIceCandidateInit();
            candidateInit.candidate = candidate.Candidate;
            candidateInit.sdpMid = candidate.SdpMid;
            candidateInit.sdpMLineIndex = candidate.SdpMLineIndex;

            //sender 유저에게 candidateInit 을 알려주자.
            string strCandidate = JsonUtility.ToJson(candidateInit);
            int receiverId = GetComponent<PhotonView>().ViewID;
            GameMgr.instance.AddIceCandidate(senderId, receiverId, strCandidate, true);
        };

        //나와 연결되는 유저와의 연결 상태가 변경될 때마다 호출되는 함수 등록
        peer.OnIceConnectionChange = (RTCIceConnectionState state) =>
        {
            print("OnIceConnectionChange : " + state);
        };

        //공유자가 공유하고 싶은 미디어들이 변경이 되면 호출되는 함수 등록
        peer.OnTrack = (RTCTrackEvent e) =>
        {
            //비디오 트랙이라면
            if (e.Track is VideoStreamTrack videoStreamTrack)
            {
                videoStreamTrack.OnVideoReceived += (Texture renderer) =>
                {
                    //공유 받은 화면을 그릴 RawIamge 만들자
                    RawImage screen = GameMgr.instance.AddScreen();
                    screen.texture = renderer;
                };
            }
        };

        //나와 연결되는 유저들의 peer 를 Dictionary 에 가지고 있자.
        peerList[senderId] = peer;

        // 공유 하는 유저에게서 넘어온 strDesc 를 RTCSessionDescription 형태로 만들어서
        // SetRemoteDescription 을 진행시키자 
        RTCSessionDescription sessionDescription = JsonUtility.FromJson<RTCSessionDescription>(strDesc);
        StartCoroutine(SetRemoteDescription(senderId, sessionDescription));
    }

    IEnumerator SetRemoteDescription(int senderId, RTCSessionDescription sessionDescription)
    {
        RTCPeerConnection p = peerList[senderId];
        RTCSetSessionDescriptionAsyncOperation operation = p.SetRemoteDescription(ref sessionDescription);
        yield return operation;

        if (operation.IsError)
        {
            print("SetRemoteDescription : " + operation.Error.message);
        }
        else
        {
            print("SetRemoteDescription Success");

            // SetRemoteDescription 이 성공하면 CreateAnswer 요청 
            yield return CreateAnswer(senderId);
        }
    }

    IEnumerator CreateAnswer(int senderId)
    {
        RTCPeerConnection p = peerList[senderId];
        RTCSessionDescriptionAsyncOperation operation = p.CreateAnswer();
        yield return operation;

        if (operation.IsError)
        {
            print("CreateAnswer : " + operation.Error.message);
        }
        else
        {
            print("CreateAnswer Success");

            // CreateAnswer 가 성공했다면 SetLocalDescription 요청 
            yield return OnSuccessCreateAnswer(senderId, operation.Desc);
        }
    }

    IEnumerator OnSuccessCreateAnswer(int senderId, RTCSessionDescription sessionDescription)
    {
        RTCPeerConnection p = peerList[senderId];
        RTCSetSessionDescriptionAsyncOperation operation = p.SetLocalDescription(ref sessionDescription);
        yield return operation;

        if (operation.IsError)
        {
            print("OnSuccessCreateAnswer : " + operation.Error.message);
        }
        else
        {
            print("OnSuccessCreateAnswer Success");

            // 공유 하는 유저야 SetRemoteDescription 을 수행해라 
            string strDesc = JsonUtility.ToJson(sessionDescription);
            int receiverId = GetComponent<PhotonView>().ViewID;
            GameMgr.instance.SetRemoteDescription(receiverId, senderId, strDesc);
        }
    }

    public void AddIceCandidate(int senderId, string strCandidate)
    {
        print("recevier");
        RTCIceCandidateInit candidateInit = JsonUtility.FromJson<RTCIceCandidateInit>(strCandidate);
        if (peerList.ContainsKey(senderId))
        {
            print("--- " + strCandidate + " ---");
            peerList[senderId].AddIceCandidate(new RTCIceCandidate(candidateInit));
        }
    }
}
