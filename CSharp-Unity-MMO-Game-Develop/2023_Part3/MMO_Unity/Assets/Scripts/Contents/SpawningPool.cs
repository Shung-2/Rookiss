using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    // 현재 몬스터, 예약한 몬스터, 유지시켜야 하는 몬스터 개수
    [SerializeField] int _monsterCount = 0;
    int _reserveCount = 0;
    [SerializeField] int _keepMonsterCount = 0;

    // 스폰 위치, 스폰 반경, 스폰 시간
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
        // 몬스터 개수가 유지시켜야 하는 몬스터 개수보다 작다면
        while (_reserveCount+ _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }

    IEnumerator ReserveSpawn()
    {
        _reserveCount++;
        // 랜덤한 스폰 시간을 가진다.
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));
        // 몬스터 카운트는 이벤트를 통해 처리되므로, 개수 처리는 따로 진행하지 않는다.
        GameObject obj = Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");
        // 갈 수 있는 영역인지 체크한다.
        NavMeshAgent nma = obj.GetOrAddComponent<NavMeshAgent>();

        // 랜덤한 위치 생성
        Vector3 randPos;
        while (true)
        {
            // insideUnitSphere 메서드를 이용해 구 안에 랜덤한 위치/방향으로 생성
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, _spawnRadius);
            // 땅에 박히면 안되니까 y 좌표 설정
            randDir.y = 0.0f;
            randPos = _spawnPos + randDir;

            // 갈 수 있는지 확인
            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(randPos, path))
                break;
        }

        obj.transform.position = randPos;
        _reserveCount--;
    }
}
