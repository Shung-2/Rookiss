using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    // 이벤트를 통해 키 입력을 전파
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressed = false;

    public void OnUpdate()
    {
        // 키보드 입력을 액션으로 전파
        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        // 마우스 입력을 이벤트 전파
        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0)) // 왼쪽 버튼을 눌렀을 때
            {
                MouseAction.Invoke(Define.MouseEvent.Press); // 마우스를 누르고 있는 동안에는 Define.MouseEvent.Press를 인자로 넘겨 액션에 등록된 함수 실행
                _pressed = true; // 누르는 중임을 표시하는 bool 변수
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (_pressed) // 누르는 중이 아닌데, 눌렸었다면 뗀 상태
                    MouseAction.Invoke(Define.MouseEvent.Click); // 마우스를 누르고 뗀 상태에는 Define.MouseEvent.Click을 인자로 넘겨 액션에 등록된 함수 실행
                
                _pressed = false; // 초기화
            }
        }
    }
}
