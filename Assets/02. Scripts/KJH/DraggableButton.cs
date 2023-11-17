using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableButton : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas parentCanvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gameObject == eventData.pointerCurrentRaycast.gameObject && parentCanvas != null)
        {
            rectTransform.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SaveButtonPosition();
    }

    private void SaveButtonPosition()
    {
        PlayerPrefs.SetFloat(gameObject.name + "_posX", rectTransform.anchoredPosition.x);
        PlayerPrefs.SetFloat(gameObject.name + "_posY", rectTransform.anchoredPosition.y);
        PlayerPrefs.Save();
    }
}
