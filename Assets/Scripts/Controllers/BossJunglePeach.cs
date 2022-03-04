using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJunglePeach : MonoBehaviour
{
    private EarthObject bossJungle;

    public void Init(EarthObject boss)
    {
        bossJungle = boss;
    }

    private void Start()
    {
        StartCoroutine(PeachCoroutine());
    }

    private IEnumerator PeachCoroutine()
    {
        float timer = 0;
        while (timer < 2f)
        {
            timer += Time.deltaTime;
            transform.localPosition += transform.localPosition.normalized * Time.deltaTime;

            yield return null;
        }

        bossJungle.MoveSpeed *= 1.3f;
        Debug.Log("이동 속도 증가!");

        Destroy(gameObject);
    }
}
