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
        //������ ȯ�濡���� StreamingAssets ���� ���� ��� zip ������ ó���մϴ�.
        string streamingAssetsPath = Path.Combine(Application.dataPath, "StreamingAssets");
        ExtractAllZipsInDirectory(streamingAssetsPath);
#elif UNITY_ANDROID
        // ����� ȯ�濡���� Android�� Ư���� �ٸ� ���� ����� �ʿ��մϴ�.
        StartCoroutine(ExtractAllZipsInAndroid());
#elif UNITY_STANDALONE
        string streamingAssetsPath = Path.Combine(Application.dataPath, "StreamingAssets");
        ExtractAllZipsInDirectorys(streamingAssetsPath);
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
    private IEnumerator ExtractAllZipsInAndroid()
    {
        string streamingAssetsPath = Path.Combine(Application.streamingAssetsPath, "*.zip");
        WWW www = new WWW(streamingAssetsPath);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("Error loading zip: " + www.error);
            yield break;
        }

        string filePath = Path.Combine(extractionPath, "*.zip");
        File.WriteAllBytes(filePath, www.bytes);

        ExtractZip(filePath, extractionPath);
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
        print("RunZip, " + zip);
        ExtractZip(zip, extractionPath.Equals("") ? this.extractionPath : extractionPath);
        // File.Delete(zip); // �ʿ信 ���� ���� ���� ����
    }

    void ExtractZip(string zipPath, string extractionPath)
    {
        print("Enter ExtractZip");
        try
        {
            print("Try");
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                print("using");
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string completeFilePath = Path.Combine(extractionPath, entry.FullName);
                    print(completeFilePath);

                    if (File.Exists(completeFilePath))
                    {
                        File.Delete(completeFilePath);
                    }

                    string directory = Path.GetDirectoryName(completeFilePath);
                    print(directory);

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    entry.ExtractToFile(completeFilePath);
                }
            }
            Debug.Log("���� ������ ���������� �����Ǿ����ϴ�.");
        }
        catch (IOException e)
        {
            Debug.LogError("���� ������ �����ϴ� �߿� ���� �߻�: " + e.Message);
        }
    }
}
