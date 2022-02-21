using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeRecord : MonoBehaviour
{
    public RankController rankController;
    [SerializeField]
    private TextMeshPro record;
    [SerializeField]
    private TextMeshPro ranks;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Inst.isRecentGameWin)
        {
            float timeSecond = Time.time - GameManager.Inst.tempTimer;
            record.text = $"Record: {timeSecond:N2}";

            var toSend = new ReqScore();
            toSend.name = "TESTNAME";
            toSend.record = Mathf.FloorToInt(timeSecond * 1000);
            toSend.stage = (int)GameManager.Inst.currentBoss;
            rankController.SendScore(toSend, () =>
            {
                rankController.GetRanks(0, 8, (res) =>
                {
                    ranks.text = "";
                    foreach (var rank in res)
                    {
                        if (rank != null)
                        {
                            ranks.text += $"{rank.name} : {(rank.record / 1000f):N2}\n";
                        }
                    }
                });
            });
        }
        else
        {
            record.text = "Record: --:--";
            rankController.GetRanks(0, 8, (res) =>
            {
                ranks.text = "";
                foreach (var rank in res)
                {
                    if (rank != null)
                    {
                        ranks.text += $"{rank.name} : {(rank.record / 1000f):N2}\n";
                    }
                }
            });
        }
    }

}
