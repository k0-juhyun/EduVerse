using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

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
    private Button Btn_Focus;
    private Button Btn_Normal;

    private Animator anim;

    private CharacterMovement characterMovement;
    private CameraSetting cameraSetting;

    [HideInInspector] public bool _isSit;
    [HideInInspector] public bool isTPSCam = true;
    [HideInInspector] public bool isDrawing = false;

    Camera subMainCam;
    Scene scene;

    private void Awake()
    {
        anim = Character.GetComponent<Animator>();
        characterMovement = GetComponent<CharacterMovement>();
        cameraSetting = GetComponentInChildren<CameraSetting>();

        Btn_Camera.onClick.AddListener(() => OnCameraButtonClick());
        Btn_Custom.onClick.AddListener(() => OnCustomButtonClick());

        if (scene.buildIndex == 4)
            subMainCam = GameObject.Find("SubMainCam").GetComponent<Camera>();

        Btn_Focus = GameObject.Find("FocusButton").GetComponent<Button>();
        Btn_Normal = GameObject.Find("NormalButton").GetComponent<Button>();
        print(Btn_Focus.gameObject.name);
    }

    private void Start()
    {
        Btn_Focus.onClick.AddListener(OnFocusBtnClick);
        Btn_Normal.onClick.AddListener(OnNormalBtnClick);
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
        if (photonView.IsMine && other.gameObject.name == "GotoTeachersRoom" && DataBase.instance.userInfo.isTeacher)
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

    public void OnFocusBtnClick()
    {
        photonView.RPC("animPlayRPC", RpcTarget.Others, "Sit");
        print("�ɱ�");
    }

    public void OnNormalBtnClick()
    {
        photonView.RPC("animPlayRPC", RpcTarget.Others, "Idle");
        print("����");
    }


    #region ī�޶� ��ȯ ���
    // TPS�� FPS ī�޶� ��ȯ
    public void OnCameraButtonClick()
    {
        isTPSCam = !isTPSCam;
        isDrawing = !isDrawing;

        if (photonView.IsMine)
        {
            cameraSetting.TPS_Camera.gameObject.SetActive(isTPSCam);
            cameraSetting.FPS_Camera.gameObject.SetActive(!cameraSetting.TPS_Camera.gameObject.activeSelf);
            if (_isSit)
            {
                // �ɾ� ���� �� ī�޶� ��ȯ�ϸ� ĵ������ Ȱ�� ���¸� �ٲߴϴ�.
                characterMovement.CharacterCanvas.gameObject.SetActive(isTPSCam);
            }
            else
            {
                // �ɾ� ���� ���� ���� ĵ������ �׻� Ȱ��ȭ�մϴ�.
                characterMovement.CharacterCanvas.gameObject.SetActive(true);
            }

            // FPS ī�޶� ����
            if (!isTPSCam && !DataBase.instance.userInfo.isTeacher)
            {
                if (subMainCam != null)
                {
                    subMainCam.orthographicSize = 1.75f;
                    subMainCam.transform.position = new Vector3(-6, 2.65f, -0.06f);
                    subMainCam.transform.rotation = Quaternion.Euler(0, -90, 0);
                }
            }

            characterMovement.SpareCanvas.gameObject.SetActive
                (characterMovement.CharacterCanvas.gameObject.activeSelf == false);
            cameraSetting.FPS_Camera.depth = _isSit ? -1 : 1;
        }
    }

    public void OnXButtonClick()
    {
        if (photonView.IsMine)
        {
            characterMovement.SpareCanvas.gameObject.SetActive(false);
            characterMovement.CharacterCanvas.gameObject.SetActive(true);
            cameraSetting.FPS_Camera.gameObject.SetActive(false);
            isTPSCam = true;
            isDrawing = false;
            cameraSetting.FPS_Camera.depth = 1;
        }
    }
    #endregion


    #region �ǻ� ���� ���
    private void OnCustomButtonClick()
    {
        if (photonView.IsMine)
        {
            NetworkManager.instance.isCustom = true;
            NetworkManager.instance.enableChoose = true;
            PhotonNetwork.LeaveRoom();
            print("Ŀ���ҹ�ư");
        }
    }
    #endregion
}
