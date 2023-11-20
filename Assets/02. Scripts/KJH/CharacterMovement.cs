using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// ĳ���� ������
// ���̽�ƽ �� �޾Ƽ� ĳ���Ͱ� ������
public class CharacterMovement : MonoBehaviourPun, IPointerDownHandler, IPointerUpHandler, IDragHandler,IPunObservable
{
    public GameObject Camera;
    public GameObject CharacterCanvas;
    public GameObject SpareCanvas;

    [Header("���̽�ƽ")]
    public RectTransform rectBackground;
    public RectTransform rectJoyStick;

    [Space] public Transform cameraPivotTransform;

    [Space]
    public GameObject SitButton;
    public GameObject CameraButton;
    public GameObject CustomizeButton;
    public GameObject GreetButton;

    [Space][Header("ĳ����")] public GameObject Character;

    [HideInInspector] public float moveSpeed = 2;
    private float radius;
    private float animParameters;

    [Space]
    private float minSpeed = 0;
#if UNITY_EDITOR
    [Header("�̵��ӵ�")]
    [SerializeField]
    private float maxSpeed = 2f;
#elif UNITY_ANDROID
    private float maxSpeed = 2f;
#endif

    private bool isTouch = false;

    private Vector3 movePos;

    private Animator animator;
    private CharacterInteraction characterInteraction;
    private TeacherInteraction characterTeacherInteraction;

    #region ���� ��
    [HideInInspector]
    public Vector3 receivePos;
    [HideInInspector]
    public Quaternion receiveRot = Quaternion.identity;
    [HideInInspector]
    public float lerpSpeed = 50;
    [HideInInspector]
    public float receiveSpeed;
    #endregion

    #region ĳ���� ������ (���̽�ƽ)
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject == rectJoyStick.gameObject)
        {
            isTouch = true;
        }
    }

    // ���� ����
    public void OnPointerUp(PointerEventData eventData)
    {
        // �� ���� ���̽�ƽ �ʱ�ȭ
        rectJoyStick.localPosition = Vector3.zero;
        movePos = Vector3.zero;
        moveSpeed = 0;

        // ���� ���� �� �̵� ������ ī�޶� �������

        isTouch = false;
        animator.SetFloat("moveSpeed", 0f);

        photonView.RPC("UpdateAnimation", RpcTarget.All, 0f);
    }

    // �巡����
    // �巡�� �ϴ� ������ ������ �̵�
    public void OnDrag(PointerEventData eventData)
    {
        if (isTouch && characterTeacherInteraction.isSpawnBtnClick == false)
        {
            Vector2 value = eventData.position - (Vector2)rectBackground.position;
            value = Vector2.ClampMagnitude(value, radius);
            rectJoyStick.localPosition = value;

            value = value.normalized;

            float dis = Vector2.Distance(rectBackground.position, rectJoyStick.position) / radius;

            // ���̽�ƽ �������� ������
            Vector3 moveDirection = new Vector3(value.x, 0, value.y);

            moveSpeed = Mathf.Lerp(minSpeed, maxSpeed, animParameters);

            movePos = cameraPivotTransform.TransformDirection(moveDirection);

            movePos.Normalize();

            movePos *= (moveSpeed * Time.deltaTime);
            movePos.y = 0f;
            Character.transform.forward = movePos;

            // ���̽�ƽ �Է¿� ���� ���� �� ���
            float distance = Vector3.Distance(rectJoyStick.localPosition, Vector3.zero);
            animParameters = distance / radius; // 0���� 1 ������ ��

            // ���� Ʈ���� Weight ���� �����Ͽ� �ִϸ��̼� ����
            animator.SetFloat("moveSpeed", animParameters);

            photonView.RPC("UpdateAnimation", RpcTarget.All, animParameters);
        }
    }
    #endregion

    private void Awake()
    {
        if (photonView.IsMine)
        {
            // �ӽ� �ּ�
            Camera.gameObject.SetActive(true);
            CharacterCanvas.gameObject.SetActive(true);
            SpareCanvas.gameObject.SetActive(false);

            if(SceneManager.GetActiveScene().name == "4.ClassRoomScene")
            {
                print("�־ȵ�??");
                SitButton.SetActive(true);
                CameraButton.SetActive(true);
                CustomizeButton.SetActive(true);
                GreetButton.SetActive(true);
            }
            else if(SceneManager.GetActiveScene().name == "5.GroundScene")
            {
                print("�־ȵ�????");
                CameraButton.SetActive(true);
                CustomizeButton.SetActive(true);
                GreetButton.SetActive(true);
            }
        }

        radius = rectBackground.rect.width * 0.5f;

        // �ִϸ����� ������Ʈ ��������
        animator = Character.GetComponent<Animator>();
        characterTeacherInteraction = GetComponent<TeacherInteraction>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // ��ġ�Ҷ��� �����̵���
            if (isTouch)
            {
                Character.transform.position += movePos;
            }
        }

        else if(receiveSpeed != 0)
        {
            Character.transform.position = Vector3.Lerp(Character.transform.position, receivePos, lerpSpeed * Time.deltaTime);
            Character.transform.rotation = Quaternion.Lerp(Character.transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        }
    }

    // �������� ���� ������
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(moveSpeed);
            stream.SendNext(Character.transform.position);
            stream.SendNext(Character.transform.rotation);
        }
        else
        {
            receiveSpeed = (float)stream.ReceiveNext();
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }

    // ���� �ִϸ��̼� ������Ʈ
    [PunRPC]
    private void UpdateAnimation(float animParameter)
    {
        animator.SetFloat("moveSpeed", animParameter);
    }
}
