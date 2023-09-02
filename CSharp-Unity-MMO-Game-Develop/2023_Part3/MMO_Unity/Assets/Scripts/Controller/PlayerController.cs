using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#region MyVector
// 1. 위치 벡터(좌표 > x, y, z)
// 2. 방향 벡터
struct MyVector
{
    public float x;
    public float y;
    public float z;

    public float magnitude
    {
        get
        {
            return Mathf.Sqrt(x * x + y * y + z * z); // 피타고라스를 이용한다.
        }
    }

    public MyVector normalized
    {
        get
        {
            float len = magnitude;
            return new MyVector(x / len, y / len, z / len);
            // 왜 길이가 1짜리인 벡터가 필요할까?
            // 단위 벡터(forward, back, left, right)와 같이 방향을 표현하기 위해서!
        }
    }

    public MyVector(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static MyVector operator +(MyVector a, MyVector b)
    {
        return new MyVector(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static MyVector operator -(MyVector a, MyVector b)
    {
        return new MyVector(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static MyVector operator *(MyVector a, float b)
    {
        return new MyVector(a.x * b, a.y * b, a.z * b);
    }
}
#endregion

public class PlayerController : BaseController
{
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    PlayerStat _stat;
    bool _stopSkill = false;

    public override void Init()
    {
        #region MyVector 예제
        // MyVector to = new MyVector(10.0f, 0.0f, 0.0f);
        // MyVector from = new MyVector(5.0f, 0.0f, 0.0f);
        // MyVector dir = to - from; // 방향 벡터

        // dir = dir.normalized; // (1.0f, 0.0f, 0.0f)

        // MyVector newPos = from + dir * _stat.MoveSpeed;

        // 방향 벡터
        // 1. 거리(크기)    // 5
        // 2. 실제 방향     // 우측
        #endregion

        WorldObjectType = Define.WorldObject.Player;
        _stat = gameObject.GetComponent<PlayerStat>();

        // Managers.Input.KeyAction -= OnKeyboard; // 중복 등록 방지
        // Managers.Input.KeyAction += OnKeyboard; // 키보드 입력을 구독 받는다.
        Managers.Input.MouseAction -= OnMouseEvent; // 중복 등록 방지
        Managers.Input.MouseAction += OnMouseEvent; // 마우스 입력을 구독 받는다.

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateMoving()
    {
        // 몬스터가 내 사정거리보다 가까우면 공격
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= 1)
            {
                State = Define.State.Skill;
                return;
            }
        }

        // 방향 벡터를 구한다.
        Vector3 dir = _destPos - transform.position;
        // y축은 무시한다.
        dir.y = 0; 

        // 도착한 상태
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            // 레이캐스트를 이용해 Block 레이어를 만난다면 이동을 멈춘다.
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0) == false)
                    State = Define.State.Idle;
                return;
            }

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;

            // transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10.0f);
        }
    }

    protected override void UpdateSkile()
    {
        if (_lockTarget != null)
        {
            // 방향 벡터 생성
            Vector3 dir = _lockTarget.transform.position - transform.position;
            // 회전 제어
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    void OnHitEvent()
    {
        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
        }

        if (_stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Skill;
        }
    }

    //void Update()
    //{
        #region 로컬 > 월드, 월드 > 로컬 전환시 사용하는 함수 목록
        // Local > World
        // TransforDirection()
        // World > Local
        // InverseTransformDirection()
        #endregion
        #region 회전 예제
        // 절대 회전값
        //transform.eulerAngles += new Vector3(0.0f, _rotY, 0.0f);

        // 상대 회전값(+- delta)
        // transform.Rotate(new Vector3(0.0f, _rotY, 0.0f));
        // transform.rotation = Quaternion.Euler(0.0f, _rotY, 0.0f);
        #endregion
    //}

    #region 키보드 움직임
    //void OnKeyboard()
    //{
    //    if (Input.GetKey(KeyCode.W))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), Time.deltaTime * 5.0f);
    //        transform.position += Vector3.forward * Time.deltaTime * _speed;
    //    }
    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), Time.deltaTime * 5.0f);
    //        transform.position += Vector3.back * Time.deltaTime * _speed;
    //    }
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), Time.deltaTime * 5.0f);
    //        transform.position += Vector3.left * Time.deltaTime * _speed;
    //    }
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), Time.deltaTime * 5.0f);
    //        transform.position += Vector3.right * Time.deltaTime * _speed;
    //    }

    //    _moveToDest = false;
    //}
    #endregion

    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case Define.State.Die:
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
                {
                    if (evt == Define.MouseEvent.PointerUp)
                    {
                        _stopSkill = true;
                    }
                }
                break;
            default:
                break;
        }
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);
        // Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        // 목적지를 저장했다가 이동한다.
                        _destPos = hit.point;
                        State = Define.State.Moving;
                        _stopSkill = false;

                        // 땅인지, 몬스터인지 충돌 정보를 파악한다.
                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            _lockTarget = hit.collider.gameObject;
                        else
                            _lockTarget = null;
                    }
                }
                break;

            case Define.MouseEvent.Press:
                {
                    if (_lockTarget == null && raycastHit)
                        _destPos = hit.point;
                }
                break;

            case Define.MouseEvent.PointerUp:
                {
                    _stopSkill = true;
                }
                break;
        }
    }
}