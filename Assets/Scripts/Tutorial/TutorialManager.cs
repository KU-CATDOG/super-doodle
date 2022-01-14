using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private Text tutorialText;

    private float timer;
    private int timeToChange;
    private int counter;
    private string[] texts = { 
        "돔황챠의 세계에 오신 것을 환영합니다",
        "지구 중앙에 제시된 키를 누르면 플레이어가 빨라집니다",
        "우리의 목표는 나쁜 적들의 뒤를 잡아 물리치는 것입니다!",
        "이제 곧 적이 움직입니다... 빠르게 키를 연타하세요!"
    };

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        tutorialText.text = "";
        timer = 0.0f;
        timeToChange = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 시간에 따라 튜토리얼 텍스트가 변화하게 해야 함.
        timer += Time.deltaTime;
        
        if (counter < 4 && timer > timeToChange)
        {
            tutorialText.text = texts[counter];
            timeToChange += 5;
            counter++;
        }

        // 텍스트들을 모두 보여줬으므로, 본 게임 (보스0)으로 넘어가기
        if (counter == 4 && timer > timeToChange)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
