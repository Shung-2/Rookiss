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

        Dictionary<int, Data.Stat> dic = Managers.Data.StatDict;

        // 커서 컨트롤러를 추가한다.
        gameObject.GetOrAddComponent<CursorController>();
    }

    public override void Clear()
    {

    }
}
