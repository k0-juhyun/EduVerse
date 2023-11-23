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
        // ��ư�� ���� ������ ��ٸ�
        yield return new WaitUntil(() => isSitButtonPressed);

        // ��ư�� ������
        isSitButtonPressed = false;
    }
}
