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
    private TextMeshPro[] ranks;

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
            rankController.GetRanks(toSend.record, (res) =>
            {
                int idx = 0;
                foreach (var rank in res)
                {
                    if (rank != null)
                    {
                        ranks[idx++].text = $"{rank.name} : {(rank.record / 1000f):N2}\n";
                    }
                }

                rankController.SendScore(toSend, () =>
                {

                });
            });
        }
        else
        {
            record.text = "Record: --:--";
            foreach (var rank in ranks)
            {
                rank.text = "";
            }
            ranks[3].text = "FAILED...";
        }
    }

}
