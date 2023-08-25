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
        GameObject go = Object.Instantiate(prefab, parent);

        // Instantiate로 생성된 객체들에 (Clone)이 붙는 것을 지워준다.
        int index = go.name.IndexOf("(Clone)");
        if (index > 0)
        {
            go.name = go.name.Substring(0, index);
        }

        return go;
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
