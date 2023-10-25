using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleGif;
using UnityEngine;
using UnityEngine.UI;

public class GifLoad : MonoBehaviour
{
    float delayTime;
    float currTime;
    public int idx;

    Gif gif;
    Sprite [] sprite;
    Image image;

    void Start()
    {
        image = GetComponent<Image>();

        byte[] data = File.ReadAllBytes(Application.streamingAssetsPath + "/Wow-gif.gif");

        gif = Gif.Decode(data);
        if(gif != null && gif.Frames.Count > 0)
        {
            delayTime = gif.Frames[0].Delay;
        }
            
        SimpleGif.Data.Color32[] gifColor32;
        Color32[] color;
        sprite = new Sprite[gif.Frames.Count];
        for (int i = 0; i < sprite.Length; i++)
        {
            Texture2D tex = new Texture2D(gif.Frames[0].Texture.width, gif.Frames[0].Texture.height);
            gifColor32 = gif.Frames[i].Texture.GetPixels32();

            color = new Color32[gifColor32.Length];

            for (int j = 0; j < gifColor32.Length; j++)
            {
                color[j].r = gifColor32[j].r;
                color[j].g = gifColor32[j].g;
                color[j].b = gifColor32[j].b;
                color[j].a = gifColor32[j].a;
            }
            tex.SetPixels32(color);
            tex.Apply();

            sprite[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }

        image.sprite = sprite[0];
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    idx++;
        //    idx %= sprite.Length;

        //    image.sprite = sprite[idx];
        //}

        currTime += Time.deltaTime;
        if (currTime > delayTime)
        {
            idx++;
            idx %= sprite.Length;

            image.sprite = sprite[idx];
            currTime = 0;
        }
    }
}
