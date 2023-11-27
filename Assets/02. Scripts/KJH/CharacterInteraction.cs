using DG.Tweening;
using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// - �����Űâ�� �ִ� ���� ������Ʈ�� �±׸� �̿��ؼ� ã�Ƽ�
// - ���� ����Ʈ�� ���� ����Ʈ�� �ְ�
// - ����Ʈ�� �ִ� ���� ������Ʈ �߿���
// - ĳ���Ϳ� ���� ����� ���ڸ� ã�Ƽ�
// - �ɱ� ��ư�� ������ �ɰ� �Ѵ�.
// - �ٸ� ����� �ɾ��ִ� ���ڴ� ���� ���Ѵ�.

public class CharacterInteraction : MonoBehaviourPun
{
    private bool isLesson = false;

    [Header("ĳ����")]
    public GameObject Character;
    [Space]
    [Header("��ư")]
    public Button Btn_Sit;
    public Button Btn_Camera, Btn_Custom, Btn_Greet;
    private Button Btn_Focus, Btn_ShareCam;
    public Button Btn_Quiz;


    private Animator anim;

    private CharacterMovement characterMovement;
    private CameraSetting cameraSetting;
    private TeacherInteraction teacherInteraction;
    private Rigidbody rb;
    private TriggerHandler triggerHandler;

    [HideInInspector] public bool _isSit;

    public delegate void SitStatusChanged(bool isSitting);
    public event SitStatusChanged OnSitStatusChanged;

    private bool isOpenUI;
    [HideInInspector] public bool isTPSCam = true;
    [HideInInspector] public bool isDrawing = false;
    private bool isLeaving;

    public TMP_Text myNickNameTxt;

    private string myNickName;

    public RectTransform characterUI;
    public GameObject OpenUI;

    public Camera Cam;
    Camera subMainCam;
    Scene scene;

    private GameObject currentChair = null;
    public event Action OnSitDown;

    private void Awake()
    {
        anim = Character.GetComponent<Animator>();
        characterMovement = GetComponent<CharacterMovement>();
        cameraSetting = GetComponentInChildren<CameraSetting>();
        triggerHandler = GetComponentInChildren<TriggerHandler>();

        Btn_Camera.onClick.AddListener(() => OnCameraButtonClick());
        Btn_Custom.onClick.AddListener(() => OnCustomButtonClick());
        Btn_Greet.onClick.AddListener(() => OnGreetBtnClick());

        subMainCam = GameObject.Find("SubMainCam")?.GetComponent<Camera>();
        Btn_Focus = GameObject.Find("FocusButton")?.GetComponent<Button>();
        Btn_ShareCam = GameObject.Find("ShareButton")?.GetComponent<Button>();
    }

    private void Start()
    {
        Btn_Focus?.onClick.AddListener(() => OnFocusBtnClick());
        //Btn_Focus?.onClick.AddListener(() => OnShareButtonClick());
        Btn_ShareCam?.onClick.AddListener(() => OnShareButtonClick());
        rb = GetComponentInChildren<Rigidbody>();

        if (photonView.IsMine)
            myNickNameTxt.text = PhotonNetwork.LocalPlayer.NickName;
        else
            myNickNameTxt.text = photonView.Owner.NickName;

        if (SceneManager.GetActiveScene().name == "5.GroundScene")
        {
            Cam.gameObject.transform.localPosition = new Vector3(0, 16, -16);
            Cam.gameObject.transform.localRotation = Quaternion.Euler(30, 0, 0);
        }

        teacherInteraction = GetComponentInChildren<TeacherInteraction>();
        if(DataBase.instance.userInfo.isteacher == false)
            teacherInteraction.quizButton.SetActive(false);

        AdjustUIHeight();
    }

    private void Update()
    {
       // if (photonView.IsMine && Cam != null)
            myNickNameTxt.transform.LookAt(myNickNameTxt.transform.position + Camera.main.transform.rotation * Vector3.forward,
                Camera.main.transform.rotation * Vector3.up);
    }

