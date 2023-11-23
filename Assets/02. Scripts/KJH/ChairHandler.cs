using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChairHandler : MonoBehaviour
{
    public Button SitButton; // �ɱ� ��ư ����
    private bool isSitButtonPressed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PhotonView>(out PhotonView photonView))
        {
            if (photonView.IsMine)
            {
                // �÷��̾ �� ���� ��ó�� ���� �� �ɱ� ��ư Ȱ��ȭ
                SitButton.gameObject.SetActive(true);

                // ��ư Ŭ�� ��� �ڷ�ƾ ����
                StartCoroutine(ICheckClickSitButton(other));
            }
        }
    }

    private IEnumerator ICheckClickSitButton(Collider other)
    {
        // ��ư�� ���� ������ ��ٸ�
        yield return new WaitUntil(() => isSitButtonPressed);

        // ��ư�� ������
        SitButton.gameObject.SetActive(false); // ��ư ��Ȱ��ȭ

        // �ɱ� ���� ����
        SitDown(other.gameObject);

        isSitButtonPressed = false;
    }

    private void SitDown(GameObject player)
    {
        // �÷��̾ �ش� ���ڿ� ������ ���� ����
        player.transform.position = this.transform.position;
    }
}

