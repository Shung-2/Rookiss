using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        // Ÿ���� �������� ���
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');

            // �������� �̸��� �����Ѵ�.
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
        // 1. prefab�� �������� ��ü�̸�, Original�� �̹� ��� ������ �ٷ� ����Ѵ�.
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab: {path}");
            return null;
        }

        // original�� Poolable ������Ʈ�� ���������, Ǯ���� �����Ѵ�.
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        // ��͸� ���ϱ����� Object�� �ٿ��־� ����Ѵ�.
        // Ǯ���� ������� �ʴ� ������Ʈ��� ���� �ڵ带 �̿��� �ν��Ͻ��� �����Ѵ�.
        GameObject go = Object.Instantiate(original, parent);
        // Instantiate�� ������ ��ü�鿡 (Clone)�� �ٴ� ���� �����ش�.
        go.name = original.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        // ���࿡ Ǯ���� ����ϴ� ��ü���, Ǯ�� �Ŵ������� ��Ź�� �Ѵ�.
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}
