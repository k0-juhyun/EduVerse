using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;

        float scaleheight = ((float)Screen.width / Screen.height)/((float)16/9);
        float scalewidth = 1 / scaleheight;

        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1 - scaleheight) / 2;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1 - scalewidth) / 2;
        }
        camera.rect = rect;
    }
}
