using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Text;

public class TextSender : MonoBehaviour
{
    // FastAPI 서버 엔드포인트 URL
    private string serverUrl = "http://221.163.19.218:5056/chat/send_student"; // 서버 주소 및 엔드포인트를 적절히 변경하세요.


    private void Start()
    {
        SendText("강차니 바보", null);
    }

    // HTTP 요청 보내는 함수
    IEnumerator SendTextToServer(string text, System.Action<DownloadHandler> callback)
    {
        print("SsndTextToServer");
        WWWForm form = new WWWForm();
        form.AddField("text", text);

        using (UnityWebRequest www = UnityWebRequest.Post(serverUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                print(Encoding.UTF8.GetString( www.downloadHandler.data));
                if(callback != null)
                {
                    callback(www.downloadHandler);
                }
            }
            else
            {
                Debug.LogError(www.error);
            }
        }
    }

    // Unity에서 버튼 또는 이벤트에서 이 함수를 호출하여 텍스트를 서버로 보낼 수 있습니다.
    public void SendText(string text, System.Action<DownloadHandler> callback)
    {
        print(text);
        StartCoroutine(SendTextToServer(text, callback));
    }
}