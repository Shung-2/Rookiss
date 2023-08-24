using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Base
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
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        // Utils�� �ִ� Extension�� �̿��Ͽ� AddUIEvnt�� ����Ѵ�.
        GetButton((int)Buttons.PointButton).gameObject.AddUIEvnt(OnButtonClicked, Define.UIEvent.Click);

        // �̹����� �ƴ� ���ӿ�����Ʈ�� �޾ƿ� ������
        // ���� ������ �̹��� ������ �ƴ�, UI_EventHandler�� �߰��ϰų�, �̹� �ִ� ��� �̸� �̿��Ͽ� �̺�Ʈ�� �����ϱ� ����.
        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        AddUIEvnt(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);
    }

    int _score = 0;

    public void OnButtonClicked(PointerEventData data)
    {
        _score++;
        // �ؽ�Ʈ �����ؼ� �ؽ�Ʈ�� �ٲ��ش�.
        GetText((int)Texts.ScoreTmp).text = $"Score : {_score}";
    }
}
