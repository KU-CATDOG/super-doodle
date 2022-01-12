using System.Collections.Generic;
using TMPro;
using Tool;
using UnityEngine;

public class Earth : MonoBehaviour
{
    private readonly HashSet<EarthObject> objects = new HashSet<EarthObject>();

    public IReadOnlyCollection<EarthObject> EarthObjects => objects;

    public int Radius { get; private set; } = 5;

    [SerializeField]
    private Transform objectSprite;

    [SerializeField]
    private TextMeshPro keyText;

    public KeyCode CurrentKeyCode { get; private set; }

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
    }

    public void KillEarthObject(EarthObject obj)
    {
        objects.Remove(obj);
        Destroy(obj);
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
            collisionInfo[(int)(i.ClampedRadian / (Mathf.PI / (collisionSystemDivisionNum / 2)))].Add(i);
        }
    }

    public HashSet<EarthObject> ProbeEarthObject(float radian, float radius)
    {
        var result = new HashSet<EarthObject>();

        radian = Mod(radian, Mathf.PI * 2);
        radius = Mathf.Clamp(radius, 0, Mathf.PI / 2 - 0.01f);

        var start = (int)Mathf.Floor((radian - radius) / (Mathf.PI / (collisionSystemDivisionNum / 2)));
        var end = (int)Mathf.Floor((radian + radius) / (Mathf.PI / (collisionSystemDivisionNum / 2)));

        for (var idx = start; idx <= end; idx++)
        {
            var isEdge = idx == start || idx == end;

            var i = idx;
            if (i < 0)
            {
                i += collisionSystemDivisionNum;
            }

            if (i > 11)
            {
                i -= collisionSystemDivisionNum;
            }

            if (isEdge)
            {
                foreach (var j in collisionInfo[i])
                {
                    var distanceRadian = Mathf.Abs(Mod(radian, Mathf.PI * 2) - Mod(j.Radian, Mathf.PI * 2));
                    if (distanceRadian > radius) continue;

                    result.Add(j);
                }
            }
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
