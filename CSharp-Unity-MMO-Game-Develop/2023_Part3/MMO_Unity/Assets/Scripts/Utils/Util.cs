using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{ 
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();

        // ���� ������Ʈ�� ������, ������Ʈ�� �߰��Ѵ�.
        if (component == null)
            component = go.AddComponent<T>();

        // ������Ʈ�� ������ ������Ʈ�� �����Ѵ�.
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        // ���� ������Ʈ�� Transform�� ������ �ֱ� ������ FindChild �Լ��� �����Ѵ�.
        Transform transform = FindChild<Transform>(go, name, recursive);

        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        // �ֻ��� ��ü�� ���� ��� �ٷ� Null �����Ѵ�.
        if (go == null)
            return null;

        // ����� Ž���� ���� ���� ���
        if (recursive == false)
        {
            for (int i = 0; i< go.transform.childCount; i++)
            {
                // GetChild = ���� �ڽ��� ã�� �Լ�
                Transform transform = go.transform.GetChild(i);

                // �̸����� ã�ƺ���.
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        // ����� Ž���� �� ���
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                // ���� �̸��� ã�Ҵ�!
                if (string.IsNullOrEmpty (name) || component.name == name)
                    return component;
            }
        }

        // ����� Ž���� ������� ã�� ������ ��� Null ����
        return null;
    }
}