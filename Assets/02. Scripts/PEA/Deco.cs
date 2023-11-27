using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class Deco : MonoBehaviour
{
    private Camera mainCam;

    public Button drawBtn;
    public Button decoBtn;
    public GameObject draw;
    public GameObject myDraws;
    public Button[] backBtn;
    public Camera drawCam;
    public RawImage drawPaper;     // 그림 그리는 판 이미지(그려지는 그림을 보여줌, 실제 그림은 큐브애 그려짐)
    public Rito.TexturePainter.TexturePaintTarget paintTarget;

    private bool isDrawButtonPressed = false;
    private bool isBackButtonPressed = false;

    private void Start()
    {
        foreach (Button button in backBtn)
        {
            button.onClick?.AddListener(OnClickBackBtn);
        }

        drawBtn.onClick.AddListener(() =>
        {
            OnClickDrawBtn();
            isDrawButtonPressed = true;  // 버튼이 눌렸음을 표시
        });

        decoBtn.onClick.AddListener(OnClickDecoBtn);
        mainCam = Camera.main;

    }

    // 그림 그리기 버튼을 눌렀을때
    private void OnClickDrawBtn()
    {
        drawBtn.gameObject.SetActive(false);
        decoBtn.gameObject.SetActive(false);
        draw.SetActive(true);

        mainCam.depth = -1;
        drawCam.gameObject.SetActive(true);
        mainCam.gameObject.SetActive(false);

        SetRanderTexture();
    }

    private void OnClickDecoBtn()
    {
        print("click deco btn");
        drawBtn.gameObject.SetActive(false);
        decoBtn.gameObject.SetActive(false);
        myDraws.SetActive(true);
    }

    private void OnClickBackBtn()
    {
        drawBtn.gameObject.SetActive(true);
        decoBtn.gameObject.SetActive(true);
        draw.SetActive(false);
        myDraws.SetActive(false);

        isBackButtonPressed = true;

        DecorateClassRoom.instance.curSelectedDraw = null;

        if (mainCam.depth != 1)
        {
            mainCam.depth = 1;
            drawCam.gameObject.SetActive(false);
            mainCam.gameObject.SetActive(true);
            paintTarget.InitRenderTexture();
            SetRanderTexture();
        }

    }

    public void OnClickEraseAllBtn()
    {
        //SetRanderTexture();
        StartCoroutine(IResetDrawPaper());
    }

    IEnumerator IResetDrawPaper()
    {
        yield return new WaitForEndOfFrame();

        SetRanderTexture();
    }

    // 그림 그리는 판 이미지에 큐브 렌더텍스쳐(그림 그려지는 텍스쳐) 넣기
    public void SetRanderTexture()
    {
        print("set render texture");
        drawPaper.texture = paintTarget.renderTexture;
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PhotonView>(out PhotonView photonView))
        {
            if (photonView.IsMine)
            {
                drawBtn.gameObject.SetActive(false);
                decoBtn.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PhotonView>(out PhotonView photonView))
        {
            if (photonView.IsMine)
            {
                drawBtn.gameObject.SetActive(true);
                decoBtn.gameObject.SetActive(true);
                StartCoroutine(ICheckClickDrawButton(other));
                StartCoroutine(ICheckClickBackButton(other));
            }
        }
    }
    private IEnumerator ICheckClickDrawButton(Collider other)
    {
        CharacterMovement characterMovement = other.GetComponentInParent<CharacterMovement>();

        // 버튼이 눌릴 때까지 기다림
        yield return new WaitUntil(() => isDrawButtonPressed);

        // 버튼이 눌리면 캔버스를 비활성화
        characterMovement.CharacterCanvas.SetActive(false);
        isDrawButtonPressed = false;
    }

    private IEnumerator ICheckClickBackButton(Collider other)
    {
        CharacterMovement characterMovement = other.GetComponentInParent<CharacterMovement>();

        yield return new WaitUntil(() => isBackButtonPressed);

        characterMovement.CharacterCanvas.SetActive(true);
        isBackButtonPressed = false;
    }
}
