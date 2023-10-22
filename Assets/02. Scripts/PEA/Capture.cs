using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class Capture : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector2 startMousePosition;
    private Vector2 endMousePosition;
    private bool isCapturing = false;

    private Collider col;

    private int width = 0;
    private int height = 0;
    private int startX = 0;
    private int startY = 0;

    private string captureResultDataPath = "";

    private RectTransform rtCaptureArea;

    public GameObject captureAreaImage;
    public GameObject captureResult;
    public Image captureResultImage;
    public InputField tagInput;
    public RectTransform rtCanvas;

    public VideoCreator videoCreator;

    // page raycast target ����.
    public GameObject pagecontainer;

    // pagecontainer �ڽ� ����
    RawImage[] pagecontainer_Children;

    private void Start()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
        captureAreaImage.SetActive(false);
        rtCaptureArea = captureAreaImage.GetComponent<RectTransform>();

        captureAreaImage.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = rtCanvas.sizeDelta * 2;
    }

    RaycastHit hit;
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {

            }
            else
            {

            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(eventData.position);
        isCapturing = true;
        startMousePosition = Input.mousePosition;
        print("start " + startMousePosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
            Debug.Log("�巡��");
        if (isCapturing)
        {
            endMousePosition = Input.mousePosition;

            width = Mathf.Abs(Mathf.RoundToInt(endMousePosition.x - startMousePosition.x));
            height = Mathf.Abs(Mathf.RoundToInt(endMousePosition.y - startMousePosition.y));

            startX = (int)Mathf.Min(startMousePosition.x, endMousePosition.x);
            startY = (int)Mathf.Min(startMousePosition.y, endMousePosition.y);

            rtCaptureArea.sizeDelta = new Vector2(width, height);

            rtCaptureArea.anchoredPosition = new Vector2(startX, startY);
            //print(Input.mousePosition + ", " + rtCaptureArea.anchoredPosition);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isCapturing)
        {
            isCapturing = false;
            CaptureScreen();
            col.enabled = false;
            captureAreaImage.SetActive(false);
        }
    }

    public void OnCaptureBtnClick()
    {
        pagecontainer_Children = pagecontainer.GetComponentsInChildren<RawImage>();

        foreach (RawImage child in pagecontainer_Children)
        {
            // �ڱ� �ڽ��� ��쿣 ���� 
            // (���ӿ�����Ʈ���� �� �ٸ��ٰ� �������� �� ���ϴ� �ڵ�)
            if (child.name == transform.name)
                return;

            child.raycastTarget = false;
        }

        col.enabled = true;
        captureAreaImage.SetActive(true);
    }

    public void OnClickSendCapture()
    {
        if(tagInput.text.Length > 0)
        {
            videoCreator.UploadImageAndDownloadVideo(captureResultDataPath);
        }
    } 

    public void OnClickBackBtn()
    {
        captureResult.SetActive(false);

        rtCaptureArea.sizeDelta = Vector2.zero;
    }

    void CaptureScreen()
    {
        print(Application.persistentDataPath);       
        StartCoroutine(IScreenCapture(width, height, startX, startY));
    }

    IEnumerator IScreenCapture(int width, int height, int startX, int startY)
    {
        yield return new WaitForEndOfFrame();

        Texture2D captureTexture = new Texture2D(width, height);
        captureTexture.ReadPixels(new Rect(startX, startY, width, height), 0, 0);

        string filePath = "/Capture/" + Time.time + ".png";
        captureResultDataPath = Application.persistentDataPath + filePath ;

        if(!Directory.Exists(Application.persistentDataPath + "/Capture/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Capture/");
        }

        File.WriteAllBytes(captureResultDataPath, captureTexture.EncodeToPNG());

        Texture2D texture = new Texture2D(captureTexture.width, captureTexture.height);
        Destroy(captureTexture);

        byte[] textureByte = File.ReadAllBytes(captureResultDataPath);
        texture.LoadImage(textureByte);
        texture.Apply();

        captureResult.SetActive(true);
        captureResultImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        captureResultImage.preserveAspect = true;

        // raycast target �ٽ� ����.
        foreach (RawImage child in pagecontainer_Children)
        {
            // �ڱ� �ڽ��� ��쿣 ���� 
            // (���ӿ�����Ʈ���� �� �ٸ��ٰ� �������� �� ���ϴ� �ڵ�)
            child.raycastTarget = true;
        }

    }
}