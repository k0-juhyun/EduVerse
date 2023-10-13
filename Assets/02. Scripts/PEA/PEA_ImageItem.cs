using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PEA_ImageItem : MonoBehaviour
{
    private bool isMoving = false;
    private bool isScaling = false;
    private float moveX = 0f;
    private float moveY = 0f;

    private RectTransform rectTransform;
    private Transform imageTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        imageTransform = transform.GetChild(0);
    }

    void Update()
    {
        if (isMoving)
        {
            MoveItem();
        }
        else if (isScaling)
        {
            SetScale();
        }
    }

    public void SetImage(Sprite sprite)
    {
        Image image = transform.GetChild(0).GetComponent<Image>();
        image.sprite = sprite;
        image.preserveAspect = true;
        image.SetNativeSize();
        image.transform.localScale = Vector3.one * 0.1f;

        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 size = (image.GetComponent<RectTransform>().sizeDelta) / 10 + Vector2.one * 15f;
        rectTransform.sizeDelta = size;
    }

    public void MovePivot()
    {
        GetComponent<RectTransform>().pivot = Vector2.zero;
    }

    public void SetScale()
    {
        imageTransform.localScale += Vector3.one * Input.GetAxis("Mouse X") * 0.1f;
    }

    public void MoveItem()
    {
        moveX = Input.GetAxis("Mouse X");
        moveY = Input.GetAxis("Mouse Y");

        rectTransform.position += new Vector3(moveX, moveY, 0f) * 20f;
    }

    public void ImageDown()
    {
        isMoving = true;
    }

    public void ImageUp()
    {
        isMoving = false;
    }

    public void ScaleDown()
    {
        isScaling = true;
    }

    public void ScaleUp()
    {
        isScaling = false;
    }
}
