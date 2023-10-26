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

    [HideInInspector] public bool _isSit;
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
        #region �л��� ����
        // ���� ��ó�� �ְ�
        if (other.gameObject.CompareTag("Chair"))
        {
            //ChairInfo chairInfo = other.GetComponent<ChairInfo>();

            //_isSit = chairInfo.isSit;

            // ���� ������ ��ư�� Ŭ��������
            // ���ڿ� ���� �ɾ� ���� ������
            if (EventSystem.current.currentSelectedGameObject == Btn_Sit.gameObject && !_isSit)
            {

                // �ɴ� �ִϸ��̼ǽ����ϰ�
                anim.Play("Sit");

                photonView.RPC(nameof(animPlayRPC), RpcTarget.All, "Sit");

                // ��ġ�� ���� ��ġ��
                Character.transform.position =
                    new Vector3(other.transform.position.x, 0.2f, other.transform.position.z);
                Character.transform.forward = other.transform.right;

                // ��� �ɾҴ�.
                _isSit = true;
                //chairInfo.photonView.RPC("UpdateChairSitStatus", RpcTarget.AllBuffered, chairIndex, true);
            }

            // �ڸ����� �Ͼ��.
            else
            {
                if (characterMovement.moveSpeed != 0)
                {
                    Character.transform.position = new Vector3
                    (Character.transform.position.x, 0, Character.transform.position.z);

                    _isSit = false;
                }
            }
        }
        #endregion

        #region ����������
        // ���� ��ü�� �����������̰�
        // ���� �������̸�
        else if (other.gameObject.name == "Teacher Chair" && DataBase.instance.userInfo.isTeacher)
        {
            // �ɱ� ��ư�� ������ ��
            if (EventSystem.current.currentSelectedGameObject == Btn_Sit.gameObject && !_isSit)
            {
                anim.Play("Sit");

                photonView.RPC(nameof(animPlayRPC), RpcTarget.All, "Sit");

                Character.transform.position =
                        new Vector3(other.transform.position.x, 0.2f, other.transform.position.z);
                Character.transform.forward = Quaternion.Euler(0, -90, 0) * other.transform.right;

                _isSit = true;
            }

            else
            {
                if (characterMovement.moveSpeed != 0)
                {
                    Character.transform.position = new Vector3
                    (Character.transform.position.x, 0, Character.transform.position.z);

                    _isSit = false;
                }
            }
        }
        #endregion

        #region ������ ��ǻ��
        if (other.gameObject.name == "Teacher Computer")
        {
            StartStudy startStudy = other.gameObject.GetComponent<StartStudy>();

            if (photonView.IsMine)
            {
                characterMovement.CharacterCanvas.gameObject.SetActive(startStudy.enableCanvas);
            }
        }
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "GotoTeachersRoom" && DataBase.instance.userInfo.isTeacher)
        {
            //PhotonNetwork.LeaveRoom();
            NetworkManager.instance.ChangeRoom("4.TeachersRoomScene");
        }

        if (other.gameObject.name == "BackToClass")
        {
            //PhotonNetwork.LeaveRoom();
            NetworkManager.instance.ChangeRoom("4.ClassRoomScene");
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

        // �ɾ��ִٸ�
        if (photonView.IsMine && _isSit)
        {
            // TPScam�� ���������� canvas ����
            characterMovement.CharacterCanvas.gameObject.SetActive(isTPSCam);
        }
    }

    #endregion

    #region �ǻ� ���� ���
    private void OnCustomButtonClick()
    {
        if (photonView.IsMine) // ���� ������Ʈ�� �������� �ִ��� Ȯ��
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(1);
        }
    }
    #endregion
}
