using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        // 신 타입 설정
        SceneType = Define.Scene.Game;

        // UI 매니저에 UI를 띄우는 함수를 호출한다.
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
