using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Compression;

public class Unzip : MonoBehaviour
{
    //private string zipFilePath;
    private string extractionPath;

    private void Start()
    {
        RunZip(Application.persistentDataPath + "/3D_Models/ModelDatas/1.521199.zip");
    }

    void RunZip(string zip)
    {
        extractionPath = Application.dataPath + "/Resources/3D_Models/ModelDatas/";
        ExtractZip(zip, extractionPath); // �޼��� �̸� ����
        File.Delete(zip);
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
