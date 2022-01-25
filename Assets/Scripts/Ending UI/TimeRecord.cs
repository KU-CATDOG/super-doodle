using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeRecord : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro record;
    private float time_record = Timer.CurrentTime();

    // Start is called before the first frame update
    void Start()
    {
        record.text = "Record: " + time_record.ToString("N2"); 
    }

}
