using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    // Poolable 컴포넌트를 들고있으면, 풀링을 진행하고
    // 그렇지 않으면 풀링을 진행하지 않는다.

    // 현재 풀링이 된 상태인가?의 정보
    public bool IsUsing;
}
