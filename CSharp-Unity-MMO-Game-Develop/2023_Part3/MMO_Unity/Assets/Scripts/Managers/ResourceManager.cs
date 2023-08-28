using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        // 타입이 프리팹일 경우
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');

            // 프리팹의 이름을 추출한다.
            if (index >= 0)
            {
                name = name.Substring(index + 1);
            }

            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        // 1. prefab은 오리지날 객체이며, Original을 이미 들고 있으면 바로 사용한다.
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab: {path}");
            return null;
        }

        // original이 Poolable 컴포넌트를 들고있으면, 풀링을 진행한다.
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        // 재귀를 피하기위해 Object를 붙여주어 명시한다.
        // 풀링을 사용하지 않는 오브젝트라면 기존 코드를 이용해 인스턴스를 진행한다.
        GameObject go = Object.Instantiate(original, parent);
        // Instantiate로 생성된 객체들에 (Clone)이 붙는 것을 지워준다.
        go.name = original.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        // 만약에 풀링을 사용하는 객체라면, 풀링 매니저에게 위탁을 한다.
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}
