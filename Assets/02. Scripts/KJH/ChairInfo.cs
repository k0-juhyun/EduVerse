using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairInfo : MonoBehaviourPun
{
    // ���ڿ� �ɾ��ִ��� ����
    public bool isFull;

    private void Start()
    {
        // �ʱ� ���� ����
        isFull = false;
    }

    [PunRPC]
    public void SetIsFull(bool value)
    {
        isFull = value;
    }
}
