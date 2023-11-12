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
        // 에디터 환경에서는 StreamingAssets 폴더 내의 파일을 사용합니다.
        zipFilePath = Path.Combine(Application.dataPath, "StreamingAssets/Mask.zip");
#elif UNITY_ANDROID
        // 모바일 환경에서는 jar 파일 내부의 assets 폴더를 사용합니다.
        zipFilePath = "jar:file://" + Application.dataPath + "!/assets/Mask.zip";
        // 모바일에서는 파일을 임시 경로에 복사한 후 사용합니다.
        StartCoroutine(CopyAndExtractZip(zipFilePath, extractionPath));
        return;
#endif

        // 에디터 환경에서는 바로 추출을 진행합니다.
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

    // 메서드 이름을 Unzip에서 ExtractZip으로 변경
    void ExtractZip(string zipPath, string extractionPath)
    {
        try
        {
            // 압축 파일을 열어 각 항목을 순회합니다.
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string completeFilePath = Path.Combine(extractionPath, entry.FullName);

                    // 해당 경로에 파일이 이미 존재한다면 삭제합니다.
                    if (File.Exists(completeFilePath))
                    {
                        File.Delete(completeFilePath);
                    }

                    // 디렉토리가 존재하지 않으면 생성합니다.
                    string directory = Path.GetDirectoryName(completeFilePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // 파일을 추출합니다.
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
