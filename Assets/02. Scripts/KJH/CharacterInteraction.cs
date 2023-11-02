using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

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

    #region 의자 앉기 기능
    private void OnTriggerStay(Collider other)
    {
        #region 학색용 의자
        // 의자 근처에 있고
        if (other.gameObject.CompareTag("Chair"))
        {
            //ChairInfo chairInfo = other.GetComponent<ChairInfo>();

            //_isSit = chairInfo.isSit;

            // 범위 내에서 버튼을 클릭했을때
            // 의자에 누가 앉아 있지 않으면
            if (EventSystem.current.currentSelectedGameObject == Btn_Sit.gameObject && !_isSit)
            {
                // 앉는 애니메이션실행하고
                anim.Play("Sit");

                photonView.RPC(nameof(animPlayRPC), RpcTarget.All, "Sit");

                // 위치를 의자 위치로
                Character.transform.position =
                    new Vector3(other.transform.position.x, 0.2f, other.transform.position.z);
                Character.transform.forward = other.transform.right;

                // 사람 앉았다.
                _isSit = true;
                //chairInfo.photonView.RPC("UpdateChairSitStatus", RpcTarget.AllBuffered, chairIndex, true);
            }

            // 자리에서 일어났다.
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

        #region 선생님의자
        // 접근 객체가 선생님의자이고
        // 내가 선생님이면
        else if (other.gameObject.name == "Teacher Chair" && DataBase.instance.userInfo.isTeacher)
        {
            // 앉기 버튼을 눌렀을 때
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

        #region 선생님 컴퓨터
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
        print("앉기");
    }

    public void OnNormalBtnClick()
    {
        photonView.RPC("animPlayRPC", RpcTarget.Others, "Idle");
        print("차렷");
    }


    #region 카메라 전환 기능
    // TPS랑 FPS 카메라 전환
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
                // 앉아 있을 때 카메라를 전환하면 캔버스의 활성 상태를 바꿉니다.
                characterMovement.CharacterCanvas.gameObject.SetActive(isTPSCam);
            }
            else
            {
                // 앉아 있지 않을 때는 캔버스를 항상 활성화합니다.
                characterMovement.CharacterCanvas.gameObject.SetActive(true);
            }

            // FPS 카메라 설정
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


    #region 의상 변경 기능
    private void OnCustomButtonClick()
    {
        if (photonView.IsMine)
        {
            NetworkManager.instance.isCustom = true;
            NetworkManager.instance.enableChoose = true;
            PhotonNetwork.LeaveRoom();
            print("커스텀버튼");
        }
    }
    #endregion
}
