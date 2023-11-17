using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Unity.VisualScripting;

public class DecoItem : MonoBehaviourPun
{
    Texture2D texture;
    //PhotonView view;
    void Awake()
    {
       //if(view == null)
       // {
       //     this.AddComponent<PhotonView>();
       // }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetDraw(Texture2D draw)
    {
        photonView.RPC(nameof(SetDrawRPC), RpcTarget.AllBuffered, draw.EncodeToPNG());
    }

    [PunRPC]
    private void SetDrawRPC(byte[] bytes)
    {
        texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        texture.Apply();
        GetComponent<RawImage>().enabled = true;
        GetComponent<RawImage>().texture = texture;
    }
}
