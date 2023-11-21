using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GIFThumbNail : MonoBehaviour
{
    private string gifPath;
    private string thumbNailPath;
    private GifLoad gifLoad;

    void Start()
    {
        gifPath = Application.persistentDataPath + "/GIF/";
        thumbNailPath = Application.persistentDataPath + "/GIFThumbNails/";

        gifLoad = GetComponent<GifLoad>();

        SaveGIFThumbNails();
    }

    void Update()
    {
        
    }

    private void SaveGIFThumbNails()
    {
        string[] gifPaths = Directory.GetFiles(gifPath);

        foreach(string path in gifPaths)
        {
            Sprite[] gifSprites = gifLoad.GetSpritesByFrame(path).Item1;
            byte[] thumbNailBytes = gifSprites[0].texture.EncodeToPNG();
            File.WriteAllBytes(thumbNailPath + Path.GetFileNameWithoutExtension(path) + ".png", thumbNailBytes);
            print(Path.GetFileNameWithoutExtension(path) + " 썸네일 저장 완료");
        }
    }
}
