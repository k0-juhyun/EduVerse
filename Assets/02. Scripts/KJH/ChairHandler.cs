using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChairHandler : MonoBehaviour
{
    public Button SitButton; // 앉기 버튼 참조
    private bool isSitButtonPressed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PhotonView>(out PhotonView photonView))
        {
            if (photonView.IsMine)
            {
                // 플레이어가 이 의자 근처에 있을 때 앉기 버튼 활성화
                SitButton.gameObject.SetActive(true);

                // 버튼 클릭 대기 코루틴 시작
                StartCoroutine(ICheckClickSitButton(other));
            }
        }
    }

    private IEnumerator ICheckClickSitButton(Collider other)
    {
        // 버튼이 눌릴 때까지 기다림
        yield return new WaitUntil(() => isSitButtonPressed);

        // 버튼이 눌리면
        SitButton.gameObject.SetActive(false); // 버튼 비활성화

        // 앉기 로직 구현
        SitDown(other.gameObject);

        isSitButtonPressed = false;
    }

    private void SitDown(GameObject player)
    {
        // 플레이어를 해당 의자에 앉히는 로직 구현
        player.transform.position = this.transform.position;
    }
}

