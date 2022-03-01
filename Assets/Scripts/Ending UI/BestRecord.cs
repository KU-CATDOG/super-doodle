using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BestRecord : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro bestrecord;
    private float cur_record;
    private static float best_time_record = 0;

    [Header("TopRecords")]
    public RankController rankController;
    [SerializeField] private TextMeshPro[] bestRecordNameTexts;
    [SerializeField] private TextMeshPro[] bestRecordTexts;

    // Start is called before the first frame update
    void Start()
    {
        cur_record = Time.time - GameManager.Inst.tempTimer;
        if ((best_time_record == 0 || cur_record <= best_time_record) && GameManager.Inst.isRecentGameWin)
        {
            best_time_record = cur_record;
        }
        
        bestrecord.text = "Best: " + (best_time_record != 0 ? best_time_record.ToString("N2") : "--:--");

        rankController.GetRanks(0, 3, res =>
        {
            int idx = 0;
            foreach (var rank in res)
            {
                bestRecordNameTexts[idx].text = rank.name;
                bestRecordTexts[idx++].text = $"{(rank.record / 1000f):N2}";
            }
        });
    }
}

    
