using System.Collections.Generic;
using UnityEngine;
using Tool;
using TMPro;

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

    private void Awake()
    {
        MessageSystem.Instance.Subscribe<SingleKeyPressedEvent>(OnSingleKeyEvent);
    }

    private void Start()
    {
        objectSprite.localScale = Vector3.one * Radius;
        UpdateKeyCode();
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
}
