using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Compression;

public class Unzip : MonoBehaviour
{
    private string extractionPath;

    private void Start()
    {
        extractionPath = Application.persistentDataPath + "/3D_Models/ModelDatas/";
        //RunZip(extractionPath + "*.zip");

        //if (!Directory.Exists(extractionPath))
        //{
        //    Debug.Log("Directory not found, creating...");
        //    Directory.CreateDirectory(extractionPath);
        //}

#if UNITY_EDITOR
        //에디터 환경에서는 StreamingAssets 폴더 내의 모든 zip 파일을 처리합니다.
        //string streamingAssetsPath = Path.Combine(Application.dataPath, "StreamingAssets");
        //ExtractAllZipsInDirectory(Application.streamingAssetsPath + "/");
#elif UNITY_ANDROID
        // 모바일 환경에서는 Android의 특성상 다른 접근 방식이 필요합니다.
        StartCoroutine(ExtractAllZipsInAndroid());
#elif UNITY_STANDALONE
        string streamingAssetsPath = Path.Combine(Application.dataPath, "StreamingAssets");
        //ExtractAllZipsInDirectorys(streamingAssetsPath);
#endif
    }

#if UNITY_EDITOR
    private void ExtractAllZipsInDirectory(string directoryPath)
    {
        string[] zipFiles = Directory.GetFiles(directoryPath, "*.zip");
        foreach (string zipFile in zipFiles)
        {
            RunZip(zipFile);
        }
    }
#endif

#if UNITY_ANDROID
    public void UnZipAndroid(string zipPath, string extractPath)
    {
        print("UnzipAndroid, " + zipPath);
        StartCoroutine(IUnlZipAndroid(zipPath, extractPath));
    }
#endif

#if UNITY_ANDROID
    private IEnumerator ExtractAllZipsInAndroid()
    {
        print("ExtractAllZipInAndroid");
        string streamingAssetsPath = Path.Combine(Application.streamingAssetsPath, "*.zip");

        print("ExtractAllInAndroid : 111111");

        WWW www = new WWW(streamingAssetsPath);
        yield return www;

        print("ExtractAllZipInAndroid : 2222222");
        if (!string.IsNullOrEmpty(www.error))
        {
            print("ExtractAllZipInAndroid Error");
            Debug.LogError("Error loading zip: " + www.error);
            yield break;
        }

        print("ExtractAllZipInAndroid : 33333333");
        string filePath = Path.Combine(extractionPath, "*.zip");
        File.WriteAllBytes(filePath, www.bytes);

        ExtractZip(filePath, extractionPath);
    }

    private IEnumerator IUnlZipAndroid(string zipPath, string extractPath)
    {
        print("start coroutine");
        WWW www = new WWW(zipPath);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("Error loading zip: " + www.error);
            yield break;
        }

        string filePath = Path.Combine(extractPath, "*.zip");
        File.WriteAllBytes(filePath, www.bytes);

        ExtractZip(filePath, extractPath);
    }
#endif

#if UNITY_STANDALONE
    private void ExtractAllZipsInDirectorys(string directoryPath)
    {
        string[] zipFiles = Directory.GetFiles(directoryPath, "*.zip");
        foreach (string zipFile in zipFiles)
        {
            RunZip(zipFile);
        }
    }
#endif

    public void RunZip(string zip, string extractionPath = "")
    {
        print("RunZip");
        ExtractZip(zip, extractionPath.Equals("") ? this.extractionPath : extractionPath);
        // File.Delete(zip); // 필요에 따라 압축 파일 삭제
    }

    void ExtractZip(string zipPath, string extractionPath)
    {
        print("ExtractZip");
        try
        {
            print("try");
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                print("using");
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    print("foreach");
                    string completeFilePath = Path.Combine(extractionPath, entry.FullName);

                    if (File.Exists(completeFilePath))
                    {
                        File.Delete(completeFilePath);
                    }

                    string directory = Path.GetDirectoryName(completeFilePath);

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    print(completeFilePath);
                    entry.ExtractToFile(completeFilePath);
                }
            }
            Debug.Log("압축 파일이 성공적으로 해제되었습니다.");
        }
        catch (IOException e)
        {
            Debug.LogError("압축 파일을 해제하는 중에 오류 발생: " + e.Message);
        }
    }
}
