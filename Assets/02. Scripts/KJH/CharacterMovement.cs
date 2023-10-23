using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

// 캐릭터 움직임
// 조이스틱 값 받아서 캐릭터가 움직임
public class CharacterMovement : MonoBehaviourPun, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPunObservable
{
    public GameObject Camera;
    public GameObject Canvas;

    [Header("조이스틱")]
    public RectTransform rectBackground;
    public RectTransform rectJoyStick;

    [Space] public Transform cameraPivotTransform;

    [Space][Header("캐릭터")] public GameObject Character;

    [HideInInspector] public float moveSpeed = 2;
    private float radius;
    private float animParameters;

    [Space]
    [Header("이동속도")]
    public float minSpeed;
    public float maxSpeed;

    private bool isTouch = false;

    private Vector3 movePos;

    private Animator animator;

    #region 포톤 값
    [HideInInspector]
    public Vector3 receivePos;
    [HideInInspector]
    public Quaternion receiveRot = Quaternion.identity;
    [HideInInspector]
    public float lerpSpeed = 50;
    #endregion

    #region 캐릭터 움직임 (조이스틱)
    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    // 손을 떼면
    public void OnPointerUp(PointerEventData eventData)
    {
        // 손 떼면 조이스틱 초기화
        rectJoyStick.localPosition = Vector3.zero;
        movePos = Vector3.zero;
        moveSpeed = 0;

        // 손을 뗐을 때 이동 방향은 카메라 정면방향

        isTouch = false;
        animator.SetFloat("moveSpeed", 0f);

        photonView.RPC("UpdateAnimation", RpcTarget.All, 0f);

        animator.SetTrigger("Idle");
    }

    // 드래그중
    // 드래그 하는 곳으로 포인터 이동
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 value = eventData.position - (Vector2)rectBackground.position;
        value = Vector2.ClampMagnitude(value, radius);
        rectJoyStick.localPosition = value;

        value = value.normalized;

        float dis = Vector2.Distance(rectBackground.position, rectJoyStick.position) / radius;

        // 조이스틱 방향으로 움직임
        Vector3 moveDirection = new Vector3(value.x, 0, value.y);
        movePos = cameraPivotTransform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime;

        Character.transform.forward = movePos;

        // 조이스틱 입력에 따른 블렌드 값 계산
        float distance = Vector3.Distance(rectJoyStick.localPosition, Vector3.zero);
        animParameters = distance / radius; // 0부터 1 사이의 값

        // 블렌드 트리의 Weight 값을 조정하여 애니메이션 설정
        animator.SetFloat("moveSpeed", animParameters);

        photonView.RPC("UpdateAnimation", RpcTarget.All, animParameters);

        moveSpeed = Mathf.Lerp(minSpeed, maxSpeed, animParameters);
    }
    #endregion

    private void Awake()
    {
        radius = rectBackground.rect.width * 0.5f;

        // 애니메이터 컴포넌트 가져오기
        animator = Character.GetComponent<Animator>();

        if (photonView.IsMine)
        {
            Camera.gameObject.SetActive(true);
            Canvas.gameObject.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            // 터치할때만 움직이도록
            if (isTouch)
            {
                Character.transform.position += movePos;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }

    [PunRPC]
    private void UpdateAnimation(float animParameter)
    {
        animator.SetFloat("moveSpeed", animParameter);
    }
}
