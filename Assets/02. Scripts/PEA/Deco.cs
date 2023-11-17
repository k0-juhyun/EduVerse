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
    }

    private void OnClickDrawBtn()
    {
        print("click draw btn");
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
        }

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
