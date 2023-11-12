using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Compression;

public class Unzip : MonoBehaviour
{
    private string extractionPath;
    private string zipFilePath;

    private void Start()
    {
        extractionPath = Application.persistentDataPath + "/3D_Models/ModelDatas/";

        if (!Directory.Exists(extractionPath))
        {
            Debug.Log("Directory not found, creating...");
            Directory.CreateDirectory(extractionPath);
        }

#if UNITY_EDITOR
        // ������ ȯ�濡���� StreamingAssets ���� ���� ������ ����մϴ�.
        zipFilePath = Path.Combine(Application.dataPath, "StreamingAssets/Mask.zip");
#elif UNITY_ANDROID
        // ����� ȯ�濡���� jar ���� ������ assets ������ ����մϴ�.
        zipFilePath = "jar:file://" + Application.dataPath + "!/assets/Mask.zip";
        // ����Ͽ����� ������ �ӽ� ��ο� ������ �� ����մϴ�.
        StartCoroutine(CopyAndExtractZip(zipFilePath, extractionPath));
        return;
#endif

        // ������ ȯ�濡���� �ٷ� ������ �����մϴ�.
        RunZip(zipFilePath);
    }

    private IEnumerator CopyAndExtractZip(string sourceZip, string targetPath)
    {
        WWW www = new WWW(sourceZip);
        yield return www;

        string filePath = Path.Combine(targetPath, "Mask.zip");
        File.WriteAllBytes(filePath, www.bytes);
        RunZip(filePath);
    }

    public void RunZip(string zip)
    {
        ExtractZip(zip, extractionPath);
        //File.Delete(zip);
    }

    // �޼��� �̸��� Unzip���� ExtractZip���� ����
    void ExtractZip(string zipPath, string extractionPath)
    {
        try
        {
            // ���� ������ ���� �� �׸��� ��ȸ�մϴ�.
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string completeFilePath = Path.Combine(extractionPath, entry.FullName);

                    // �ش� ��ο� ������ �̹� �����Ѵٸ� �����մϴ�.
                    if (File.Exists(completeFilePath))
                    {
                        File.Delete(completeFilePath);
                    }

                    // ���丮�� �������� ������ �����մϴ�.
                    string directory = Path.GetDirectoryName(completeFilePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // ������ �����մϴ�.
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
