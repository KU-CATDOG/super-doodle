using System;
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

[Serializable]
public class ResRanks
{
    public ResRank[] ranks;
}

[Serializable]
public class ResRank
{
    public int timeStamp;
    public string name;
    public int stage;
    public int record;
}

public class RankController : MonoBehaviour
{
    [SerializeField]
    private string serverPath = "http://localhost:5454";

    private ResRanks getResult = new ResRanks();

    public void SendScore(ReqScore score, Action then = null)
    {
        StartCoroutine(Post("rank", JsonUtility.ToJson(score), then));
    }

    public void GetRanks(int from, int length, Action<ResRank[]> then = null)
    {
        StartCoroutine(Get("rank", () =>
        {
            ResRank[] newResult = new ResRank[length];
            for (int i = from; i - from < Mathf.Min(length, getResult.ranks.Length); ++i)
            {
                newResult[i - from] = getResult.ranks[i];
            }
            then?.Invoke(newResult);
        }));
    }

    public void GetRanks(int myScore, Action<ResRank[]> then = null)
    {
        StartCoroutine(Get("rank", () =>
        {
            ResRank[] newResult = new ResRank[6];
            int rankIdx = 0;
            while (rankIdx < getResult.ranks.Length)
            {
                if (getResult.ranks[rankIdx].record <= myScore)
                {
                    rankIdx++;
                }
                else
                {
                    break;
                }
            }
            // -3 -2 -1 myScore 0 1 2
            newResult[0] = rankIdx - 3 >= 0 ? getResult.ranks[rankIdx - 3] : null;
            newResult[1] = rankIdx - 2 >= 0 ? getResult.ranks[rankIdx - 2] : null;
            newResult[2] = rankIdx - 1 >= 0 ? getResult.ranks[rankIdx - 1] : null;
            newResult[3] = rankIdx     < getResult.ranks.Length ? getResult.ranks[rankIdx] : null;
            newResult[4] = rankIdx + 1 < getResult.ranks.Length ? getResult.ranks[rankIdx + 1] : null;
            newResult[5] = rankIdx + 2 < getResult.ranks.Length ? getResult.ranks[rankIdx + 2] : null;

            then?.Invoke(newResult);
        }));
    }

    private IEnumerator Get(string uri="", Action then = null)
    {
        yield return new WaitForSecondsRealtime(0.5f);
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
                getResult = JsonUtility.FromJson<ResRanks>(message);

                then?.Invoke();
            }
        }
    }

    private IEnumerator Post(string uri, string data, Action then = null)
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
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogWarning(request.error);
        }
        else
        {
            //Debug.Log(request.downloadHandler.isDone);
            then?.Invoke();
        }
    }
}
