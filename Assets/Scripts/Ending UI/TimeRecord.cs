using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeRecord : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro record;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Inst.isRecentGameWin)
        {
            record.text = $"Record: {(Time.time - GameManager.Inst.tempTimer).ToString("N2")}";
        }
        else
        {
            record.text = "Record: --:--";
        }
    }

}
