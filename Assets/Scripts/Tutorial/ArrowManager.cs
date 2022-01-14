using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    private float speed = 1.0f;
    private float direction;
    private float timer;
    private int counter;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        direction = 1;
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // 처음 5초간은 "돔황챠에 오신것을 환영합니다"이므로 기다려야 함.
        if (counter == 0)
        {
            if (timer > 5)
            {
                counter++;
                timer = 0;
            }
        }

        // 화살표가 왔다갔다 하며 3번만 움직이게끔 함.
        else if (timer < 0.7 && counter < 6)
        {
            this.transform.Translate(direction * Vector3.left * speed * Time.deltaTime);
        }
        else if (counter < 6)
        {
            direction *= -1; // 방향바꾸기
            timer = 0;
            counter++;
        }

        // 이제 할 일을 다 마쳤으므로 화살표는 퇴장...
        if (counter == 6 && timer > 2)
        {
            this.transform.Translate(Vector3.forward*5);
            counter++;
        }
    }
}
