using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeRecord : MonoBehaviour
{
    public RankController rankController;
    [SerializeField] private TextMeshPro record;
    [SerializeField] private TextMeshPro[] ranks;
    [SerializeField] private TextMeshProUGUI submitName;
    [SerializeField] private Button submitButton;

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

            rankController.GetRanks((int)GameManager.Inst.currentBoss, Mathf.FloorToInt(timeSecond * 1000), (res) =>
            {
                for (int i = 0; i < ranks.Length; ++i)
                {
                    if (i != 3)
                    {
                        ranks[i].text = "-";
                    }
                    else
                    {
                        ranks[i].text = $"*YOU* : {timeSecond:N2}";
                    }
                }

                int idx = 0;
                foreach (var rank in res)
                {
                    if (rank != null)
                    {
                        Debug.Log($"{rank.name}, {idx}");
                        if (idx == 3)
                        {
                            idx++;
                        }
                        ranks[idx].text = $"{rank.name} : {(rank.record / 1000f):N2}";
                    }
                    idx++;
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
            name = submitName.text.Trim().Substring(0, Mathf.Min(submitName.text.Length, 4)),
            record = Mathf.FloorToInt((endTime - GameManager.Inst.tempTimer) * 1000),
            stage = (int)GameManager.Inst.currentBoss
        };
        rankController.SendScore(toSend, () =>
        {
            ranks[3].text = $"{toSend.name} : {(toSend.record / 1000f):N2}";
            submitButton.interactable = false;
        });
    }
}
