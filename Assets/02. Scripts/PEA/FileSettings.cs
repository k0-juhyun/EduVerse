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
    private string modelZipPath;
    private string zipExtractionPath;

    private string myItemJsonPath;

    void Awake()
    {
        unzip = GetComponent<Unzip>();

#if UNITY_ANDROID
        print("streaming assets : " + Directory.Exists("jar:file://" + Application.dataPath + "!/assets/"));
        print("File_Settings : " + Directory.Exists("jar:file://" + Application.dataPath + "!/assets/File_Settings/"));
        zipPath = "jar:file://" + Application.dataPath + "!/assets/File_Settings/";
        myItemJsonPath = "jar:file://" + Application.dataPath + "!/assets/File_Settings/MyItems.txt";
#elif UNITY_EDITOR
        zipPath = Application.streamingAssetsPath + "/File_Settings/";
        myItemJsonPath = Application.streamingAssetsPath + "/File_Settings/MyItems.txt"; 
#endif

        print("zipPath : " + zipPath);
        gifZipPath = zipPath + "GIF.zip";
        gifThumbNailZipPath = zipPath + "GIFThumbNails.zip";
        videoZipPath = zipPath + "Videos.zip";
        modelZipPath = zipPath + "ModelDatas.zip";

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

        Directory.CreateDirectory(Application.persistentDataPath + "/3D_Models/ModelDatas/");
        unzip.UnZipAndroid(modelZipPath, zipExtractionPath + "3D_Models/");
#elif UNITY_EDITOR
        print("setting, editor");
        unzip.RunZip(gifZipPath, zipExtractionPath);
        unzip.RunZip(gifThumbNailZipPath, zipExtractionPath);
        unzip.RunZip(videoZipPath, zipExtractionPath);
#endif
        print("setting, 11111");
#if UNITY_EDITOR
        //File.WriteAllBytes(Application.persistentDataPath + "/MyItems.txt", File.ReadAllBytes(myItemJsonPath));
#elif UNITY_ANDROID
        WWW wwwfile = new WWW(myItemJsonPath);
        while (!wwwfile.isDone) { }
        string filePath = Application.persistentDataPath + "/" + Path.GetFileName(myItemJsonPath);
        File.WriteAllBytes(filePath, wwwfile.bytes);
#endif

        print("setting, 22222");
        Texture2D[] marketImageItems = Resources.LoadAll<Texture2D>("Market_Item_Sprites");

        if (!Directory.Exists(Application.persistentDataPath + "/MarketItems/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/MarketItems/");
        }

        //foreach(Texture2D texture in marketImageItems)
        //{
        //    print("아아 : " + texture.name);
        //    File.WriteAllBytes(Application.persistentDataPath + "/MarketItems/" + texture.name + ".png", texture.EncodeToPNG());
        //}

        foreach (Texture2D compressedTexture in marketImageItems)
        {
            // 압축 해제를 위한 새로운 Texture2D 생성
            Texture2D uncompressedTexture = new Texture2D(compressedTexture.width, compressedTexture.height);
            uncompressedTexture.SetPixels(compressedTexture.GetPixels());
            uncompressedTexture.Apply();

            // 파일로 저장
            File.WriteAllBytes(Application.persistentDataPath + "/MarketItems/" + compressedTexture.name + ".png", uncompressedTexture.EncodeToPNG());

            // 메모리 해제
            //Destroy(uncompressedTexture);
        }
    }
}
