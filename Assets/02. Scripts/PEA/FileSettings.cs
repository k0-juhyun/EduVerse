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


#if UNITY_ANDROID
        print("streaming assets : " + Directory.Exists("jar:file://" + Application.dataPath + "!/assets/"));
        print("File_Settings : " + Directory.Exists("jar:file://" + Application.dataPath + "!/assets/File_Settings/"));
        zipPath = "jar:file://" + Application.dataPath + "!/assets";
        myItemJsonPath = "jar:file://" + Application.dataPath + "!/assets/File_Settings/MyItems.txt";
#elif UNITY_EDITOR
        zipPath = Application.streamingAssetsPath + "/File_Settings/";
        myItemJsonPath = Application.streamingAssetsPath + "/File_Settings/MyItems.txt"; 
#endif

        print("zipPath : " + zipPath);
        gifZipPath = zipPath + "GIF.zip";
        gifThumbNailZipPath = zipPath + "GIFThumbNails.zip";
        videoZipPath = zipPath + "Videos.zip";

        zipExtractionPath = Application.persistentDataPath + "/";

        Setting();
    }

    void Update()
    {
        
    }

    private void Setting()
    {
#if UNITY_ANDROID
        print("setting, Android");
        unzip.UnZipAndroid(gifZipPath, zipExtractionPath);
        unzip.UnZipAndroid(gifThumbNailZipPath, zipExtractionPath);
        unzip.UnZipAndroid(videoZipPath, zipExtractionPath);
#elif UNITY_EDITOR
        print("setting, editor");
        unzip.RunZip(gifZipPath, zipExtractionPath);
        unzip.RunZip(gifThumbNailZipPath, zipExtractionPath);
        unzip.RunZip(videoZipPath, zipExtractionPath);
#endif
        print("setting, 11111");
#if UNITY_ANDROID
        WWW wwwfile = new WWW(myItemJsonPath);
        while (!wwwfile.isDone) { }
        string filePath = Application.persistentDataPath + "/" + Path.GetFileName(myItemJsonPath);
        File.WriteAllBytes(filePath, wwwfile.bytes);
#elif UNITY_EDITOR
        File.WriteAllBytes(Application.persistentDataPath + "/MyItems.txt", File.ReadAllBytes(myItemJsonPath));
#endif

        print("setting, 22222");
        Texture2D[] marketImageItems = Resources.LoadAll<Texture2D>("Market_Item_Sprites");

        if (!Directory.Exists(Application.persistentDataPath + "/MarketItems/"))
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
