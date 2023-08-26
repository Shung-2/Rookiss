using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    // 신에 대한 최상위 부모이므로 디파인 값을 사용한다.
    // 프로퍼티를 이용해 외부에서 정보를 알 수 있도록 하고, Set은 자식들만 할 수 있도록 한다.
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        // 이벤트 시스템이 있는지 확인한다.
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));

        // 없으면 생성한다.
        if (obj == null)
                Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    public abstract void Clear();
}