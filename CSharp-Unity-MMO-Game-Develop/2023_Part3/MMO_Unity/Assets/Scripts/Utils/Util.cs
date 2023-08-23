using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    /** 인자 값 : 최상위 부모, 이름, 재귀적 탐색 여부*/
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