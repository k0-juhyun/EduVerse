using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSettings : MonoBehaviour
{
    private Unzip unzip;

    private string zipPath;

    private string gifZipPath;
    private string gifThumbNailZipPath;
    private string videoZipPath;
    private string zipExtractionPath;

    private string myItemJsonPath;

    void Awake()
    {
        unzip = GetComponent<Unzip>();

        zipPath = Application.streamingAssetsPath + "/File_Settings/";

        gifZipPath = zipPath + "GIF.zip";
        gifThumbNailZipPath = zipPath + "GIFThumbNails.zip";
        videoZipPath = zipPath + "Videos.zip";

        zipExtractionPath = Application.persistentDataPath + "/";

        myItemJsonPath = Application.streamingAssetsPath + "/File_Settings/MyItems.txt"; 
        Setting();
    }

    void Update()
    {
        
    }

    private void Setting()
    {
        unzip.RunZip(gifZipPath, zipExtractionPath);
        unzip.RunZip(gifThumbNailZipPath, zipExtractionPath);
        unzip.RunZip(videoZipPath, zipExtractionPath);

        File.WriteAllBytes(Application.persistentDataPath + "/MyItems.txt", File.ReadAllBytes(myItemJsonPath));

        Texture2D[] marketImageItems = Resources.LoadAll<Texture2D>("Market_Item_Sprites");

        if (Directory.Exists(Application.persistentDataPath + "/MarketItems/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/MarketItems/");
        }

        foreach(Texture2D texture in marketImageItems)
        {
            print("¾Æ¾Æ : " + texture.name);
            File.WriteAllBytes(Application.persistentDataPath + "/MarketItems/" + texture.name + ".png", texture.EncodeToPNG());
        }
    }
}
