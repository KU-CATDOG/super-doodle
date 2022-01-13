using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Tool
{
    public static class AssetLoader
    {
        private static readonly Dictionary<string, AsyncOperationHandle> LoadedDict =
            new Dictionary<string, AsyncOperationHandle>();

        /// <summary>
        /// Assets/Prefab 아래의 프리팹을 로드하는 루틴을 리턴한다.
        /// </summary>
        public static IEnumerator LoadPrefabAsync<T>(string prefabAddress, Action<T> onFinished) where T : UnityEngine.Object
        {
            if (LoadedDict.TryGetValue(prefabAddress, out var v))
            {
                onFinished((T)v.Result);
                yield break;
            }

            var op = Addressables.LoadAssetAsync<T>("Assets/Prefabs/" + prefabAddress + ".prefab");

            yield return op;

            if (op.Status != AsyncOperationStatus.Succeeded)
            {
                onFinished(null);
                yield break;
            }

            LoadedDict[prefabAddress] = op;
            onFinished(op.Result);
        }

        /// <summary>
        /// Assets/Prefab 아래의 프리팹을 로드하고 로드가 끝날 때까지 기다린다. (여러 개를 연속으로 로드할 때 쓰면 심각한 퍼포먼스 저하가 있을 수도 있음)
        /// </summary>
        public static T LoadPrefab<T>(string prefabAddress) where T : UnityEngine.Object
        {
            if (LoadedDict.TryGetValue(prefabAddress, out var v))
            {
                return (T)v.Result;
            }

            var op = Addressables.LoadAssetAsync<T>("Assets/Prefabs/" + prefabAddress + ".prefab");

            op.WaitForCompletion();

            if (op.Status != AsyncOperationStatus.Succeeded)
            {
                return null;
            }

            LoadedDict[prefabAddress] = op;
            return op.Result;
        }
    }
}
