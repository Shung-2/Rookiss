using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    // �̺�Ʈ�� ���� Ű �Է��� ����
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressed = false;

    public void OnUpdate()
    {
        // UI ���� ���콺�� �ö� �ִٸ� (UI �������� �÷��̾ �������� �ʵ���)
        if (EventSystem.current.IsPointerOverGameObject()) 
            return;

        // Ű���� �Է��� �׼����� ����
        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        // ���콺 �Է��� �̺�Ʈ ����
        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0)) // ���� ��ư�� ������ ��
            {
                MouseAction.Invoke(Define.MouseEvent.Press); // ���콺�� ������ �ִ� ���ȿ��� Define.MouseEvent.Press�� ���ڷ� �Ѱ� �׼ǿ� ��ϵ� �Լ� ����
                _pressed = true; // ������ ������ ǥ���ϴ� bool ����
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (_pressed) // ������ ���� �ƴѵ�, ���Ⱦ��ٸ� �� ����
                    MouseAction.Invoke(Define.MouseEvent.Click); // ���콺�� ������ �� ���¿��� Define.MouseEvent.Click�� ���ڷ� �Ѱ� �׼ǿ� ��ϵ� �Լ� ����
                
                _pressed = false; // �ʱ�ȭ
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
