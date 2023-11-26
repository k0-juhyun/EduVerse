using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public static GameMgr instance;

    //모든 Player 을 담을 변수
    public Dictionary<int, PhotonView> allPlayerPv = new Dictionary<int, PhotonView>();

    //내 Player 를 담을 변수 
    public PhotonView myPlayerPv;

    //캠 화면을 보여줄 UI Prefab
    public GameObject camViewerFactory;

    //나의 WebCam 화면
    GameObject myWebCamView;

    //공유 하는 사람들의 WebCam 화면
    List<RawImage> senderWebCamView = new List<RawImage>();

    // 공유되고 있는지 체크
    public bool viewOn = false;

    public bool isPlayerTPSCamOn = true;


    private void Awake()
    {
        instance = this;
    }


    void Start()
    {

    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    OnClickShare();
        //}
    }

    public void OnClickShare()
    {
        StartCoroutine(IOnClickShare(1));
    }

    private IEnumerator IOnClickShare(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log(myPlayerPv);
        //나의 RtcSender 로 공유를 시작하게 한다
        if (!viewOn)
            myPlayerPv.GetComponent<RtcSender>().Setup();

        // 화면 공유 시작중.
        viewOn = true;
    }

    public void AddPlayer(PhotonView pv)
    {
        allPlayerPv[pv.ViewID] = pv;

        //내 photonView 를 저장한다 
        if (pv.IsMine)
        {
            myPlayerPv = pv;
        }
        else
        {
            //내 photonView 가 아니면 내 화면을 보는 애들이다.
            //화면 공유 받을 view id 를 가지고 있자.
            myPlayerPv.GetComponent<RtcSender>().AddReceiverList(pv.ViewID);
        }
    }

    public void CreateMyWebCamView(Texture texture)
    {
        // Canvas 를 찾아서 RawImage 하나 만들고 자식으로 설정 
        Transform trCanvas = GameObject.Find("BlackBoard").transform;
        myWebCamView = Instantiate(camViewerFactory, trCanvas);

        // 위치를 화면의 왼쪽 상단으로 설정 
        RectTransform rt = myWebCamView.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero;

        // RawImage 의 texture 에 WebCam 화면이 보이게 설정 
        RawImage screen = myWebCamView.GetComponent<RawImage>();
        screen.texture = texture;
    }

    public RawImage AddScreen()
    {
        //공유 받은 화면을 그릴 RawIamge 만들자
        Transform trCanvas = GameObject.Find("BlackBoard").transform;
        GameObject goScreen = Instantiate(camViewerFactory, trCanvas);
        RawImage screen = goScreen.GetComponent<RawImage>();
        senderWebCamView.Add(screen);
        
        RectTransform rt = goScreen.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero;

        //공유 하는 사람들의 인원수에 맞춰서 화면을 배치하자 
        //Vector2 pos = Vector2.zero;

        ////RawImage 가 현재 화면에 들어갈 수 있는 가로 갯수 
        //int hCount = Screen.width / 300;

        ////x 값
        //pos.x = (senderWebCamView.Count % hCount) * 10;

        ////y 값
        //pos.y = -(senderWebCamView.Count / hCount) * 10;

        ////계산된 값으로 위치시키자 
        //RectTransform rt = goScreen.GetComponent<RectTransform>();
        //rt.anchoredPosition = pos;

        return screen;
    }

    public void CreateViewerPeer(int receiverId, int senderId, string strDesc)
    {
        //공유 받을 사람의 PhotonView 를 가져오자
        PhotonView receiver = allPlayerPv[receiverId];

        //공유 받을 사람에게만 Peer 를 만들라고 요청
        receiver.RPC("RpcCreateViewerPeer", receiver.Owner, senderId, strDesc);
    }

    public void SetRemoteDescription(int receiverId, int senderId, string strDesc)
    {
        //공유 시작한 사람의 PhotonView 를 가져오자
        PhotonView sender = allPlayerPv[senderId];

        //공유 시작한 사람에게만 SetRemoteDescription 을 해라
        sender.RPC("RpcSetRemoteDescription", sender.Owner, receiverId, strDesc);
    }

    public void AddIceCandidate(int to, int from, string strCandidate, bool isToSender)
    {
        PhotonView player = allPlayerPv[to];
        player.RPC("RpcAddIceCandidate", player.Owner, from, strCandidate, isToSender);
    }
}
