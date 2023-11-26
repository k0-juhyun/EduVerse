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
public class CharacterMovement : MonoBehaviourPun, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPunObservable
{
    public GameObject Camera;
    public GameObject CharacterCanvas;
    public GameObject SpareCanvas;
    public GameObject Character;

    [Header("���̽�ƽ")]
    public RectTransform rectBackground;
    public RectTransform rectJoyStick;

    [Space] public Transform cameraPivotTransform;

    [Space]
    public GameObject CameraButton;
    public GameObject CustomizeButton;
    public GameObject GreetButton;
    private GameObject QuizButton;

    private float radius;
    private float animParameters;

    [Space]
    public float moveSpeed = 3;
    private float minSpeed = 0;
    private float maxSpeed = 3;

    private bool isTouch = false;
    private bool isRequest = false;
    private bool gotFirstPos = false;

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

    // �ּ�
    // �巡����
    // �巡�� �ϴ� ������ ������ �̵�
    public void OnDrag(PointerEventData eventData)
    {
        if (isTouch && characterTeacherInteraction.isSpawnBtnClick == false)
        {
            Vector2 value = eventData.position - (Vector2)rectBackground.position;
            value = Vector2.ClampMagnitude(value, radius);
            TTT(value);

            // rectJoyStick.localPosition = value;

            //value = value.normalized;



            //float dis = Vector2.Distance(rectBackground.position, rectJoyStick.position) / radius;

            //// ���̽�ƽ �������� ������
            //Vector3 moveDirection = new Vector3(value.x, 0, value.y);

            //moveSpeed = Mathf.Lerp(minSpeed, maxSpeed, animParameters);

            //movePos = cameraPivotTransform.TransformDirection(moveDirection);

            //movePos.Normalize();

            //movePos *= (moveSpeed * Time.deltaTime);
            //movePos.y = 0f;
            //Character.transform.forward = movePos;

            //// ���̽�ƽ �Է¿� ���� ���� �� ���
            //float distance = Vector3.Distance(rectJoyStick.localPosition, Vector3.zero);
            //animParameters = distance / radius; // 0���� 1 ������ ��

            //// ���� Ʈ���� Weight ���� �����Ͽ� �ִϸ��̼� ����
            //animator.SetFloat("moveSpeed", animParameters);

            //photonView.RPC("UpdateAnimation", RpcTarget.All, animParameters);
        }
    }

    void TTT(Vector2 value)
    {
        rectJoyStick.localPosition = value;
        value = value.normalized;

        // ���̽�ƽ �������� ������
        Vector3 moveDirection = new Vector3(value.x, 0, value.y);
        //print(value);

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

    #endregion

    private void Awake()
    {
        characterTeacherInteraction = GetComponentInChildren<TeacherInteraction>();

        if (photonView.IsMine)
        {
            // �ӽ� �ּ�
            Camera.gameObject.SetActive(true);
            CharacterCanvas.gameObject.SetActive(true);
            SpareCanvas.gameObject.SetActive(false);

            if (SceneManager.GetActiveScene().name == "4.ClassRoomScene")
            {
                print("�־ȵ�??");
                CameraButton.SetActive(true);
                CustomizeButton.SetActive(true);
                GreetButton.SetActive(true);
            }
            else if (SceneManager.GetActiveScene().name == "5.GroundScene")
            {
                print("�־ȵ�????");
                CameraButton.SetActive(true);
                CustomizeButton.SetActive(true);
                GreetButton.SetActive(true);
                QuizButton = characterTeacherInteraction.quizButton;
                QuizButton.SetActive(true);
            }
        }

        radius = rectBackground.rect.width * 0.5f;

        // �ִϸ����� ������Ʈ ��������
        animator = Character.GetComponent<Animator>();

        if(SceneManager.GetActiveScene().name == "5.GroundScene")
        {
            moveSpeed = 6;
            maxSpeed = 6;
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            //if(isRequest == false)
            //{
            //    isRequest = true;
            //    Character.transform.position = Vector3.Lerp(Character.transform.position, receivePos, lerpSpeed * Time.deltaTime);
            //    Character.transform.rotation = Quaternion.Lerp(Character.transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
            //}

            // ��ġ�Ҷ��� �����̵���
            if (isTouch)
            {
                //Character.transform.position += movePos;
                GetComponentInChildren<Rigidbody>().velocity = Character.transform.forward * 2;
            }

            else
            {
                Vector2 value = new Vector2();
                value.x = Input.GetAxisRaw("Horizontal");
                value.y = Input.GetAxisRaw("Vertical");
                if (value.sqrMagnitude <= 0)
                {
                    GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
                    moveSpeed = 0;
                }
                else
                {
                    TTT(value);
                    GetComponentInChildren<Rigidbody>().velocity = Character.transform.forward * 2;
                }
            }
        }

        else if (!gotFirstPos || (receiveSpeed != 0))
        {
            Character.transform.position = Vector3.Lerp(Character.transform.position, receivePos, lerpSpeed * Time.deltaTime);
            Character.transform.rotation = Quaternion.Lerp(Character.transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
            gotFirstPos = true;
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