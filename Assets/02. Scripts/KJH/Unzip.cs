using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Compression;

public class Unzip : MonoBehaviour
{
    private string zipFilePath;
    private string extractionPath;

    void RunZip(string zip)
    {
        zipFilePath = Application.persistentDataPath + "/" + zip;
        extractionPath = Application.dataPath + "/Resources";
        ExtractZip(zipFilePath, extractionPath); // �޼��� �̸� ����
        File.Delete(zipFilePath);
    }

    // �޼��� �̸��� Unzip���� ExtractZip���� ����
    void ExtractZip(string zipPath, string extractionPath)
    {
        try
        {
            ZipFile.ExtractToDirectory(zipPath, extractionPath);
            Debug.Log("���� ������ ���������� �����Ǿ����ϴ�.");
        }
        catch (IOException e)
        {
            Debug.LogError("���� ������ �����ϴ� �߿� ���� �߻�: " + e.Message);
        }
    }
}
