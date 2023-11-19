using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PDF_Page : MonoBehaviourPun
{
    private int page = 0;
    private int curDocumentPageCount = 0;

    public Paroxe.PdfRenderer.PDFViewer pdfViewer;
    public Button nextPageBtn;
    public Button prevPageBtn;
    public LoadButton loadButton;

    void Start()
    {
        nextPageBtn.onClick.AddListener(NextPage);
        prevPageBtn.onClick.AddListener(PrevPage);
    }

    void Update()
    {

    }

    public void GetCurDocumentPageCount()
    {
        curDocumentPageCount = pdfViewer.Document.GetPageCount() - 1;
    }

    public void NextPage()
    {
        print("next page");
        if (pdfViewer.IsLoaded)
        {
            // ���� �������� ������ �ѱ��
            if(page + 2 < curDocumentPageCount)
            {
                page += 2;
                pdfViewer.GoToPage(page);

                //if (loadButton.gameObject.activeSelf)
                //{
                loadButton.LoadSelectedSession(page);
                //}

                //photonView.RPC(nameof(LoadInteractionBtn), RpcTarget.All);
            }
        }
    }

    public void PrevPage()
    {
        print("prev page");
        // ������ 0 ������ �ȳ�������
        if (pdfViewer.IsLoaded && page > 0)
        {
            page -= 2;
            pdfViewer.GoToPage(page);

            //if (loadButton.gameObject.activeSelf)
            //{
                loadButton.LoadSelectedSession(page);
            //}
            //photonView.RPC(nameof(LoadInteractionBtn), RpcTarget.All);
        }
    }

    [PunRPC]
    public void LoadInteractionBtn()
    {
        loadButton.LoadSelectedSession(page);
    }
}
