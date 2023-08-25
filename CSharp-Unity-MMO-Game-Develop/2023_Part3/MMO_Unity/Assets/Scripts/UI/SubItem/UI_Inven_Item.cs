using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Inven_Item : UI_Base
{
    enum GameObjects
    {
        ItemIcon,
        ItemNameTmp,
    }

    string _name;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        // ���ε�
        Bind<GameObject>(typeof(GameObjects));
        Get<GameObject>((int)GameObjects.ItemNameTmp).GetComponent<TextMeshProUGUI>().text = _name;

        // �������� Ŭ���� ��� ������ �˷��� ��
        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent((PointerEventData) => { Debug.Log($"Item Click! {_name}"); });
    }

    public void SetInfo(string name)
    {
        _name = name;
    }
}
