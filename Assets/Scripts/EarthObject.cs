using UnityEngine;

public sealed class EarthObject : MonoBehaviour
{
    /// <summary>
    /// 각으로 표현된 위치 (2pi보다 커질 수 있음에 주의! 진짜 각도로 쓰고 싶으면 % 2pi 해야 함)
    /// </summary>
    public float Radian { get; set; }

    /// <summary>
    /// 0 ~ 2pi 사이로 표현된 위치
    /// </summary>
    public float ClampedRadian => Mod(Radian, Mathf.PI * 2);

    /// <summary>
    /// 이동 속도 (1초에 몇 라디안을 가는가)
    /// 시계방향(플레이어와 마왕이 움직이는 방향)을 양수로 둠
    /// </summary>
    public float MoveSpeed { get; set; }

    /// <summary>
    /// 이 오브젝트가 올라가 있는 땅
    /// </summary>
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
            controller?.DetachThis();
            controller = value;
            controller?.AttachThis(this);
        }
    }

    private void Awake()
    {
        Earth = GetComponentInParent<Earth>();
    }

    private void Update()
    {
        if (!(controller is { IsResourceLoaded: true })) return;

        // 관련 리소스가 로드되지 않은 상태에선 행동을 멈춘다.

        SetPosition();
        TryAttack();

        controller.OnUpdate();
    }

    private void OnDestroy()
    {
        Controller = null;
    }

    private void TryAttack()
    {
        // 다른 오브젝트를 못 때리는 상태라면
        if (!controller.CanAttackOtherObject) return;

        var attackRange = Mathf.PI / 30;

        foreach (var target in Earth.ProbeEarthObject(Radian + attackRange / 2, attackRange))
        {
            var c = target.controller;

            // 같은 편은 안 때린다.
            if (c.Side == controller.Side) continue;

            // 전부 로딩되지 않은 경우에도 넘어간다.
            if (!c.IsResourceLoaded) continue;

            c.MeleeAttackThis(this);
        }
    }

    private void SetPosition()
    {
        // 모든 오브젝트 공통으로 위치와 속도에 따라 움직이게 한다.
        Radian += MoveSpeed * Time.deltaTime;

        var t = transform;

        var currentEulerAngle = t.localEulerAngles;
        currentEulerAngle.z = -Radian / (2 * Mathf.PI) * 360;
        t.localEulerAngles = currentEulerAngle;
    }

    private float Mod(float x, float m)
    {
        var r = x % m;
        return r < 0 ? r + m : r;
    }
}
