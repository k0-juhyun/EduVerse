using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Photon.Pun;

// 1. ���ڿ� �ɱ�
// - �����Űâ�� �ִ� ���� ������Ʈ�� �±׸� �̿��ؼ� ã�Ƽ�
// - ���� ����Ʈ�� ���� ����Ʈ�� �ְ�
// - ����Ʈ�� �ִ� ���� ������Ʈ �߿���
// - ĳ���Ϳ� ���� ����� ���ڸ� ã�Ƽ�
// - �ɱ� ��ư�� ������ �ɰ� �Ѵ�.
// - �ٸ� ����� �ɾ��ִ� ���ڴ� ���� ���Ѵ�.

public class CharacterInteraction : MonoBehaviourPun
{
    [Header("ĳ����")]
    public GameObject Character;
    [Space]
    [Header("��ư")]
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

    #region ���� �ɱ� ���
    private void OnTriggerStay(Collider other)
    {
        // ���� ��ó�� �ְ�
        if (other.gameObject.CompareTag("Chair"))
        {
            ChairInfo chairInfo = other.GetComponent<ChairInfo>();

            bool isSit = chairInfo.isSit;

            // ���� ������ ��ư�� Ŭ��������
            if (EventSystem.current.currentSelectedGameObject == Btn_Sit.gameObject)
            {
                // ���ڿ� ���� �ɾ� ���� ������
                // isFull = false �̸� ����ġ�� ���� �ɴ� �ִϸ��̼� ����
                if (!isSit)
                {
                    // �ɴ� �ִϸ��̼ǽ����ϰ�
                    anim.Play("Sit");

                    photonView.RPC(nameof(animPlayRPC), RpcTarget.All, "Sit");

                    // ��ġ�� ���� ��ġ��
                    Character.transform.position =
                        new Vector3(other.transform.position.x, 0.2f, other.transform.position.z);
                    Character.transform.forward = other.transform.right;

                    // ��� �ɾҴ�.
                    isSit = true;
                    //chairInfo.photonView.RPC("UpdateChairSitStatus", RpcTarget.AllBuffered, chairIndex, true);
                }
            }

            // �ڸ����� �Ͼ��.
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

    #region ī�޶� ��ȯ ���
    // TPS�� FPS ī�޶� ��ȯ
    public void OnCameraButtonClick()
    {
        isTPSCam = !isTPSCam;
    }
    #endregion

    #region �ǻ� ���� ���
    private void OnCustomButtonClick()
    {
        if (photonView.IsMine) // ���� ������Ʈ�� �������� �ִ��� Ȯ��
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    #endregion
}
