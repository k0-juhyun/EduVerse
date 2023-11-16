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
    public Button Btn_Camera, Btn_Custom, Btn_Greet;
    private Button Btn_Focus, Btn_Normal, Btn_ShareCam;

    private Animator anim;

    private CharacterMovement characterMovement;
    private CameraSetting cameraSetting;

    public bool _isSit;
    [HideInInspector] public bool isTPSCam = true;
    [HideInInspector] public bool isDrawing = false;

    public Text myNickNameTxt;
    private string myNickName;

    public Camera Cam;

    Camera subMainCam;
    Scene scene;

    private void Awake()
    {
        anim = Character.GetComponent<Animator>();
        characterMovement = GetComponent<CharacterMovement>();
        cameraSetting = GetComponentInChildren<CameraSetting>();

        Btn_Camera.onClick.AddListener(() => OnCameraButtonClick());
        Btn_Custom.onClick.AddListener(() => OnCustomButtonClick());
        Btn_Greet.onClick.AddListener(() => OnGreetBtnClick());

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

        if(photonView.IsMine)
            myNickNameTxt.text = PhotonNetwork.LocalPlayer.NickName;
        else
            myNickNameTxt.text = photonView.Owner.NickName;

        if (SceneManager.GetActiveScene().name == "5.GroundScene")
        {
            Cam.gameObject.transform.localPosition = new Vector3(0, 16, -16);
            Cam.gameObject.transform.localRotation = Quaternion.Euler(30, 0, 0);

        }
    }

    private void Update()
    {
        if (photonView.IsMine && Cam != null)
            myNickNameTxt.transform.LookAt(myNickNameTxt.transform.position + Cam.transform.rotation * Vector3.forward, 
                Cam.transform.rotation * Vector3.up);

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Chair"))
        {
            HandleChairInteraction(other);
        }
        else if (other.gameObject.name == "Teacher Chair" && DataBase.instance.user.isTeacher)
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
        SetCharacterForwardDirection(chair.transform.forward * -1);
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
        Character.transform.position = new Vector3(position.x, 0.4f, position.z);
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
        if (photonView.IsMine && other.gameObject.name == "GotoTeahcersRoom" && DataBase.instance.user.isTeacher)
        {
            PhotonNetwork.JoinLobby();
            PhotonNetwork.LoadLevel("3.TeacherMyPage");
            //NetworkManager.instance.ChangeRoom("3.TeacherMyPage");
            //StartCoroutine(ILeaveRoomAndLoadScene("3.TeacherMyPage"));
        }

        if (other.gameObject.name == "BackToClass")
        {
            //PhotonNetwork.LeaveRoom();
            NetworkManager.instance.ChangeRoom("4.ClassRoomScene");
        }

        if(photonView.IsMine && other.gameObject.name == "GotoGround")
        {
            NetworkManager.instance.ChangeRoom("5.GroundScene");
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
        photonView.RPC("TrySitDownRPC", RpcTarget.Others);
        photonView.RPC("SetIsSitRPC", RpcTarget.Others);

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
        if (!isTPSCam && !DataBase.instance.user.isTeacher)
        {
            ConfigureSubMainCamera();
        }
        cameraSetting.FPS_Camera.depth = _isSit ? -1 : 1;
    }

    private void ConfigureSubMainCamera()
    {
        if (subMainCam == null) return;

        subMainCam.orthographicSize = 1.9f;
        subMainCam.transform.position = new Vector3(-6, 2.525f, -0.0f);
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
        }
    }

    [PunRPC]
    public void TrySitDownRPC()
    {
        // 선생님 캐릭터가 아니라면, 앉습니다.
        if (!DataBase.instance.user.isTeacher)
        {
            Collider nearestChair = FindNearestChair();
            if (nearestChair != null)
            {
                SitDown(nearestChair);
                _isSit = true;
            }
        }
    }

    private Collider FindNearestChair()
    {
        // TODO: 가장 가까운 'Chair' 태그를 가진 의자를 찾아서 그 Collider를 반환해야 합니다.
        // 의자들을 리스트로 관리하고 있지 않다면, 여기서 FindGameObjectsWithTag 등을 사용하여
        // 가장 가까운 의자를 찾는 로직을 구현해야 합니다.
        // 아래는 간단한 예시 코드입니다:
        GameObject[] chairs = GameObject.FindGameObjectsWithTag("Chair");
        GameObject nearestChair = null;
        float closestDistance = Mathf.Infinity;
        Vector3 position = Character.transform.position;

        foreach (GameObject chair in chairs)
        {
            float distance = (chair.transform.position - position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestChair = chair;
            }
        }

        return nearestChair?.GetComponent<Collider>();
    }

    [PunRPC]
    public void SetIsSitRPC()
    {
        print("dd");
        _isSit = true;
    }

    public void OnGreetBtnClick()
    {
        anim.Play("GreetR");
        photonView.RPC(nameof(animPlayRPC), RpcTarget.All, "GreetR");
        StartCoroutine(WaitForAnimation(anim, "GreetR"));
    }

    private IEnumerator WaitForAnimation(Animator animator, string stateName)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) ||
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        anim.Play("GreetL");
        photonView.RPC(nameof(animPlayRPC), RpcTarget.All, "GreetL");
    }
}