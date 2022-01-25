using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BestRecord : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro bestrecord;
    private float cur_record = Timer.CurrentTime();
    private static float best_time_record = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (best_time_record == 0)
            best_time_record = cur_record;
        else
        {
            if (cur_record <= best_time_record)
                best_time_record = cur_record;
        }
        
        bestrecord.text = "Best Record: " + best_time_record.ToString("N2");
    }
}

    
