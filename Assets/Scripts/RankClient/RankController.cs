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

    public void GetRanks(int stage, int from, int length, Action<ResRank[]> then = null)
    {
        StartCoroutine(Get("rank", () =>
        {
            CutOutOtherBoss(stage);

            ResRank[] newResult = new ResRank[length];
            for (int i = from; i - from < Mathf.Min(length, getResult.ranks.Length); ++i)
            {
                newResult[i - from] = getResult.ranks[i];
            }
            then?.Invoke(newResult);
        }));
    }

    public void GetRanks(int stage, int myScore, Action<ResRank[]> then = null)
    {
        StartCoroutine(Get("rank", () =>
        {
            CutOutOtherBoss(stage);

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
            //http????????? ?????? ????????? ?????? 
            yield return www.SendWebRequest();

            //http??????????????? ????????? ?????????. 
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //???????????? ???????????? ?????? 
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

        //POST???????????? http????????? ????????? ??????????????????.
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        //Debug.Log(bodyRaw.Length);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        //????????? ???????????????.
        yield return request.SendWebRequest();

        //????????? ???????????????.
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

    private void CutOutOtherBoss(int boss)
    {
        List<ResRank> result = new List<ResRank>();

        for (int i = 0; i < getResult.ranks.Length; ++i)
        {
            if (getResult.ranks[i].stage == boss)
            {
                result.Add(getResult.ranks[i]);
            }
        }

        getResult.ranks = result.ToArray();
    }
}
