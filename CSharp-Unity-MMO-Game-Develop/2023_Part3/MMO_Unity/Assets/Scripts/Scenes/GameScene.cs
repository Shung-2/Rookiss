using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        // �� Ÿ�� ����
        SceneType = Define.Scene.Game;

        // UI �Ŵ����� UI�� ���� �Լ��� ȣ���Ѵ�.
        Managers.UI.ShowSceneUI<UI_Inven>();

        Dictionary<int, Data.Stat> dic = Managers.Data.StatDict;

        // Ŀ�� ��Ʈ�ѷ��� �߰��Ѵ�.
        gameObject.GetOrAddComponent<CursorController>();
    }

    public override void Clear()
    {

    }
}
