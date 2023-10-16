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
    TEXTURE
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
        if (useDefaultUrl) this.url = "http://192.168.1.97:5000";
        this.url = url;
        onReceive = callback;
    }
}

public class PEA_HttpManager : MonoBehaviour
{
    public static PEA_HttpManager instance = null;

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
                req = UnityWebRequest.Post(httpInfo.url, httpInfo.body);
                byte[] byteBody = Encoding.UTF8.GetBytes(httpInfo.body);
                req.uploadHandler = new UploadHandlerRaw(byteBody);

                //��� �߰�
                //req.SetRequestHeader("Content-Type", "application/json");
                break;
            case RequestType.PUT:
                req = UnityWebRequest.Put(httpInfo.url, httpInfo.body);
                break;
            case RequestType.DELETE:
                req = UnityWebRequest.Delete(httpInfo.url);
                break;
            case RequestType.TEXTURE:
                req = UnityWebRequestTexture.GetTexture(httpInfo.url);
                break;
        }


        // ������ ��û�� ������ ������ �ö����� ��ٸ�
        yield return req.SendWebRequest();

        // ���� ������ ���������� 
        if (req.result == UnityWebRequest.Result.Success)
        {
            //print("��Ʈ��ũ ���� : " + req.downloadHandler.text);

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
    }
}
