using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeRecord : MonoBehaviour
{
    public RankController rankController;
    [SerializeField] private TextMeshPro record;
    [SerializeField] private TextMeshPro[] ranks;
    [SerializeField] private TextMeshProUGUI submitName;

    [SerializeField] private GameObject[] toHideOnDefeat;

    private float endTime;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Inst.isRecentGameWin)
        {
            endTime = Time.time;
            float timeSecond = endTime - GameManager.Inst.tempTimer;
            record.text = $"Record: {timeSecond:N2}";

            rankController.GetRanks(Mathf.FloorToInt(timeSecond * 1000), (res) =>
            {
                for (int i = 0; i < ranks.Length; ++i)
                {
                    if (i != 3)
                    {
                        ranks[i].text = "-";
                    }
                    else
                    {
                        ranks[i].text = $"YOU : {timeSecond:N2}";
                    }
                }

                int idx = 0;
                foreach (var rank in res)
                {
                    if (rank != null)
                    {
                        if (idx != 3)
                        {
                            ranks[idx++].text = $"{rank.name} : {(rank.record / 1000f):N2}\n";
                        }
                    }
                }
            });
        }
        else
        {
            foreach (var toHide in toHideOnDefeat)
            {
                toHide.SetActive(false);
            }

            record.text = "Record: --:--";
            foreach (var rank in ranks)
            {
                rank.text = "";
            }
            ranks[3].text = "FAILED...";
        }
    }

    public void SubmitScore()
    {
        var toSend = new ReqScore
        {
            name = submitName.text.Trim().Substring(0, 4),
            record = Mathf.FloorToInt((endTime - GameManager.Inst.tempTimer) * 1000),
            stage = (int)GameManager.Inst.currentBoss
        };
        rankController.SendScore(toSend);
    }
}
