using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Deco : MonoBehaviour
{
    public Button drawBtn;
    public Button decoBtn;
    public GameObject draw;
    public GameObject myDraws;
    public Button[] backBtn;
    public Camera drawCam;

    private void Start()
    {
        foreach (Button button in backBtn)
        {
            button.onClick.AddListener(OnClickBackBtn);
        }

        drawBtn.onClick.AddListener(OnClickDrawBtn);
        decoBtn.onClick.AddListener(OnClickDecoBtn);
    }

    private void OnClickDrawBtn()
    {
        print("click draw btn");
        drawBtn.gameObject.SetActive(false);
        decoBtn.gameObject.SetActive(false);
        draw.SetActive(true);

        drawCam.gameObject.SetActive(true);
        Camera.main.gameObject.SetActive(false);
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
        drawCam.gameObject.SetActive(false);
        Camera.main.gameObject.SetActive(true);
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
