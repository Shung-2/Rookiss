using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField] protected int _exp;
    [SerializeField] protected int _gold;

    public int Exp
    { 
        get { return _exp; }
        set
        {
            _exp = value;
            
            // 레벨업 체크
            int level = 1;
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false)
                    break;

                if (_exp < stat.totalExp)
                    break;

                level++;
            }

            // 레벨업을 해서 현재 레벨과 다른 경우
            if (level != Level)
            {
                Debug.Log("Level Up!");
                
                // 레벨을 변경하고, 스탯을 갱신한다.
                Level = level;
                SetStat(level);
            }
        }
    }

    public int Gold { get { return _gold; } set { _gold = value; } }

    private void Start()
    {
        _level = 1;
        _exp = 0;
        _defense = 5;
        _moveSpeed = 5.0f;
        _gold = 0;

        SetStat(_level);
    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dic = Managers.Data.StatDict;
        Data.Stat stat = dic[level];

        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
    }

    protected override void OnDead(Stat attacker)
    {
        Debug.Log("Player Dead");
    }
}
