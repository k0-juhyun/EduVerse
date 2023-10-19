using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

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

    private RectTransform rtCaptureArea;

    public GameObject captureAreaImage;
    public GameObject captureResult;
    public Image captureResultImage;

    private void Start()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
        captureAreaImage.SetActive(false);
        rtCaptureArea = captureAreaImage.GetComponent<RectTransform>();
    }

    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isCapturing = true;
        startMousePosition = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isCapturing)
        {
            endMousePosition = Input.mousePosition;

            width = Mathf.Abs(Mathf.RoundToInt(endMousePosition.x - startMousePosition.x));
            height = Mathf.Abs(Mathf.RoundToInt(endMousePosition.y - startMousePosition.y));

            startX = (int)Mathf.Min(startMousePosition.x, endMousePosition.x);
            startY = (int)Mathf.Min(startMousePosition.y, endMousePosition.y);

            rtCaptureArea.sizeDelta = new Vector2(width, height);

            rtCaptureArea.anchoredPosition = new Vector2(startX, startY);
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
        col.enabled = true;
        captureAreaImage.SetActive(true);
    }

    void CaptureScreen()
    {
        StartCoroutine(IScreenCapture(width, height, startX, startY));
    }

    IEnumerator IScreenCapture(int width, int height, int startX, int startY)
    {
        yield return new WaitForEndOfFrame();

        Texture2D captureTexture = new Texture2D(width, height);
        captureTexture.ReadPixels(new Rect(startX, startY, width, height), 0, 0);

        string filePath = "ScreenCaptureTextures/" + Time.time;
        File.WriteAllBytes("Assets/Resources/" + filePath +".png", captureTexture.EncodeToPNG());

        Destroy(captureTexture);
        AssetDatabase.Refresh();

        Texture2D texture = Resources.Load<Texture2D>(filePath);

        captureResult.SetActive(true);
        captureResultImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        captureResultImage.preserveAspect = true;
    }
}