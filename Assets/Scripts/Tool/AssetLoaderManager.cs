using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoaderManager : SingletonBehavior<AssetLoaderManager>
{
    private readonly Dictionary<string, UnityEngine.Object> LoadedDict =
            new Dictionary<string, UnityEngine.Object>();


    /// <summary>
    /// Assets/Prefab 아래의 프리팹을 로드하는 루틴을 리턴한다.
    /// </summary>
    public IEnumerator LoadPrefabAsync<T>(string prefabAddress, Action<T> onFinished) where T : UnityEngine.Object
    {
        if (LoadedDict.TryGetValue(prefabAddress, out var v))
        {
            onFinished((T)v);
            yield break;
        }

        var op = Resources.LoadAsync<T>("Prefabs/" + prefabAddress);

        yield return op;

        if (!op.isDone)
        {
            onFinished(null);
            yield break;
        }

        LoadedDict[prefabAddress] = op.asset;
        onFinished(op.asset as T);
    }

    /// <summary>
    /// Assets/Prefab 아래의 프리팹을 로드하고 로드가 끝날 때까지 기다린다. (여러 개를 연속으로 로드할 때 쓰면 심각한 퍼포먼스 저하가 있을 수도 있음)
    /// </summary>
    public T LoadPrefab<T>(string prefabAddress) where T : UnityEngine.Object
    {
        if (LoadedDict.TryGetValue(prefabAddress, out var v))
        {
            return (T)v;
        }

        var op = Resources.Load<T>("Prefabs/" + prefabAddress);

        if (op == null)
        {
            Debug.LogWarning($"No prefab loaded (Prefabs/{prefabAddress}.prefab)");
            return null;
        }

        LoadedDict[prefabAddress] = op;
        return op;
    }
}
