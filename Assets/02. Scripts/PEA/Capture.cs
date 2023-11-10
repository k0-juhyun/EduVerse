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

    // page raycast target 설정.
    public GameObject pagecontainer;

    // pagecontainer 자식 설정
    RawImage[] pagecontainer_Children;

    [Space(20)]
    // 이미지 받아오기와 문제 생성
    public GameObject Imagedown;
    public GameObject QuizCreate;
    public GameObject ImagedownButton;
    public GameObject QuizCreateButton;

    public GameObject Test;


    private void Start()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
        captureAreaImage.SetActive(false);
        captureAreaImage.transform.SetParent(null);
        rtCaptureArea = captureAreaImage.GetComponent<RectTransform>();

        //captureAreaImage.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = rtCanvas.sizeDelta * 2;
    }

    //RaycastHit hit;
    void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        //    {

        //    }
        //    else
        //    {

        //    }
        //}
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
        if (isCapturing)
        {
            endMousePosition = Input.mousePosition;

            width = Mathf.Abs(Mathf.RoundToInt(endMousePosition.x - startMousePosition.x));
            height = Mathf.Abs(Mathf.RoundToInt(endMousePosition.y - startMousePosition.y));

            startX = (int)((Mathf.Min(startMousePosition.x, endMousePosition.x) ));
            startY = (int)((Mathf.Min(startMousePosition.y, endMousePosition.y) ));

            rtCaptureArea.sizeDelta = new Vector2(width, height);

            rtCaptureArea.anchoredPosition = new Vector2(startX / rtCanvas.localScale.x, startY / rtCanvas.localScale.y);
            //print(Input.mousePosition + ", " + rtCaptureArea.anchoredPosition);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isCapturing)
        {
            isCapturing = false;
            CaptureScreen(Application.persistentDataPath + "/Capture/");
            col.enabled = false;
            captureAreaImage.SetActive(false);
            captureAreaImage.transform.SetParent(null);
        }
    }

    #region Gif 버튼
    public void OnGifBtnClick()
    {
        Imagedown.SetActive(true);
        ImagedownButton.SetActive(true);
    }

    public void OffGifBtnClick()
    {
        Imagedown.SetActive(false);
        ImagedownButton.SetActive(false);
    }
    #endregion

    #region quiz 버튼
    public void OnquizBtnClick()
    {
        QuizCreate.SetActive(true);
        QuizCreateButton.SetActive(true);
    }

    public void OffquizBtnClick()
    {
        QuizCreate.SetActive(false);
        QuizCreateButton.SetActive(false);
    }
    #endregion

    public void OnCaptureBtnClick()
    {
        pagecontainer_Children = pagecontainer.GetComponentsInChildren<RawImage>();

        foreach (RawImage child in pagecontainer_Children)
        {
            // 자기 자신의 경우엔 무시 
            // (게임오브젝트명이 다 다르다고 가정했을 때 통하는 코드)
            if (child.name == transform.name)
                return;

            child.raycastTarget = false;
        }

        col.enabled = true;
        captureAreaImage.transform.localScale = Vector3.one;
        captureAreaImage.transform.SetParent(rtCanvas);
        captureAreaImage.SetActive(true);

        // main camera pivot rotation 0으로 설정
        // main camera의 부모가 rotation 값이 바뀌면 이상해짐.
        //Camera camera = Camera.main;

        //// 카메라 위치 0 , 1  -10으로 설정
        //// 이렇게 해야 캡쳐 드래그가 됌..? 왜지?
        //camera.transform.localPosition = new Vector3(0, 1, -10);
        //camera.transform.localEulerAngles = Vector3.zero;
        //// 카메라 세팅 끄고
        ////if(camera.GetComponentInParent<CameraSetting>()!=null)
        ////camera.GetComponentInParent<CameraSetting>().enabled = false;
        //// 카메라 부모 Pivot 0으로 설정
        //camera.transform.parent.transform.localPosition = new Vector3(0,0,0);
        //camera.transform.parent.transform.rotation = Quaternion.Euler(0,0,0);

        Debug.Log("캡쳐 클릭");
        
    }

    // GIF랑 문제 생성이랑 나눠야함.
    public void OnClickSendCapture_Gif()
    {
        rtCaptureArea.sizeDelta = Vector2.zero;
        videoCreator.UploadImageAndDownloadVideo(captureResultDataPath);
    }

    // 일단은 이미지 없이 태그로만 문제 생성이 되므로 이렇게 해둠.
    public void OnClickSendCapture_Quiz()
    {
        rtCaptureArea.sizeDelta = Vector2.zero;
        videoCreator.UploadImageAndDownloadQuiz();
    }

    // TagText(Clone)
    public void OnClickBackBtn()
    {
        captureResult.SetActive(false);

        rtCaptureArea.sizeDelta = Vector2.zero;
    }

    void CaptureScreen(string savePath)
    {
        print(Application.persistentDataPath);       
        StartCoroutine(IScreenCapture_GIF(width, height, startX, startY, savePath));
    }

    IEnumerator IScreenCapture_GIF(int width, int height, int startX, int startY, string savePath)
    {
        yield return new WaitForEndOfFrame();

        Texture2D captureTexture = new Texture2D(width, height);
        captureTexture.ReadPixels(new Rect(startX, startY, width, height), 0, 0);

        string filePath = Time.time + ".png";
        captureResultDataPath = savePath + filePath ;

        //if(!Directory.Exists(Application.persistentDataPath + "/Capture/"))
        if(!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
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

        // raycast target 다시 설정.
        foreach (RawImage child in pagecontainer_Children)
        {
            // 자기 자신의 경우엔 무시
            // (게임오브젝트명이 다 다르다고 가정했을 때 통하는 코드)
            child.raycastTarget = true;
        }
    }

    public void OnClickAiTest()
    {
        Test.SetActive(true);
    }
}