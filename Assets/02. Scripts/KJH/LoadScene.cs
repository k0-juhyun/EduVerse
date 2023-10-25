using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LoadScene : MonoBehaviour
{
    private Vector3 spawnPos = new Vector3(-4, 0, 6);
    private Quaternion spawnRot = Quaternion.identity;

    private void Awake()
    {
        PhotonNetwork.Instantiate("Character", spawnPos, spawnRot);
    }
}
