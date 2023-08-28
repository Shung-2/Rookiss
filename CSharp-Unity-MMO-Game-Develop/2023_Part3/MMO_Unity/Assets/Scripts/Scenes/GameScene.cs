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

        for (int i = 0; i < 5; i++)
        {
            Managers.Resource.Instantiate("UnityChan");
        }
    }

    public override void Clear()
    {

    }
}
