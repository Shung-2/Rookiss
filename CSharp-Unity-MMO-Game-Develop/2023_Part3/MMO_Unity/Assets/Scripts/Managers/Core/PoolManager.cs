using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region Pool
    // 여러 풀을 가지고 있다.
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; private set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
                Push(Create());
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(Poolable poolable)
        {
            // poolable 널이라면 문제가 있는 상황!
            if (poolable == null)
                return;

            // 부모님을 연결한다.
            poolable.transform.parent = Root;
            // 오브젝트를 꺼준다.
            poolable.gameObject.SetActive(false);
            // 풀링 상태가 아님을 표시한다
            poolable.IsUsing = false;

            // 스택에 넣어준다
            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if (_poolStack.Count > 0)
            {
                poolable = _poolStack.Pop();
            }
            else
            {
                poolable = Create();
            }

            // 오브젝트를 활성화한다.
            poolable.gameObject.SetActive(true);

            // DontDestroyOnLoad를 해제용
            if (parent == null)
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;

            // 부모를 설정한다.
            poolable.transform.parent = parent;
            // 풀링 상태임을 표시한다.
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion

    // 풀 목록들을 Dictionary를 이용하여 String 형태의 키로 관리한다.
    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;

    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        // 풀을 만들어준다.
        Pool pool = new Pool();
        pool.Init(original, count);
        // 풀의 부모를 설정한다.
        pool.Root.parent = _root;

        // 풀 목록(Dictionary)에 추가한다.
        _pool.Add(original.name, pool);
    }

    // 반환할 것이니 Pool에 넣어놔라!
    public void Push(Poolable poolable)
    {
        // 이름을 불러온다.
        string name = poolable.gameObject.name;

        // 팝을 안한 상황일경우
        if (_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    // 원본 객체를 인자 값으로 받고, 부모가 없으면 null로 초기화한다.
    public Poolable Pop(GameObject original, Transform parent = null)
    {
        // 풀이 있는지 확인한다.
        if (_pool.ContainsKey(original.name) == false)
        {
            // 풀을 만들어준다.
            CreatePool(original);
        }

        return _pool[original.name].Pop(parent);
    }

    // 원본 인자 값을 찾는다.
    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false)
            return null;

        return _pool[name].Original;
    }

    public void Clear()
    {
        // foreach를 이용하여 자식을 삭제한다.
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        // 풀 목록을 비운다.
        _pool.Clear();
    }
}