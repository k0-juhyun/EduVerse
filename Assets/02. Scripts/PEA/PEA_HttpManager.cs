using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum RequestType
{
    GET,        // 정보 요청
    POST,       // 정보 저장
    PUT,        // 정보 업데이트
    DELETE,     // 정보 삭제
    GETTEXTURE,
    POSTTEXTURE
}

public class HttpInfo
{
    public RequestType requestType;
    public string url = "";
    public string body;
    public Action<DownloadHandler> onReceive;

    public void Set(RequestType type, string url, Action<DownloadHandler> callback, bool useDefaultUrl = true)
    {
        requestType = type;
        this.url = "http://221.163.19.218:5051/chat/send";
        //this.url = url;
        onReceive = callback;
    }
}

public class PEA_HttpManager : MonoBehaviour
{
    public static PEA_HttpManager instance = null;


    public Texture2D texture;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void SendRequest(HttpInfo httpInfo)
    {
        StartCoroutine(CoSendRequest(httpInfo));
    }

    IEnumerator CoSendRequest(HttpInfo httpInfo)
    {
        UnityWebRequest req = null;

        switch (httpInfo.requestType)
        {
            case RequestType.GET:
                // Get방식으로 req에 정보 세팅
                req = UnityWebRequest.Get(httpInfo.url);
                break;
            case RequestType.POST:
                print(httpInfo.body.GetType()) ;
                req = UnityWebRequest.Post(httpInfo.url, httpInfo.body);
                byte[] byteBody = Encoding.UTF8.GetBytes(httpInfo.body);
                req.uploadHandler = new UploadHandlerRaw(byteBody);
                //print(byteBody.GetType());

                //헤더 추가
                //req.SetRequestHeader("Content-Type", "application/json");
                break;
            case RequestType.PUT:
                req = UnityWebRequest.Put(httpInfo.url, httpInfo.body);
                break;
            case RequestType.DELETE:
                req = UnityWebRequest.Delete(httpInfo.url);
                break;
            //case RequestType.TEXTURE:
            //    req = UnityWebRequestTexture.GetTexture(httpInfo.url);
            //    break;
        }


        // 서버에 요청을 보내고 응답이 올때까지 기다림
        yield return req.SendWebRequest();

        // 만약 응답이 성공했으면 
        if (req.result == UnityWebRequest.Result.Success)
        {
            print("네트워크 응답 : " + req.downloadHandler.text);

            if (httpInfo.onReceive != null)
            {
                httpInfo.onReceive(req.downloadHandler);
            }
        }

        // 통신 실패
        else
        {
            print("네트워크 에러 : " + req.error);
        }

        req.Dispose();
    }
}
