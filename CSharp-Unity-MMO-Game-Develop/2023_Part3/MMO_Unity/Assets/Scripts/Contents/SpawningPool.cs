using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    // ���� ����, ������ ����, �������Ѿ� �ϴ� ���� ����
    [SerializeField] int _monsterCount = 0;
    int _reserveCount = 0;
    [SerializeField] int _keepMonsterCount = 0;

    // ���� ��ġ, ���� �ݰ�, ���� �ð�
    [SerializeField] Vector3 _spawnPos;
    [SerializeField] float _spawnRadius = 15.0f;
    [SerializeField] float _spawnTime = 5.0f;

    public void AddMonsterCount(int value) { _monsterCount += value; }
    public void SetKeppMonsterCount(int count) { _keepMonsterCount = count; }

    void Start()
    {
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    void Update()
    {
        // ���� ������ �������Ѿ� �ϴ� ���� �������� �۴ٸ�
        while (_reserveCount+ _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }

    IEnumerator ReserveSpawn()
    {
        _reserveCount++;
        // ������ ���� �ð��� ������.
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));
        // ���� ī��Ʈ�� �̺�Ʈ�� ���� ó���ǹǷ�, ���� ó���� ���� �������� �ʴ´�.
        GameObject obj = Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");
        // �� �� �ִ� �������� üũ�Ѵ�.
        NavMeshAgent nma = obj.GetOrAddComponent<NavMeshAgent>();

        // ������ ��ġ ����
        Vector3 randPos;
        while (true)
        {
            // insideUnitSphere �޼��带 �̿��� �� �ȿ� ������ ��ġ/�������� ����
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, _spawnRadius);
            // ���� ������ �ȵǴϱ� y ��ǥ ����
            randDir.y = 0.0f;
            randPos = _spawnPos + randDir;

            // �� �� �ִ��� Ȯ��
            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(randPos, path))
                break;
        }

        obj.transform.position = randPos;
        _reserveCount--;
    }
}
