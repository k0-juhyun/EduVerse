using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawingController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
   
    public RawImage rawImage;
    public Color drawingColor = Color.black;
    public float brushSize = 0.1f;

    private bool isDrawing = false;
    private List<Vector2> drawingPoints = new List<Vector2>();
    private Texture2D drawingTexture;

    void Start()
    {
        // 초기화 코드
        drawingTexture = new Texture2D((int)rawImage.rectTransform.sizeDelta.x, (int)rawImage.rectTransform.sizeDelta.y);
        drawingTexture.filterMode = FilterMode.Point;
        rawImage.texture = drawingTexture;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDrawing = true;
        drawingPoints.Clear();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDrawing)
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
            {
                drawingPoints.Add(localPoint);
                UpdateTexture();
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDrawing = false;
    }

    private void UpdateTexture()
    {
        for (int i = 0; i < drawingPoints.Count; i++)
        {
            int x = Mathf.RoundToInt(drawingPoints[i].x);
            int y = Mathf.RoundToInt(drawingPoints[i].y);

            if (x >= 0 && x < drawingTexture.width && y >= 0 && y < drawingTexture.height)
            {
                drawingTexture.SetPixel(x, y, drawingColor);
            }
        }
        drawingTexture.Apply();
    }
}
