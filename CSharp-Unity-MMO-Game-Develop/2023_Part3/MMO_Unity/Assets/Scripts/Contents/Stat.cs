using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField] protected int _level;
    [SerializeField] protected int _hp;
    [SerializeField] protected int _maxHp;
    [SerializeField] protected int _attack;
    [SerializeField] protected int _defense;
    [SerializeField] protected float _moveSpeed;

    // 프로퍼티로 만들면 유니티에서 뜨지 않으므로, 툴에서 확인하기 위해 아래와 같이 작업한다.
    public int Level { get { return _level; } set { _level = value; } }
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public int Defense { get { return _defense; } set { _defense = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    private void Start()
    {
        _level = 1;
        _hp = 100;
        _maxHp = 100;
        _attack = 10;
        _defense = 5;
        _moveSpeed = 5.0f;
    }

    public virtual void OnAttacked(Stat attacker)
    {
        // 음수 방지
        int damage = Mathf.Max(0, attacker.Attack - Defense);
        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            OnDead(attacker);
        }
    }

    protected virtual void OnDead(Stat attacker)
    {
        // 경험치를 주기 위한 플레이어 형 변환
        PlayerStat playerStat = attacker as PlayerStat;

        // 널 체크
        if (playerStat != null)
        {
            playerStat.Exp += 100;
        }

        Managers.Game.Despawn(gameObject);
    }
}
