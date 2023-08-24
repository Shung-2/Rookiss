using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{ 
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();

        // 만약 컴포넌트가 없으면, 컴포넌트를 추가한다.
        if (component == null)
            component = go.AddComponent<T>();

        // 컴포넌트가 있으면 컴포넌트를 리턴한다.
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        // 게임 오브젝트는 Transform을 가지고 있기 때문에 FindChild 함수를 재사용한다.
        Transform transform = FindChild<Transform>(go, name, recursive);

        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        // 최상위 객체가 없을 경우 바로 Null 리턴한다.
        if (go == null)
            return null;

        // 재귀적 탐색을 하지 않을 경우
        if (recursive == false)
        {
            for (int i = 0; i< go.transform.childCount; i++)
            {
                // GetChild = 직속 자식을 찾는 함수
                Transform transform = go.transform.GetChild(i);

                // 이름으로 찾아본다.
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        // 재귀적 탐색을 할 경우
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                // 같은 이름을 찾았다!
                if (string.IsNullOrEmpty (name) || component.name == name)
                    return component;
            }
        }

        // 재귀적 탐색과 관계없이 찾지 못했을 경우 Null 리턴
        return null;
    }
}