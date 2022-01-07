using UnityEngine;

public abstract class EarthObject : MonoBehaviour
{
    /// <summary>
    /// 각으로 표현된 위치 (2pi보다 커질 수 있음에 주의!)
    /// </summary>
    public float Radian { get; set; }

    /// <summary>
    /// 이동 속도 (한 바퀴를 도는 데에 몇 초 걸리는가)
    /// 시계방향(플레이어가 움직이는 방향)을 양수로 두고, 작을수록 빠름
    /// </summary>
    public float MoveSpeed { get; set; }

    private void Update()
    {
        Radian += 2 * Mathf.PI / MoveSpeed * Time.deltaTime;

        var currentEulerAngle = transform.localEulerAngles;
        currentEulerAngle.z = Radian / (2 * Mathf.PI) * 360;
        transform.localEulerAngles = currentEulerAngle;
    }
}
