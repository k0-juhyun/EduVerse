using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackBoard : MonoBehaviour, IPunObservable
{
    public RenderTexture renderTexture; // ���� �ؽ�ó
    RawImage rawImage;
    Texture2D texture;

    void Start()
    {

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

        }
        else
        {

        }
    }
}
