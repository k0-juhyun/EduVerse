using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PDF_Page : MonoBehaviour
{
    private int page = 0;

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

    public void NextPage()
    {
        if (pdfViewer.IsLoaded)
        {
            page += 2;
            pdfViewer.GoToPage(page);
            loadButton.LoadSelectedSession(page);
        }
    }

    public void PrevPage()
    {
        if (pdfViewer.IsLoaded)
        {
            page -= 2;
            pdfViewer.GoToPage(page);
            loadButton.LoadSelectedSession(page);
        }
    }
}
