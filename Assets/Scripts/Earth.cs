using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour
{
    private readonly HashSet<EarthObject> objects = new HashSet<EarthObject>();

    public IReadOnlyCollection<EarthObject> EarthObjects => objects;

    public int Radius { get; private set; } = 5;

    [SerializeField]
    private Transform objectSprite;

    private void Start()
    {
        objectSprite.localScale = Vector3.one * Radius;
    }

    public void RemoveEarthObject(EarthObject obj)
    {
        objects.Remove(obj);
    }
}
