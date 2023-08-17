using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab: {path}");
            return null;
        }

        // 재귀를 피하기위해 Object를 붙여주어 명시한다.
        return Object.Instantiate(prefab, parent);
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        Object.Destroy(go);
    }
}
