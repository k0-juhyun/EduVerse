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

    // ���� ���
    private string pdfFileType;

    private void Start()
    {
        // pdf ��������
        pdfFileType = NativeFilePicker.ConvertExtensionToFileType("pdf");
    }

    // ��ư �Լ�

    // �޴�����
    public void OpenMenuButton()
    {
        // ����������
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

    // ������ â ����
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

    // ê �� ����
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

    // ����� ��
    public void DrawingToolMenu()
    {
        DrawingTool.SetActive(true);
        DrawingCanvas.GetComponent<RectTransform>().DOAnchorPosY(-295, 0.5f);
        isdrawing_menu = !isdrawing_menu;
    }

    // PDF ���� ����.
    // PDF ������ �����ϱ� ���� ���� ���� ���̾�α�
    public void PickFile()
    {
        // NativeFilePicker �÷������� ���� ���� ��� ������.
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
                // PDF ����.
                PDFViewer pdfViewer = FindObjectOfType<PDFViewer>();
                pdfViewer.LoadDocumentFromFile(path);

                // pdf �ε��ϸ� �� �������� �޾ƿ���
                pdfPage.GetCurDocumentPageCount();
            }
        }, new string[] { pdfFileType });

        //Debug.Log("Permission result: " + permission);
#else
        #region ������ ����
        // ������ ����
        // PDF ����.
        string path = "C:\\Users\\user\\Desktop\\��������\\������.pdf";
        PDFViewer pdfViewer = FindObjectOfType<PDFViewer>();
        pdfViewer.LoadDocumentFromFile(path);
        pdfPage.GetCurDocumentPageCount();
        #endregion
#endif


        // pdf �ε��ϸ� �� �������� �޾ƿ���
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
        // ������ instantsiate �Ѵ� (�����ν��Ͻÿ���Ʈ X)
        // ������ ���� �г� ���� ��ư.
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
