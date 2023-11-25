using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RtcPlayer : MonoBehaviourPun
{
    // 화면, 음성, 데이터 등을 보내는 역할
    RtcSender rtcSender;
    // 화면, 음성, 데이터 등을 내는 역할
    RtcReceiver rtcReceiver;

    

    void Start()
    {
        // 로드된 씬이 교무실이라면. return 해줘야함.
        if (!GameObject.Find("LoadScene"))
        {
            Debug.Log("실행");
            return;
        }

        // 문서 간트 체크리스트                                                  

        // 내 캐릭터에만 RtcSender, RtcReceiver 를 붙이자
        if(photonView.IsMine)
        {
            rtcSender = gameObject.AddComponent<RtcSender>();
            rtcReceiver = gameObject.AddComponent<RtcReceiver>();

            // rtcSender가 있는 경우
            if (GameMgr.instance.viewOn)
            {
                rtcSender.Setup();
            }
        }

        //플레이어의 photonView 를 GameMgr 에 알려주자
        GameMgr.instance.AddPlayer(photonView);
    }

    void Update()
    {
        
    }

    [PunRPC]
    void RpcCreateViewerPeer(int senderId, string strDesc)
    {
        rtcReceiver.CreatePeer(senderId, strDesc);
    }

    [PunRPC]
    void RpcSetRemoteDescription(int receiverId, string strDesc)
    {
        StartCoroutine(rtcSender.SetRemoteDescription(receiverId, strDesc));
    }

    [PunRPC]
    void RpcAddIceCandidate(int from, string strCandidate, bool isToSender)
    {
        if (isToSender)
        {
            rtcSender.AddIceCandidate(from, strCandidate);
        }
        else
        {
            rtcReceiver.AddIceCandidate(from, strCandidate);
        }
    }
}
