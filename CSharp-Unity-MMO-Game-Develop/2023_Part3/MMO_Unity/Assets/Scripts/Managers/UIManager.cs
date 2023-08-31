using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int _order = 10;
    
    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    // �׳� �˾��� ������ ���, ���̾��Ű�信 �˾��� �߱��������� �����Ǳ� ������
    // UI_Root �θ� ��ü�� ����� �� ��, �� �ȿ� ����ش�.
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };

            return root;
        }
    }

    // �ܺο��� �˾� UI�� ���� ���, Manager�� ���� SetCanvas�� ��û�Ͽ� SetOrder�� ���� ������ �����ش�.
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        // ������ ��û�� ���
        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        // ������ ��û���� ���� ��� (�˾��� �ƴ� �Ϲ� UI�� ���)
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");

        // �θ� �����Ѵ�.
        if (parent != null)
            go.transform.SetParent(parent);

        // ĵ���� ����
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return Util.GetOrAddComponent<T>(go);
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        // �θ� �����Ѵ�.
        if (parent != null)
            go.transform.SetParent(parent);

        return Util.GetOrAddComponent<T>(go);
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        // �̸��� ���� ���� �ʾҴٸ�, <T>�� �̸��� ����ϰڴ�.
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");

        // ������Ʈ�� ���´�.
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        // popup�� ��ȯ�Ѵ�.
        return sceneUI;
    }

    // ���ڰ��� �������� �̸��� �޴´�.
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        // �̸��� ���� ���� �ʾҴٸ�, <T>�� �̸��� ����ϰڴ�.
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        
        // ������Ʈ�� ���´�.
        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);
        
        // popup�� ��ȯ�Ѵ�.
        return popup;
    }

    // �˾��� Ȯ���� ���Ŀ� �ݾ��ش�. (�˾��� �������� ���)
    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        // �˾��� �ƴ� ��� �α׸� �����ش�.
        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        // ���� ������ üũ�Ѵ�.
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        // �˾��� �����ش�.
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;

        _order--;
    }

    // ��� �˾��� �����ش�
    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
