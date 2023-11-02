using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

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
    private Button Btn_Focus, Btn_Normal,Btn_ShareCam;

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

        subMainCam = GameObject.Find("SubMainCam")?.GetComponent<Camera>();
        Btn_Focus = GameObject.Find("FocusButton")?.GetComponent<Button>();
        Btn_Normal = GameObject.Find("NormalButton")?.GetComponent<Button>();
        Btn_ShareCam = GameObject.Find("ShareButton")?.GetComponent<Button>();
    }

    private void Start()
    {
        Btn_Focus?.onClick.AddListener(() => OnFocusBtnClick());
        Btn_Normal?.onClick.AddListener(() => OnNormalBtnClick());
        Btn_ShareCam?.onClick.AddListener(() => OnShareButtonClick());
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Chair"))
        {
            HandleChairInteraction(other);
        }
        else if (other.gameObject.name == "Teacher Chair" && DataBase.instance.userInfo.isTeacher)
        {
            HandleTeacherChairInteraction(other);
        }
        else if (other.gameObject.name == "Teacher Computer")
        {
            HandleTeacherComputerInteraction(other);
        }
    }

    private void HandleChairInteraction(Collider chair)
    {
        if (EventSystem.current.currentSelectedGameObject == Btn_Sit.gameObject && !_isSit)
        {
            SitDown(chair);
        }
        else
        {
            StandUp();
        }
    }

    private void HandleTeacherChairInteraction(Collider chair)
    {
        if (EventSystem.current.currentSelectedGameObject == Btn_Sit.gameObject && !_isSit)
        {
            SitDownTeacher(chair);
        }
        else
        {
            StandUp();
        }
    }

    private void HandleTeacherComputerInteraction(Collider computer)
    {
        StartStudy startStudy = computer.GetComponent<StartStudy>();
        if (photonView.IsMine)
        {
            characterMovement.CharacterCanvas.gameObject.SetActive(startStudy.enableCanvas);
        }
    }

    private void SitDown(Collider chair)
    {
        PlaySitAnimation();
        SetCharacterPosition(chair.transform.position);
        SetCharacterForwardDirection(chair.transform.right);
        _isSit = true;
    }

    private void SitDownTeacher(Collider chair)
    {
        PlaySitAnimation();
        SetCharacterPosition(chair.transform.position);
        SetCharacterForwardDirection(Quaternion.Euler(0, -90, 0) * chair.transform.right);
        _isSit = true;
    }

    private void StandUp()
    {
        if (characterMovement.moveSpeed != 0)
        {
            SetCharacterYPosition(0);
            _isSit = false;
        }
    }

    private void PlaySitAnimation()
    {
        anim.Play("Sit");
        photonView.RPC(nameof(animPlayRPC), RpcTarget.All, "Sit");
    }

    private void SetCharacterPosition(Vector3 position)
    {
        Character.transform.position = new Vector3(position.x, 0.2f, position.z);
    }

    private void SetCharacterForwardDirection(Vector3 direction)
    {
        Character.transform.forward = direction;
    }

    private void SetCharacterYPosition(float y)
    {
        Vector3 position = Character.transform.position;
        Character.transform.position = new Vector3(position.x, y, position.z);
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

    // TPS랑 FPS 카메라 전환
    public void OnCameraButtonClick()
    {
        ToggleCameraMode();
        UpdateCameraSettings();
        UpdateCanvasSettings();
    }

    private void ToggleCameraMode()
    {
        isTPSCam = !isTPSCam;
        isDrawing = !isDrawing;
    }

    private void UpdateCameraSettings()
    {
        cameraSetting.TPS_Camera.gameObject.SetActive(isTPSCam);
        cameraSetting.FPS_Camera.gameObject.SetActive(!isTPSCam);
        if (!isTPSCam && !DataBase.instance.userInfo.isTeacher)
        {
            ConfigureSubMainCamera();
        }
        cameraSetting.FPS_Camera.depth = _isSit ? -1 : 1;
    }

    private void ConfigureSubMainCamera()
    {
        if (subMainCam == null) return;

        subMainCam.orthographicSize = 1.75f;
        subMainCam.transform.position = new Vector3(-6, 2.65f, -0.06f);
        subMainCam.transform.rotation = Quaternion.Euler(0, -90, 0);
    }

    private void UpdateCanvasSettings()
    {
        if (_isSit)
        {
            characterMovement.CharacterCanvas.gameObject.SetActive(isTPSCam);
        }
        else
        {
            characterMovement.CharacterCanvas.gameObject.SetActive(true);
        }
        characterMovement.SpareCanvas.gameObject.SetActive(!characterMovement.CharacterCanvas.gameObject.activeSelf);
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

    public void OnShareButtonClick()
    {
        photonView.RPC("SwitchAllStudentsCam", RpcTarget.Others);
    }

    [PunRPC]
    public void SwitchAllStudentsCam()
    {
        if (_isSit)
        {
            OnCameraButtonClick();
            print("22");
        }
    }
}
