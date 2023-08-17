using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    // 이벤트를 통해 키 입력을 전파
    public Action KeyAction = null;

    public void OnUpdate()
    {
        if (Input.anyKey == false)
            return;

        // 액션으로 전파
        if (KeyAction != null)
            KeyAction.Invoke();
    }
}
