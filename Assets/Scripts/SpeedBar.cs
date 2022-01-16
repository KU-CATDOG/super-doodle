using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBar : MonoBehaviour
{
    private EarthObject player;
    private Earth earth;
    private Test.EarthTester test;
    [SerializeField] TextMesh text;
    public void Initialize()
    {
        test = FindObjectOfType<Test.EarthTester>();
        earth = test.GetComponentInChildren<Earth>();
        player = earth.Player;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = player.MoveSpeed.ToString("F1");
    }
}
