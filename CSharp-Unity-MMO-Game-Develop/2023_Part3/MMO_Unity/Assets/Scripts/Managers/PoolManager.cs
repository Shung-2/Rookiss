using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region Pool
    // ���� Ǯ�� ������ �ִ�.
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
            // poolable ���̶�� ������ �ִ� ��Ȳ!
            if (poolable == null)
                return;

            // �θ���� �����Ѵ�.
            poolable.transform.parent = Root;
            // ������Ʈ�� ���ش�.
            poolable.gameObject.SetActive(false);
            // Ǯ�� ���°� �ƴ��� ǥ���Ѵ�
            poolable.IsUsing = false;

            // ���ÿ� �־��ش�
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

            // ������Ʈ�� Ȱ��ȭ�Ѵ�.
            poolable.gameObject.SetActive(true);

            // DontDestroyOnLoad�� ������
            if (parent == null)
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;

            // �θ� �����Ѵ�.
            poolable.transform.parent = parent;
            // Ǯ�� �������� ǥ���Ѵ�.
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion

    // Ǯ ��ϵ��� Dictionary�� �̿��Ͽ� String ������ Ű�� �����Ѵ�.
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
        // Ǯ�� ������ش�.
        Pool pool = new Pool();
        pool.Init(original, count);
        // Ǯ�� �θ� �����Ѵ�.
        pool.Root.parent = _root;

        // Ǯ ���(Dictionary)�� �߰��Ѵ�.
        _pool.Add(original.name, pool);
    }

    // ��ȯ�� ���̴� Pool�� �־����!
    public void Push(Poolable poolable)
    {
        // �̸��� �ҷ��´�.
        string name = poolable.gameObject.name;

        // ���� ���� ��Ȳ�ϰ��
        if (_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    // ���� ��ü�� ���� ������ �ް�, �θ� ������ null�� �ʱ�ȭ�Ѵ�.
    public Poolable Pop(GameObject original, Transform parent = null)
    {
        // Ǯ�� �ִ��� Ȯ���Ѵ�.
        if (_pool.ContainsKey(original.name) == false)
        {
            // Ǯ�� ������ش�.
            CreatePool(original);
        }

        return _pool[original.name].Pop(parent);
    }

    // ���� ���� ���� ã�´�.
    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false)
            return null;

        return _pool[name].Original;
    }

    public void Clear()
    {
        // foreach�� �̿��Ͽ� �ڽ��� �����Ѵ�.
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        // Ǯ ����� ����.
        _pool.Clear();
    }
}