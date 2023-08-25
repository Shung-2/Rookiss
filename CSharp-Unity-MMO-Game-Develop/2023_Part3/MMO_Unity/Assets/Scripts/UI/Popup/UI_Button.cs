using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
    enum Buttons
    {
        // 유니티 하이어라키 뷰 버튼 이름과 동일하게 작성한다,.
        PointButton
    }

    enum Texts
    {
        // 유니티 하이어라키 뷰 텍스트 이름과 동일하게 작성한다.
        PointTmp,
        ScoreTmp
    }

    enum GameObjects
    {
        // 유니티 하이어라키 뷰 게임오브젝트 이름과 동일하게 작성한다.
        TestObject,
    }

    enum Images
    {
        // 유니티 하이어라키 뷰 이미지 이름과 동일하게 작성한다.
        ItemIcon,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        // Utils에 있는 Extension을 이용하여 BindEvent를 사용한다.
        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked, Define.UIEvent.Click);

        // 이미지가 아닌 게임오브젝트를 받아온 이유는
        // 지금 당장은 이미지 설정이 아닌, UI_EventHandler를 추가하거나, 이미 있는 경우 이를 이용하여 이벤트를 연동하기 위함.
        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);
    }

    int _score = 0;

    public void OnButtonClicked(PointerEventData data)
    {
        _score++;
        // 텍스트 접근해서 텍스트를 바꿔준다.
        GetText((int)Texts.ScoreTmp).text = $"Score : {_score}";
    }
}
