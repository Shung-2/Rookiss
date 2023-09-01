using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int _order = 10;
    
    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    // 그냥 팝업을 생성할 경우, 하이어라키뷰에 팝업이 중구난방으로 생성되기 때문에
    // UI_Root 부모 객체를 만들어 준 후, 그 안에 담아준다.
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

    // 외부에서 팝업 UI가 켜질 경우, Manager를 통해 SetCanvas를 요청하여 SetOrder를 통해 순서를 정해준다.
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        // 정렬을 요청할 경우
        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        // 정렬을 요청하지 않을 경우 (팝업이 아닌 일반 UI일 경우)
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

        // 부모를 설정한다.
        if (parent != null)
            go.transform.SetParent(parent);

        // 캔버스 설정
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

        // 부모를 설정한다.
        if (parent != null)
            go.transform.SetParent(parent);

        return Util.GetOrAddComponent<T>(go);
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        // 이름을 만약 받지 않았다면, <T>의 이름을 사용하겠다.
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");

        // 컴포넌트를 빼온다.
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        // popup을 반환한다.
        return sceneUI;
    }

    // 인자값은 프리팹의 이름을 받는다.
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        // 이름을 만약 받지 않았다면, <T>의 이름을 사용하겠다.
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        
        // 컴포넌트를 빼온다.
        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);
        
        // popup을 반환한다.
        return popup;
    }

    // 팝업을 확인한 이후에 닫아준다. (팝업이 여러개일 경우)
    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        // 팝업이 아닐 경우 로그를 남겨준다.
        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        // 먼저 개수를 체크한다.
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        // 팝업을 지워준다.
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;

        _order--;
    }

    // 모든 팝업을 지워준다
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
