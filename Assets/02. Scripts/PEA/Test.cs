using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    string streamingAssetsDirectory = "";//"jar:file://" + Application.dataPath + "!/assets/";


    void Start()
    {
        //byte[] bytes = Encoding.UTF8.GetBytes("Hi");
        //File.WriteAllBytes(Application.persistentDataPath + "/aaa.txt", bytes);
        //Debug.Log("File Write");

        //byte[] readBytes =  File.ReadAllBytes(Application.persistentDataPath + "/aaa.txt");
        //string s = Encoding.UTF8.GetString(readBytes);
        //Debug.Log(s);

#if UNITY_EDITOR

        streamingAssetsDirectory =  Application.streamingAssetsPath + "/24.56843.gif";

#elif UNITY_ANDROID

        streamingAssetsDirectory = "jar:file://" + Application.dataPath + "!/assets/ + 24.56843.gif";
   string path = "jar:file://" + Application.dataPath + "!/assets/24.56843.gif";
        WWW wwwfile = new WWW(path);
        while (!wwwfile.isDone) { }
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, "24.56843.gif");
        File.WriteAllBytes(filepath, wwwfile.bytes);

        StreamReader wr = new StreamReader(filepath);
        string line;
        while ((line = wr.ReadLine()) != null)
        {
            //your code
            print(line);
        }
#endif

        print(File.Exists(streamingAssetsDirectory));




        //string oriPath = Path.Combine(Application.streamingAssetsPath, "/24.56843.gif");
        //print("////////////////");


        //print("000000000000000000000000");
        //string realPath = Application.persistentDataPath + "/24.56843.gif";
        //File.WriteAllBytes(realPath, www.bytes);

        //string s = File.ReadAllText(realPath);

        //print(s);
        //WWW www = new WWW(streamingAssetsDirectory);
        //while (!www.isDone) { } //완료될 때까지 대기(원래 www는 비동기 처리방식임)
        ////결과 텍스트(인코딩 전)
        //byte[] resultBytes = www.bytes;

        //print(resultBytes.Length);

        //string videoPath = Application.persistentDataPath +"/24.56843.gif";  // 저장할 동영상 파일 경로
        //File.WriteAllBytes(videoPath, resultBytes);



        //if (Directory.Exists(streamingAssetsDirectory))
        //{
        //    print("sssssss");
        //}

        //if (File.Exists(streamingAssetsDirectory + "/24.56843.gif"))
        //{
        //    byte[] videoData = File.ReadAllBytes(streamingAssetsDirectory + "/24.56843.gif");
        //    print("1111");

        //    //string videoPath = Application.persistentDataPath + "/24.56843.gif";  // 저장할 동영상 파일 경로
        //    File.WriteAllBytes(videoPath, videoData);

        //    Debug.Log("ff");
        //}
        //else
        //{
        //    print("streamingDataPath nn");
        //}
    }

    void Update()
    {
        
    }
}