    public void HandleTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Chair"))
        {
            HandleChairInteraction(other);
        }
        else if (other.gameObject.name == "Teacher Chair" && DataBase.instance.user.isTeacher)
        {
            HandleTeacherChairInteraction(other);
        }
        if (other.gameObject.name == "Teacher Computer")
        {
            HandleTeacherComputerInteraction(other);
        }
    }

    public void SetSitStatus(bool sitStatus)
    {
        _isSit = sitStatus;
        OnSitStatusChanged?.Invoke(_isSit);

        if (_isSit)
        {
            OnSitDown?.Invoke(); // ���� ���°� �Ǹ� �̺�Ʈ �߻�
        }
    }

    public void HandleTriggerEnter(Collider other)
    {
        if (photonView.IsMine && other.gameObject.name == "GotoTeachersRoom"
            && DataBase.instance.user.isTeacher)
        {
            if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Leaving)
                return;

            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel("3.TeacherMyPage");
        }

        if (other.gameObject.name == "BackToClass")
        {
            NetworkManager.instance.ChangeRoom("4.ClassRoomScene");
        }

        if (photonView.IsMine && other.gameObject.name == "GotoGround")
        {
            NetworkManager.instance.ChangeRoom("5.GroundScene");
        }

    }

    private IEnumerator WaitAndHandleChairInteraction(Collider chair)
    {
        // Btn_Sit ��ư�� ���� ������ ���
        yield return new WaitUntil(() => EventSystem.current.currentSelectedGameObject == Btn_Sit?.gameObject);

        // ��ư�� ������ �ش� ���� ����
        if (!_isSit)
        {
            SitDown(chair);
            _isSit = true;
        }
        else
        {
            StandUp();
        }
    }

    private void HandleChairInteraction(Collider chair)
    {
        Btn_Sit = triggerHandler.sitBtn;
        if (EventSystem.current.currentSelectedGameObject == Btn_Sit.gameObject)
        {
            if (!_isSit)
            {
                SitDown(chair);
                _isSit = true;
            }
        }
        else
        {
            StandUp();
        }
    }

    private void HandleTeacherChairInteraction(Collider chair)
    {
        Btn_Sit = triggerHandler.sitBtn;
        if (EventSystem.current.currentSelectedGameObject == Btn_Sit.gameObject)
        {
            if (!_isSit)
            {
                SitDownTeacher(chair);
                _isSit = true;
            }
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
            print(startStudy.enableCanvas);
        }
    }

    public void SitDown(Collider chair)
    {
        _isSit = true;
        Vector3 position = new Vector3(chair.transform.position.x, 0.4f, chair.transform.position.z);
        Quaternion rotation = Quaternion.LookRotation(chair.transform.forward * -1);

        SetCharacterPosition(chair.transform.position);
        SetCharacterForwardDirection(chair.transform.forward * -1);

        rb.useGravity = false;
        rb.isKinematic = true;

        photonView.RPC("SitDownRPC", RpcTarget.Others, position, rotation);
        PlaySitAnimation(); // ���� �÷��̾��� �ִϸ��̼� ����
        print("���");
        currentChair = chair.gameObject;
        SetSitStatus(true);
        StudentChairSitHandler chairHandler = currentChair.GetComponent<StudentChairSitHandler>();
        if (chairHandler != null)
        {
            photonView.RPC("MarkChairAsOccupied", RpcTarget.All, currentChair.name);
        }
    }

    [PunRPC]
    public void MarkChairAsOccupied(string chairName)
    {
        GameObject chairObject = GameObject.Find(chairName);
        if (chairObject != null)
        {
            StudentChairSitHandler chairHandler = chairObject.GetComponent<StudentChairSitHandler>();
            if (chairHandler != null)
            {
                chairHandler.isOccupied = true;
            }
        }
    }

    [PunRPC]
    public void MarkChairAsUnoccupied(string chairName)
    {
        GameObject chairObject = GameObject.Find(chairName);
        if (chairObject != null)
        {
            StudentChairSitHandler chairHandler = chairObject.GetComponent<StudentChairSitHandler>();
            if (chairHandler != null)
            {
                chairHandler.isOccupied = false;
            }
        }
    }

    [PunRPC]
    public void SitDownRPC(Vector3 position, Quaternion rotation)
    {
        Character.transform.position = position;
        Character.transform.rotation = rotation;
        PlaySitAnimation();
    }

    public void SitDownTeacher(Collider chair)
    {
        _isSit = true;
        Vector3 position = new Vector3(chair.transform.position.x, 0.4f, chair.transform.position.z);
        Quaternion rotation = Quaternion.LookRotation(Quaternion.Euler(0, -90, 0) * chair.transform.right);

        PlaySitAnimation();
        SetCharacterPosition(chair.transform.position);
        SetCharacterForwardDirection(Quaternion.Euler(0, -90, 0) * chair.transform.right);
        photonView.RPC("SitDownRPC", RpcTarget.Others, position, rotation);
    }

    private void StandUp()
    {
        if (characterMovement.moveSpeed != 0)
        {
            if (currentChair != null)
            {
                // ���� ���� ��ġ�� �����ϴ� ����
                Vector3 chairPosition = currentChair.transform.position;
                Vector3 chairForward = currentChair.transform.forward;

                // ���� ������ ���� �̵��� ��ġ�� ��� (��: ���� �������� 1 ����)
                Vector3 standUpPosition = chairPosition - chairForward * -1.0f;

                // ĳ������ Y ��ġ�� ���� (���� ���̿� ���߰ų� �����ϰ� ����)
                standUpPosition.y = 0; // �Ǵ� ������ ���� ����

                SetCharacterPosition(standUpPosition);

                StudentChairSitHandler chairHandler = currentChair.GetComponent<StudentChairSitHandler>();
                if (chairHandler != null)
                {
                    photonView.RPC("MarkChairAsUnoccupied", RpcTarget.All, currentChair.name);
                }
                currentChair = null;
            }

            rb.useGravity = true;
            rb.isKinematic = false;
            _isSit = false;
            currentChair = null; // ���ڿ��� �Ͼ���Ƿ� ���� ����
            SetSitStatus(false);
        }
    }

    private void PlaySitAnimation()
    {
        anim.Play("Sit");
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

    [PunRPC]
    public void animPlayRPC(string animation)
    {
        anim.Play(animation);
    }

    public void OnFocusBtnClick()
    {
        isLeaving = !isLeaving;
        if (!_isSit)
        {
            photonView.RPC("animPlayRPC", RpcTarget.Others, "Sit");
            photonView.RPC("TrySitDownRPC", RpcTarget.Others);
            photonView.RPC("SetIsSitRPC", RpcTarget.Others);
            print("��������");

            print("ȭ����ȯ �غ� �Ϸ�");
        }
        photonView.RPC("SwitchAllStudentsCam", RpcTarget.Others);
        SoundManager.instance.BGMPlayOrStop(!isLeaving);
    }

    public void SetPlayerIdle()
    {
        photonView.RPC("animPlayRPC", RpcTarget.Others, "Idle");
        print("�л��� �����ϱ�");
    }

    public void SetToIdleState()
    {
        if (_isSit)
        {
            StandUp();
        }
    }

    // TPS�� FPS ī�޶� ��ȯ
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

        if(!isTPSCam && _isSit)
        {
            LoadButton.instance.Interaction(true);
        }
        else
        {
            LoadButton.instance.Interaction(false);
        }
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
        LoadButton.instance.Interaction(characterMovement.SpareCanvas.activeSelf);
        SoundManager.instance.BGMPlayOrStop(!characterMovement.SpareCanvas.activeInHierarchy);
    }

    public void OnXButtonClick()
    {
        if (photonView.IsMine)
        {
            characterMovement.SpareCanvas.gameObject.SetActive(false);
            characterMovement.CharacterCanvas.gameObject.SetActive(true);
            cameraSetting.FPS_Camera.gameObject.SetActive(false);
            Cam.gameObject.SetActive(true);
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
            PhotonNetwork.LoadLevel("LoadingScene");
            print("Ŀ���ҹ�ư");
        }
    }

    public void OnShareButtonClick()
    {
        photonView.RPC("SwitchAllStudentsCam", RpcTarget.Others);
        print("�л��� ȭ����ȯ");
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
        // ������ ĳ���Ͱ� �ƴ϶��, �ɽ��ϴ�.
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
        // TODO: ���� ����� 'Chair' �±׸� ���� ���ڸ� ã�Ƽ� �� Collider�� ��ȯ�ؾ� �մϴ�.
        // ���ڵ��� ����Ʈ�� �����ϰ� ���� �ʴٸ�, ���⼭ FindGameObjectsWithTag ���� ����Ͽ�
        // ���� ����� ���ڸ� ã�� ������ �����ؾ� �մϴ�.
        // �Ʒ��� ������ ���� �ڵ��Դϴ�:
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

    public void OnClickOpenUI()
    {
        Debug.Log("open");
        if (!isOpenUI)
        {
            OpenUI.SetActive(false);
            characterUI.DOAnchorPos(new Vector2(-150, 0), 0.7f).SetEase(Ease.InQuart);
            isOpenUI = !isOpenUI;
            StartCoroutine(ICloseUI(2.5f));
        }
    }

    private IEnumerator ICloseUI(float delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);
        characterUI.DOAnchorPos((new Vector2(150, 0)), 0.5f);
        isOpenUI = !isOpenUI;
        OpenUI.SetActive(true);
    }

    private int FindChilds(GameObject parent)
    {
        int count = 0;

        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.GetComponent<Button>() != null && child.gameObject.activeSelf)
            {
                count++;
            }
        }

        return count;
    }

    private void AdjustUIHeight()
    {
        int buttonCount = FindChilds(characterUI.transform.GetChild(0).gameObject);
        print("�" + buttonCount);
        RectTransform rectTransform = characterUI.GetComponent<RectTransform>();
        RectTransform childrectTransform = characterUI.transform.GetChild(0).GetComponent<RectTransform>();

        switch(buttonCount)
        {
            case 6:
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 880);
                childrectTransform.anchoredPosition = new Vector3(10, 340, 0);
                break;

            case 5:
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 720);
                childrectTransform.anchoredPosition = new Vector3(10, 270, 0);
                break;

            case 4:
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 580);
                childrectTransform.anchoredPosition = new Vector3(10, 200, 0);
                break;

            case 3:
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 460);
                childrectTransform.anchoredPosition = new Vector3(10, 130, 0);
                break;
        }
    }
    public void OnQuizBtnClick()
    {
        Quiz.instance.OnQuizBtnClick();
    }
    
    [PunRPC]
    public void SendMyPosition(Vector3 position,Quaternion rotation)
    {
        Character.transform.position = position;
        Character.transform.rotation = rotation;
    }

    public void SitDownAtThisChair(GameObject chairObject, StudentChairSitHandler chairHandler)
    {
        if (chairObject != null && !_isSit && DataBase.instance.userInfo.isteacher == false)
        {
            if (!chairHandler.isOccupied)  // ���ڰ� �̹� ��� ������ Ȯ��
            {
                Collider chairCollider = chairObject.GetComponent<Collider>();
                if (chairCollider != null)
                {
                    SitDown(chairCollider);
                    _isSit = true;
                    chairHandler.isOccupied = true;
                }
            }
        }
    }

    public void SitDownTeacherChair(GameObject chairObject)
    {
        if (chairObject != null && !_isSit && DataBase.instance.userInfo.isteacher)
        {
            Collider chairCollider = chairObject.GetComponent<Collider>();
            if (chairCollider != null)
            {
                SitDownTeacher(chairCollider);
                _isSit = true;
            }
        }
    }

}
