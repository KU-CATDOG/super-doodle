using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private GameObject cameraObject;
    private EarthObject player;
    private Earth earth;
    private Test.EarthTester test;
    float curSpeed;
    public void Start()
    {
        test = FindObjectOfType<Test.EarthTester>();
        earth = test.GetComponentInChildren<Earth>();
        player = earth.Player;
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        cameraObject.GetComponent<Camera>().orthographicSize = 5;
    }

    // Update is called once per frame
    void Update()
    {
        curSpeed = Mathf.Abs(player.MoveSpeed);
        // 카메라 size = 현재 속도 + 5임.
        // 카메라 사이즈가 현재속도보다 작다면 -> 카메라 사이즈는 커져야함.
        if (cameraObject.GetComponent<Camera>().orthographicSize <= 15 && cameraObject.GetComponent<Camera>().orthographicSize < curSpeed + 5)
        {
            cameraObject.GetComponent<Camera>().orthographicSize += 0.01f;
        }
        else if (cameraObject.GetComponent<Camera>().orthographicSize >= 5 && cameraObject.GetComponent<Camera>().orthographicSize > curSpeed + 5)
        {
            cameraObject.GetComponent<Camera>().orthographicSize -= 0.01f;
        }
    }
}
