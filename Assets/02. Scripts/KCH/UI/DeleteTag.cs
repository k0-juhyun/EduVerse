using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteTag : MonoBehaviour
{
    RectTransform rect;
    const int adjustrect = 50;
    public void Deletetag()
    {
        Transform parentTransform = transform.parent;
        parentTransform.GetComponent<TagInputField>().tagchage();
        Destroy(gameObject);
    }
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        Text text= GetComponentInChildren<Text>();
        
        if(text.text.Length > 5)
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x + (text.text.Length*10 - adjustrect), rect.sizeDelta.y);
        }
    }
}
