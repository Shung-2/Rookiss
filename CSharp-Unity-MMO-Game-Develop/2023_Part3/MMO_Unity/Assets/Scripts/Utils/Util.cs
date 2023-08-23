using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    /** ���� �� : �ֻ��� �θ�, �̸�, ����� Ž�� ����*/
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