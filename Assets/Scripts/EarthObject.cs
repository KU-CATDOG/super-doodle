using UnityEngine;

public enum EarthObjectType
{
    Player,
    Mob,
    Boss,
}

public sealed class EarthObject : MonoBehaviour
{
    /// <summary>
    /// 각으로 표현된 위치 (2pi보다 커질 수 있음에 주의! 진짜 각도로 쓰고 싶으면 % 2pi 해야 함)
    /// </summary>
    public float Radian { get; set; }

    /// <summary>
    /// 이동 속도 (한 바퀴를 도는 데에 몇 초 걸리는가)
    /// 시계방향(플레이어와 마왕이 움직이는 방향)을 양수로 두고, 작을수록 빠름
    /// </summary>
    public float MoveSpeed { get; set; } = 15;

    public Earth Earth { get; private set; }

    private EarthObjectController controller;

    /// <summary>
    /// 이거를 갈아치워주면 다른 유닛처럼 행동하기 시작함
    /// </summary>
    public EarthObjectController Controller
    {
        get => controller;
        set
        {
            if (controller != null)
            {
                controller.DetachThis();
            }
            controller = value;

            if (controller != null)
            {
                controller.AttachThis(this);
            }
        }
    }

    private void Awake()
    {
        Earth = GetComponentInParent<Earth>();
    }

    private void Update()
    {
        if (controller == null) return;

        // 관련 리소스가 로드되지 않은 상태에선 행동을 멈춘다.
        if (!controller.IsResourceLoaded) return;

        SetPosition();

        controller.OnUpdate();
    }

    public void SetPosition()
    {
        // 모든 오브젝트 공통으로 위치와 속도에 따라 움직이게 한다.
        Radian += 2 * Mathf.PI / MoveSpeed * Time.deltaTime;

        var currentEulerAngle = transform.localEulerAngles;
        currentEulerAngle.z = -Radian / (2 * Mathf.PI) * 360;
        transform.localEulerAngles = currentEulerAngle;
    }

    private void OnDestroy()
    {
        Controller = null;
    }
}
