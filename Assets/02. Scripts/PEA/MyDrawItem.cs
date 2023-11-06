using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
   
public class MyDrawItem : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => DecorateClassRoom.instance.SelectDraw((Texture2D)transform.GetChild(0).GetComponent<RawImage>().texture));
    }

    void Update()
    {

    }
}
