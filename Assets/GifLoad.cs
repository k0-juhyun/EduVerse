using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SimpleGif;
using UnityEngine;
using UnityEngine.UI;

public class GifLoad : MonoBehaviour
{
    private string gifPath = "";
    private bool isShowing = false;

    float delayTime;
    float currTime;
    public int idx;

    Gif gif;
    public Sprite [] sprite;
    Image image;
    byte[] gifBytes;

    void Start()
    {
        //gifPath = Application.persistentDataPath + "/24.56843.gif";
    }

    void Update()
    {
        if (isShowing)
        {
            ShowGif();
        }
    }

    public (Sprite[], float) GetSpritesByFrame(string gifPath)
    {
        if (File.Exists(gifPath))
        {
            byte[] data = File.ReadAllBytes(gifPath);

            gif = Gif.Decode(data);

            if (gif != null && gif.Frames.Count > 0)
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
        }
        return (sprite, delayTime);
    }

    public (Sprite[],float) /*Sprite[]*/ GetSpritesByFrame(byte[] gifBytes)
    {
        this.gifBytes = gifBytes;
        gif = Gif.Decode(gifBytes);

        if (gif != null && gif.Frames.Count > 0)
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

        //return sprite;
        return (sprite, delayTime);
    }

    public void Show(Image image, Sprite[] sprites, float gifDelayTime)
    {
        isShowing = true;
        this.image = image;
        this.sprite = sprites;
        delayTime = gifDelayTime;
    }

    private void ShowGif()
    {
        if (sprite != null)
        {
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

    public void StopGif()
    {
        isShowing = false;
        idx = 0;
        currTime = 0f;
    }

    public void SaveGIF(string path, System.Action action)
    {
        File.WriteAllBytes(path, gifBytes);

        // 아이템 정보 저장
        Item item = new Item(Item.ItemType.GIF, Time.time.ToString(), sprite);

        MyItemsManager.instance.AddItem(item);
        //string json = "";
        //MyItems myItems = new MyItems();

        //if (File.Exists(Application.persistentDataPath + "/MyItems.txt"))
        //{
        //    byte[] jsonBytes = File.ReadAllBytes(Application.persistentDataPath + "/MyItems.txt");
        //    json = Encoding.UTF8.GetString(jsonBytes);
        //    myItems = JsonUtility.FromJson<MyItems>(json);
        //}
        //else
        //{
        //    myItems.data = new List<Item>();
        //}

        //myItems.data.Add(item);
        //json = JsonUtility.ToJson(myItems);
        //File.WriteAllText(Application.persistentDataPath + "/MyItems.txt", json);

        action();
    }
}
