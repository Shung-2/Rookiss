using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    // 이벤트를 통해 키 입력을 전파
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressed = false;
    float _pressedTime = 0;

    public void OnUpdate()
    {
        // UI 위에 마우스가 올라가 있다면 (UI 위에서는 플레이어가 움직이지 않도록)
        if (EventSystem.current.IsPointerOverGameObject()) 
            return;

        // 키보드 입력을 액션으로 전파
        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        // 마우스 입력을 이벤트 전파
        if (MouseAction != null)
        {
            // 왼쪽 버튼을 눌렀을 때
            if (Input.GetMouseButton(0)) 
            {
                if (!_pressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }

                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else
            {
                // 누르는 중이 아니고, 눌렸었다면 뗀 상태
                if (_pressed) 
                {
                    if (Time.time < _pressedTime + 0.2f)
                        MouseAction.Invoke(Define.MouseEvent.Click);

                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }
                
                // 초기화
                _pressed = false;
                _pressedTime = 0;
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
