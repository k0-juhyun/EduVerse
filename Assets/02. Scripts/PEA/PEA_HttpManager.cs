using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum RequestType
{
    GET,        // ���� ��û
    POST,       // ���� ����
    PUT,        // ���� ������Ʈ
    DELETE,     // ���� ����
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
                // Get������� req�� ���� ����
                req = UnityWebRequest.Get(httpInfo.url);
                break;
            case RequestType.POST:
                print(httpInfo.body.GetType()) ;
                req = UnityWebRequest.Post(httpInfo.url, httpInfo.body);
                byte[] byteBody = Encoding.UTF8.GetBytes(httpInfo.body);
                req.uploadHandler = new UploadHandlerRaw(byteBody);
                //print(byteBody.GetType());

                //��� �߰�
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


        // ������ ��û�� ������ ������ �ö����� ��ٸ�
        yield return req.SendWebRequest();

        // ���� ������ ���������� 
        if (req.result == UnityWebRequest.Result.Success)
        {
            print("��Ʈ��ũ ���� : " + req.downloadHandler.text);

            if (httpInfo.onReceive != null)
            {
                httpInfo.onReceive(req.downloadHandler);
            }
        }

        // ��� ����
        else
        {
            print("��Ʈ��ũ ���� : " + req.error);
        }

        req.Dispose();
    }
}
