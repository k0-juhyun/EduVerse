using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairHandler : MonoBehaviour
{
    private bool isSitButtonPressed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PhotonView>(out PhotonView photonView))
        {
            if (photonView.IsMine)
            {

            }
        }
    }


    private IEnumerator ICheckClickSitButton(Collider other)
    {
        // 버튼이 눌릴 때까지 기다림
        yield return new WaitUntil(() => isSitButtonPressed);

        // 버튼이 눌리면
        isSitButtonPressed = false;
    }
}
