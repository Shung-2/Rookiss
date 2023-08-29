using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        // �� Ÿ�� ����
        SceneType = Define.Scene.Login;
    }

    private void Update()
    {
        // Q�� ������ ���� ������ �̵��Ѵ�.
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Managers.Scene.LoadScene(Define.Scene.Game);
        }
    }

    public override void Clear()
    {
        Debug.Log("LoginScene Clear!");
    }
}
