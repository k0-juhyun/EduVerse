using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairInfo : MonoBehaviourPun
{
    // 의자에 앉아있는지 여부
    public bool isFull;

    private void Start()
    {
        // 초기 상태 설정
        isFull = false;
    }

    [PunRPC]
    public void SetIsFull(bool value)
    {
        isFull = value;
    }
}
