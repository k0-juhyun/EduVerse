using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureTest : MonoBehaviour
{
    private Vector2 startMousePosition;
    private Vector2 endMousePosition;
    private bool isCapturing = false;

    void Update()
    {

    }

    private void OnMouseDown()
    {
        isCapturing = true;
        startMousePosition = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        if (isCapturing)
        {
            endMousePosition = Input.mousePosition;
        }
    }

    private void OnMouseUp()
    {
        if (isCapturing)
        {
            isCapturing = false;
            CaptureScreen();
        }
    }

    void CaptureScreen()
    {
        int width = Mathf.Abs(Mathf.RoundToInt(endMousePosition.x - startMousePosition.x));
        int height = Mathf.Abs(Mathf.RoundToInt(endMousePosition.y - startMousePosition.y));
        int startX = (int)Mathf.Min(startMousePosition.x, endMousePosition.x);
        int startY = (int)Mathf.Min(startMousePosition.y, endMousePosition.y);

        StartCoroutine(IScreenCapture(width, height, startX, startY));
    }

    IEnumerator IScreenCapture(int width, int height, int startX, int startY)
    {
        yield return new WaitForEndOfFrame();

        Texture2D captureTexture = new Texture2D(width, height);
        captureTexture.ReadPixels(new Rect(startX, Screen.height - startY - height, width, height), 0, 0);

        File.WriteAllBytes("Assets/Resources/ScreenCaptureTextures/" + Time.time + ".png", captureTexture.EncodeToPNG());
        Destroy(captureTexture);
    }
}