using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Paroxe.PdfRenderer;
using System.IO;
using UnityEngine.Android;

public class MenuUI : MonoBehaviour
{
    public HorizontalLayoutGroup horizontalgroup;
    public RectTransform ItemMenu;
    public RectTransform ChatBotMenu;
    public GameObject DrawingTool;
    public GameObject DrawingCanvas;

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
            DOTween.To(() => horizontalgroup.spacing, x => horizontalgroup.spacing = x, 150, 0.4f).SetEase(Ease.OutBack);
            isOpen_menu = !isOpen_menu;
        }
        else
        {
            DOTween.To(() => horizontalgroup.spacing, x => horizontalgroup.spacing = x, -50, 0.4f);
            isOpen_menu = !isOpen_menu;
        }
    }

    // ������ â ����
    public void ItemOpenMenu()
    {
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
            ChatBotMenu.DOAnchorPos((new Vector2(300, 0)), 0.5f);
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
            }
        }, new string[] { pdfFileType });

        Debug.Log("Permission result: " + permission);

    }

    // Example code doesn't use this function but it is here for reference. It's recommended to ask for permissions manually using the
    // RequestPermissionAsync methods prior to calling NativeFilePicker functions
    private async void RequestPermissionAsynchronously(bool readPermissionOnly = false)
    {
        NativeFilePicker.Permission permission = await NativeFilePicker.RequestPermissionAsync(readPermissionOnly);
        Debug.Log("Permission result: " + permission);
    }
}
