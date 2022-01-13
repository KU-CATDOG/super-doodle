using System.Collections.Generic;
using TMPro;
using Tool;
using UnityEngine;

public class Earth : MonoBehaviour
{
    private readonly HashSet<EarthObject> objects = new HashSet<EarthObject>();

    /// <summary>
    /// 이 땅 위에 올라와 있는 모든 오브젝트
    /// </summary>
    public IReadOnlyCollection<EarthObject> EarthObjects => objects;

    /// <summary>
    /// 크기 (보여주기 용도)
    /// </summary>
    public int Radius { get; private set; } = 5;

    [SerializeField]
    private Transform objectSprite;

    [SerializeField]
    private TextMeshPro keyText;

    public KeyCode CurrentKeyCode { get; private set; }

    // 충돌 탐지를 위해서 이 땅을 몇 구획으로 쪼갤 것인가
    private const int collisionSystemDivisionNum = 12;

    private readonly HashSet<EarthObject>[] collisionInfo = new HashSet<EarthObject>[collisionSystemDivisionNum];

    private void Awake()
    {
        MessageSystem.Instance.Subscribe<SingleKeyPressedEvent>(OnSingleKeyEvent);

        for (var i = 0; i < collisionInfo.Length; i++)
        {
            collisionInfo[i] = new HashSet<EarthObject>();
        }
    }

    private void Start()
    {
        objectSprite.localScale = Vector3.one * Radius;
        UpdateKeyCode();
    }

    private void Update()
    {
        UpdateCollisionInfo();
    }

    private void OnDestroy()
    {
        MessageSystem.Instance.Unsubscribe<SingleKeyPressedEvent>(OnSingleKeyEvent);
    }

    public void AddEarthObject(EarthObject obj)
    {
        obj.transform.parent = transform;
        objects.Add(obj);

        UpdateCollisionInfo();
    }

    public void KillEarthObject(EarthObject obj)
    {
        objects.Remove(obj);
        Destroy(obj);

        UpdateCollisionInfo();
    }

    private void OnSingleKeyEvent(IEvent e)
    {
        if (!(e is SingleKeyPressedEvent se)) return;

        var isCorrect = se.PressedKey == CurrentKeyCode;

        if (isCorrect)
        {
            UpdateKeyCode();
        }

        foreach (var i in objects)
        {
            i.Controller.OnEarthKeyPressed(isCorrect);
        }
    }

    // 새로운 키로 갱신한다.
    private void UpdateKeyCode()
    {
        CurrentKeyCode = KeyGenerator.GetKeyCode();
        keyText.text = CurrentKeyCode.ToString();
    }

    private void UpdateCollisionInfo()
    {
        foreach (var i in collisionInfo)
        {
            i.Clear();
        }

        foreach (var i in objects)
        {
            // 현재 오브젝트의 위치에 따라 인덱스를 정한다.
            collisionInfo[(int)(i.ClampedRadian / (Mathf.PI / (collisionSystemDivisionNum / 2)))].Add(i);
        }
    }

    /// <summary>
    /// 주어진 Radian 지점 앞뒤로 Radius 라디안 안에 있는 오브젝트를 전부 리턴한다.
    /// </summary>
    public HashSet<EarthObject> ProbeEarthObject(float radian, float radius)
    {
        var result = new HashSet<EarthObject>();

        radian = Mod(radian, Mathf.PI * 2);
        radius = Mathf.Clamp(radius, 0, Mathf.PI / 2 - 0.01f);

        // 구획 탐색의 시작점과 끝 점을 구한다.
        var start = (int)Mathf.Floor((radian - radius) / (Mathf.PI / (collisionSystemDivisionNum / 2)));
        var end = (int)Mathf.Floor((radian + radius) / (Mathf.PI / (collisionSystemDivisionNum / 2)));

        for (var idx = start; idx <= end; idx++)
        {
            var isEdge = idx == start || idx == end;

            var i = idx;

            // 무조건 0 ~ num 사이로 맞춰야 한다.
            if (i < 0)
            {
                i += collisionSystemDivisionNum;
            }

            if (i > 11)
            {
                i -= collisionSystemDivisionNum;
            }

            // 끝자락에 있는 놈들은 안쪽에서 거리를 재서 나눈다.
            if (isEdge)
            {
                foreach (var j in collisionInfo[i])
                {
                    var distanceRadian = Mathf.Abs(Mod(radian, Mathf.PI * 2) - Mod(j.Radian, Mathf.PI * 2));
                    if (distanceRadian > radius) continue;

                    result.Add(j);
                }
            }
            // 끝자락이 아니면 무조건 포함된다.
            else
            {
                foreach (var j in collisionInfo[i])
                {
                    result.Add(j);
                }
            }
        }

        return result;
    }

    private float Mod(float x, float m)
    {
        float r = x % m;
        return r < 0 ? r + m : r;
    }
}
