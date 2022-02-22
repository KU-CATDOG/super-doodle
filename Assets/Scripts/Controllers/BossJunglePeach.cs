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
        yield return new WaitForSeconds(1f);

        bossJungle.MoveSpeed *= 1.1f;
        Debug.Log("이동 속도 증가!");

        Destroy(gameObject);
    }
}
