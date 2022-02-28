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

    // Start is called before the first frame update
    void Start()
    {
        cur_record = Time.time - GameManager.Inst.tempTimer;
        if ((best_time_record == 0 || cur_record <= best_time_record) && GameManager.Inst.isRecentGameWin)
        {
            best_time_record = cur_record;
        }
        
        bestrecord.text = "Best: " + (best_time_record != 0 ? best_time_record.ToString("N2") : "--:--");
    }
}

    
