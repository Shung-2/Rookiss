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
        // ����Ƽ ���̾��Ű �� ��ư �̸��� �����ϰ� �ۼ��Ѵ�,.
        PointButton
    }

    enum Texts
    {
        // ����Ƽ ���̾��Ű �� �ؽ�Ʈ �̸��� �����ϰ� �ۼ��Ѵ�.
        PointTmp,
        ScoreTmp
    }

    enum GameObjects
    {
        // ����Ƽ ���̾��Ű �� ���ӿ�����Ʈ �̸��� �����ϰ� �ۼ��Ѵ�.
        TestObject,
    }

    enum Images
    {
        // ����Ƽ ���̾��Ű �� �̹��� �̸��� �����ϰ� �ۼ��Ѵ�.
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

        // Utils�� �ִ� Extension�� �̿��Ͽ� BindEvent�� ����Ѵ�.
        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked, Define.UIEvent.Click);

        // �̹����� �ƴ� ���ӿ�����Ʈ�� �޾ƿ� ������
        // ���� ������ �̹��� ������ �ƴ�, UI_EventHandler�� �߰��ϰų�, �̹� �ִ� ��� �̸� �̿��Ͽ� �̺�Ʈ�� �����ϱ� ����.
        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);
    }

    int _score = 0;

    public void OnButtonClicked(PointerEventData data)
    {
        _score++;
        // �ؽ�Ʈ �����ؼ� �ؽ�Ʈ�� �ٲ��ش�.
        GetText((int)Texts.ScoreTmp).text = $"Score : {_score}";
    }
}
