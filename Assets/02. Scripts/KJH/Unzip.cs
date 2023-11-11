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
        ExtractZip(zipFilePath, extractionPath); // 메서드 이름 변경
        File.Delete(zipFilePath);
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
