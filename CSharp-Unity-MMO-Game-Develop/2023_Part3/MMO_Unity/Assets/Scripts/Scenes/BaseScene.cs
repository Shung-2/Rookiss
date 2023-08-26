using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    // �ſ� ���� �ֻ��� �θ��̹Ƿ� ������ ���� ����Ѵ�.
    // ������Ƽ�� �̿��� �ܺο��� ������ �� �� �ֵ��� �ϰ�, Set�� �ڽĵ鸸 �� �� �ֵ��� �Ѵ�.
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        // �̺�Ʈ �ý����� �ִ��� Ȯ���Ѵ�.
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));

        // ������ �����Ѵ�.
        if (obj == null)
                Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    public abstract void Clear();
}