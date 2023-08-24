using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    // ��ųʸ��� �̿��Ͽ� ��ư/�ؽ�Ʈ ���� Object �迭�� ����ִ´�.
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    // ���׸�&���÷����� �̿��� Bind
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);

        // ���̸� ������, ��ųʸ��� �־��ش�.
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            // ���� ������Ʈ Ÿ���� ���
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Util.FindChild(gameObject, names[i], true);
            }
            // �� ���� Ÿ���� ���
            else
            {
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            }

            // ��ã�� ��� �α׸� �����.
            if (objects[i] == null)
                Debug.Log($"Failed to bind {names[i]}");
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        // �ƹ��͵� �����Ƿ� Null�� ��ȯ�Ѵ�.
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        // T�� ĳ�������ش�.
        return objects[idx] as T;
    }

    protected TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    public static void AddUIEvnt(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }
}
