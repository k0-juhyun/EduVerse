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
        ExtractZip(zip, extractionPath); // 메서드 이름 변경
        File.Delete(zip);
    }

    // 메서드 이름을 Unzip에서 ExtractZip으로 변경
    void ExtractZip(string zipPath, string extractionPath)
    {
        try
        {
            ZipFile.ExtractToDirectory(zipPath, extractionPath);
            Debug.Log("압축 파일이 성공적으로 해제되었습니다.");
        }
        catch (IOException e)
        {
            Debug.LogError("압축 파일을 해제하는 중에 오류 발생: " + e.Message);
        }
    }
}
