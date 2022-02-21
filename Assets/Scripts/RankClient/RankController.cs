using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ReqScore
{
    public string name;
    public int stage;
    public int record;
}

public class RankController : MonoBehaviour
{
    [SerializeField]
    private string serverPath = "http://localhost:5454";

    public void SendScore(ReqScore score)
    {
        StartCoroutine(Post("rank", JsonUtility.ToJson(score)));
    }

    private IEnumerator Get(string uri="")
    {
        var url = $"{serverPath}/{uri}";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            //http서버로 부터 응답을 대기 
            yield return www.SendWebRequest();

            //http서버로부터 응답을 받았다. 
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //바이너리 데이터를 복구 
                byte[] results = www.downloadHandler.data;

                var message = Encoding.UTF8.GetString(results);

                Debug.Log(message);     //응답했다.!

                //JsonUtility.FromJson()
                //var res_common = JsonConvert.DeserializeObject<res_common>(message);

                //Debug.LogFormat("{0}, {1}", res_common.cmd, res_common.message);

            }
        }
    }

    private IEnumerator Post(string uri, string data)
    {
        var url = $"{serverPath}/{uri}";

        //POST방식으로 http서버에 요청을 보내겠습니다.
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        //Debug.Log(bodyRaw.Length);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        //응답을 기다립니다.
        yield return request.SendWebRequest();

        //응답을 받았습니다.
        //Debug.Log(request.downloadHandler.data);
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogWarning(request.downloadHandler.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.isDone);
        }
        //var res_login = JsonConvert.DeserializeObject<res_login>(request.downloadHandler.text);
        //Debug.LogFormat("{0}, {1}", res_login.cmd, res_login.message);
    }
}
