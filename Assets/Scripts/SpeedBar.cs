using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBar : MonoBehaviour
{
    private EarthObject player;
    private Earth earth;
    private Test.EarthTester test;
    Image radial;
    float maxSpeed = Mathf.PI / 2; // 현재 설정값
    float curSpeed;
    public void Initialize()
    {
        test = FindObjectOfType<Test.EarthTester>();
        earth = test.GetComponentInChildren<Earth>();
        player = earth.Player;
        radial = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        curSpeed = Mathf.Abs(player.MoveSpeed);
        radial.fillAmount = curSpeed / maxSpeed;
    }
}
