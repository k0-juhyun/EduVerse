using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Paroxe.PdfRenderer;
using System.IO;
using UnityEngine.Android;
using Photon.Pun;

public class MenuUI : MonoBehaviourPun
{
    public HorizontalLayoutGroup horizontalgroup;
    public RectTransform ItemMenu;
    public RectTransform ChatBotMenu;
    public RectTransform DrawingToolmenu;
    public GameObject DrawingTool;
    public GameObject DrawingCanvas;
    public GameObject HomePage;
    public GameObject MyCanvas;
    public GameObject QuizMenu;

    [Space(10)]
    public PDF_Page pdfPage;

    GameObject quizPanel;

    bool isOpen_menu;
    bool isItem_menu;
    bool isChatbot_menu;
    bool isdrawing_menu;

    // 파일 경로
    private string pdfFileType;

    private void Start()
    {
        // pdf 가져오기
        pdfFileType = NativeFilePicker.ConvertExtensionToFileType("pdf");
    }

    // 버튼 함수

    // 메뉴열기
    public void OpenMenuButton()
    {
        // 닫혀있으면
        if (!isOpen_menu)
        {
            DOTween.To(() => horizontalgroup.spacing, x => horizontalgroup.spacing = x, 110, 0.4f).SetEase(Ease.OutCubic);
            DrawingToolmenu.DOAnchorPosX(125, 0.4f).SetEase(Ease.OutBack);
            isOpen_menu = !isOpen_menu;
        }
        else
        {
            DOTween.To(() => horizontalgroup.spacing, x => horizontalgroup.spacing = x, -50, 0.4f);
            DrawingToolmenu.DOAnchorPosX(40, 0.4f);
            isOpen_menu = !isOpen_menu;
        }
    }

    // 아이템 창 열기
    public void ItemOpenMenu()
    {
        print(isItem_menu);
        if (!isItem_menu)
        {
            //ItemMenu.DOLocalMoveY(750, 0.5f);
            ItemMenu.DOAnchorPos(new Vector2(0, -125), 0.5f);
            isItem_menu = !isItem_menu;
        }
        else
        {
            ItemMenu.DOAnchorPos(new Vector2(0, 0), 0.5f);
            isItem_menu = !isItem_menu;
        }
    }

    // 챗 봇 열기
    public void ChatBotOpenMenu()
    {
        Debug.Log("item");
        if (!isOpen_menu)
        {
            ChatBotMenu.DOAnchorPos(new Vector2(0, 0), 0.5f);
            isOpen_menu = !isOpen_menu;
        }
        else
        {
            ChatBotMenu.DOAnchorPos((new Vector2(400, 0)), 0.5f);
            isOpen_menu = !isOpen_menu;
        }
    }

    // 드로잉 툴
    public void DrawingToolMenu()
    {
        DrawingTool.SetActive(true);
        DrawingCanvas.GetComponent<RectTransform>().DOAnchorPosY(-295, 0.5f);
        isdrawing_menu = !isdrawing_menu;
    }

    // PDF 파일 열기.
    // PDF 파일을 선택하기 위한 파일 선택 다이얼로그
    public void PickFile()
    {
        // NativeFilePicker 플러그인을 통해 파일 경로 가져옴.
#if UNITY_ANDROID
        if (NativeFilePicker.IsFilePickerBusy())
            return;

        // Pick a PDF file
        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
                Debug.Log("Operation cancelled");
            else
            {
                // PDF 열기.
                PDFViewer pdfViewer = FindObjectOfType<PDFViewer>();
                pdfViewer.LoadDocumentFromFile(path);

                // pdf 로드하면 총 페이지수 받아오기
                pdfPage.GetCurDocumentPageCount();
            }
        }, new string[] { pdfFileType });

        //Debug.Log("Permission result: " + permission);
#else
        #region 윈도우 빌드
        // 윈도우 빌드
        // PDF 열기.
        string path = "C:\\Users\\user\\Desktop\\최종플젝\\교과서.pdf";
        PDFViewer pdfViewer = FindObjectOfType<PDFViewer>();
        pdfViewer.LoadDocumentFromFile(path);
        pdfPage.GetCurDocumentPageCount();
        #endregion
#endif


        // pdf 로드하면 총 페이지수 받아오기
        //pdfPage.GetCurDocumentPageCount();

    }

    // Example code doesn't use this function but it is here for reference. It's recommended to ask for permissions manually using the
    // RequestPermissionAsync methods prior to calling NativeFilePicker functions
    private async void RequestPermissionAsynchronously(bool readPermissionOnly = false)
    {
        NativeFilePicker.Permission permission = await NativeFilePicker.RequestPermissionAsync(readPermissionOnly);
        Debug.Log("Permission result: " + permission);
    }

    public void OnBackBtnClick()
    {
        HomePage.SetActive(true);
        MyCanvas.SetActive(false);
    }

    public void OnQuizBtnClick()
    {
        // 프리팹 instantsiate 한다 (포톤인스턴시에이트 X)
        // 선생이 퀴즈 패널 여는 버튼.
        quizPanel = PhotonNetwork.Instantiate("Teacher_QuizCanvas", Vector3.zero, Quaternion.identity);
        //GameObject quizPanel = Instantiate(QuizMenu, Vector3.zero, Quaternion.identity);
        photonView.RPC(nameof(DestroyOtherQuizPanels), RpcTarget.All);
    }

    [PunRPC]
    private void DestroyOtherQuizPanels()
    {
        GameObject[] quizPanels = GameObject.FindGameObjectsWithTag("QuizPanel");

        foreach (GameObject panel in quizPanels)
        {
            if (!panel.GetPhotonView().IsMine)
            {
                //Destroy(panel);
                panel.GetComponent<Canvas>().enabled = false;
            }
        }
    }

}
