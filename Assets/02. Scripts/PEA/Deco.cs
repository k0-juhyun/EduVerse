using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class Deco : MonoBehaviour
{
    private Camera mainCam;

    public Button drawBtn;
    public Button decoBtn;
    public GameObject draw;
    public GameObject myDraws;
    public Button[] backBtn;
    public Camera drawCam;
    public RawImage drawPaper;     // 그림 그리는 판 이미지(그려지는 그림을 보여줌, 실제 그림은 큐브애 그려짐)
    public Rito.TexturePainter.TexturePaintTarget paintTarget;

    private void Start()
    {
        foreach (Button button in backBtn)
        {
            button.onClick.AddListener(OnClickBackBtn);
        }

        drawBtn.onClick.AddListener(OnClickDrawBtn);
        decoBtn.onClick.AddListener(OnClickDecoBtn);
        mainCam = Camera.main;

        SetRanderTexture();
    }

    private void OnClickDrawBtn()
    {
        drawBtn.gameObject.SetActive(false);
        decoBtn.gameObject.SetActive(false);
        draw.SetActive(true);

        mainCam.depth = -1;
        drawCam.gameObject.SetActive(true);
        mainCam.gameObject.SetActive(false);
    }

    private void OnClickDecoBtn()
    {
        print("click deco btn");
        drawBtn.gameObject.SetActive(false);
        decoBtn.gameObject.SetActive(false);
        myDraws.SetActive(true);
    }

    private void OnClickBackBtn()
    {
        drawBtn.gameObject.SetActive(true);
        decoBtn.gameObject.SetActive(true);
        draw.SetActive(false);
        myDraws.SetActive(false);

        DecorateClassRoom.instance.curSelectedDraw = null;

        if(mainCam.depth != 1)
        {
            mainCam.depth = 1;
            drawCam.gameObject.SetActive(false);
            mainCam.gameObject.SetActive(true);
            paintTarget.InitRenderTexture();
            SetRanderTexture();
        }

    }

    // 그림 그리는 판 이미지에 큐브 렌더텍스쳐(그림 그려지는 텍스쳐) 넣기
    public void SetRanderTexture()
    {
        drawPaper.texture = paintTarget.renderTexture;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PhotonView>(out PhotonView photonView))
        {
            if (photonView.IsMine)
            {
                drawBtn.gameObject.SetActive(true);
                decoBtn.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PhotonView>(out PhotonView photonView))
        {
            if (photonView.IsMine)
            {
                drawBtn.gameObject.SetActive(false);
                decoBtn.gameObject.SetActive(false);
            }
        }
    }
}
