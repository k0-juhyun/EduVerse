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
            // 다음 페이지가 있으면 넘기기
            if(page + 2 < curDocumentPageCount)
            {
                print("next     1");
                page += 2;
                pdfViewer.GoToPage(page);

                //if (loadButton.gameObject.activeSelf)
                //{
                print("next    2");
                loadButton.LoadSelectedSession(page);
                //}

                //photonView.RPC(nameof(LoadInteractionBtn), RpcTarget.All);
            }
        }
    }

    public void PrevPage()
    {
        print("prev page");
        // 페이지 0 밑으로 안내려가게
        if (pdfViewer.IsLoaded && page > 0)
        {
            print("prev  1");
            page -= 2;
            pdfViewer.GoToPage(page);

            //if (loadButton.gameObject.activeSelf)
            //{
            print("prev   2");
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
