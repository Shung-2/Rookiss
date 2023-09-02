using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    // 나중에 서버랑 연동을 한다고 가정한다면
    // int와 GameObject를 연동한다

    GameObject _player;
    // 딕셔너리와 같지만, 키가 없다.
    HashSet<GameObject> _monster = new HashSet<GameObject>();
    // 이벤트를 통한 SpawningPool 몬스터 개수 관리
    public Action<int> OnSpawnEvent;

    public GameObject GetPlayer() { return _player; }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Monster:
                _monster.Add(go);
                if (OnSpawnEvent != null)
                    OnSpawnEvent.Invoke(1);
                break;

            case Define.WorldObject.Player:
                _player = go;
                break;
        }

        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
        {
            return Define.WorldObject.Unknown;
        }

        return bc.WorldObjectType;
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go);

        switch (type)
        {
            case Define.WorldObject.Monster:
                if (_monster.Contains(go))
                {
                    _monster.Remove(go);
                    if (OnSpawnEvent != null)
                        OnSpawnEvent.Invoke(-1);
                }
                break;

            case Define.WorldObject.Player:
                if (_player == go)
                    _player = null;
                break;
        }

        Managers.Resource.Destroy(go);
    }
}
