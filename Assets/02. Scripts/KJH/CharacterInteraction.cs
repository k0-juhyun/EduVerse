using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Photon.Pun;

// 1. 의자에 앉기
// - 히어라키창에 있는 의자 오브젝트를 태그를 이용해서 찾아서
// - 의자 리스트를 만들어서 리스트에 넣고
// - 리스트에 있는 의자 오브젝트 중에서
// - 캐릭터와 가장 가까운 의자를 찾아서
// - 앉기 버튼을 누르면 앉게 한다.
// - 다른 사람이 앉아있는 의자는 앉지 못한다.

public class CharacterInteraction : MonoBehaviourPun
{
    [Header("캐릭터")]
    public GameObject Character;
    [Space]
    [Header("버튼")]
    public Button Btn_Sit;
    public Button Btn_Camera;
    public Button Btn_Custom;

    private Animator anim;

    private CharacterMovement characterMovement;

    [HideInInspector] public bool isTPSCam = true;

    private void Awake()
    {
        anim = Character.GetComponent<Animator>();
        characterMovement = GetComponent<CharacterMovement>();

        Btn_Camera.onClick.AddListener(() => OnCameraButtonClick());
        Btn_Custom.onClick.AddListener(() => OnCustomButtonClick());
    }

    #region 의자 앉기 기능
    private void OnTriggerStay(Collider other)
    {
        // 의자 근처에 있고
        if (other.gameObject.CompareTag("Chair"))
        {
            ChairInfo chairInfo = other.GetComponent<ChairInfo>();

            bool isSit = chairInfo.isSit;

            // 범위 내에서 버튼을 클릭했을때
            if (EventSystem.current.currentSelectedGameObject == Btn_Sit.gameObject)
            {
                // 의자에 누가 앉아 있지 않으면
                // isFull = false 이면 그위치로 가서 앉는 애니메이션 실행
                if (!isSit)
                {
                    // 앉는 애니메이션실행하고
                    anim.Play("Sit");

                    photonView.RPC(nameof(animPlayRPC), RpcTarget.All, "Sit");

                    // 위치를 의자 위치로
                    Character.transform.position =
                        new Vector3(other.transform.position.x, 0.2f, other.transform.position.z);
                    Character.transform.forward = other.transform.right;

                    // 사람 앉았다.
                    isSit = true;
                    //chairInfo.photonView.RPC("UpdateChairSitStatus", RpcTarget.AllBuffered, chairIndex, true);
                }
            }

            // 자리에서 일어났다.
            else
            {
                if (characterMovement.moveSpeed != 0)
                {
                    Character.transform.position = new Vector3
                    (Character.transform.position.x, 0, Character.transform.position.z);

                    isSit = false;
                    //chairInfo.photonView.RPC("UpdateChairSitStatus", RpcTarget.AllBuffered, chairIndex, false);
                    // chairInfo.isFull = false;
                }
            }
        }
    }

    [PunRPC]
    private void animPlayRPC(string animation)
    {
        anim.Play(animation);
    }

    #endregion

    #region 카메라 전환 기능
    // TPS랑 FPS 카메라 전환
    public void OnCameraButtonClick()
    {
        isTPSCam = !isTPSCam;
    }
    #endregion

    #region 의상 변경 기능
    private void OnCustomButtonClick()
    {
        if (photonView.IsMine) // 현재 오브젝트의 소유권이 있는지 확인
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    #endregion
}
