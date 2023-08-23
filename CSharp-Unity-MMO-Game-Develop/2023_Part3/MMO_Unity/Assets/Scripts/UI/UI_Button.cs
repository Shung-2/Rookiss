using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    // ��ųʸ��� �̿��Ͽ� ��ư/�ؽ�Ʈ ���� Object �迭�� ����ִ´�.
    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    enum Buttons
    {
        PointButton
    }

    enum Texts
    {
        PointTmp,
        ScoreTmp
    }

    private void Start()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
    }

    // ���׸�&���÷����� �̿��� Bind
    void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);

        // ���̸� ������, ��ųʸ��� �־��ش�.
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            objects[i] = Util.FindChild<T>(gameObject, names[i], true);
        }
    }

    int _score = 0;

    public void OnButtonClicked()
    {
        _score++;
    }
}
