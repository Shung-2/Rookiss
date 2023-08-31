using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#region MyVector
// 1. ��ġ ����(��ǥ > x, y, z)
// 2. ���� ����
struct MyVector
{
    public float x;
    public float y;
    public float z;

    public float magnitude
    {
        get
        {
            return Mathf.Sqrt(x * x + y * y + z * z); // ��Ÿ��󽺸� �̿��Ѵ�.
        }
    }

    public MyVector normalized
    {
        get
        {
            float len = magnitude;
            return new MyVector(x / len, y / len, z / len);
            // �� ���̰� 1¥���� ���Ͱ� �ʿ��ұ�?
            // ���� ����(forward, back, left, right)�� ���� ������ ǥ���ϱ� ���ؼ�!
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

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,
    }

    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    PlayerStat _stat;
    Vector3 _destPos;

    [SerializeField] PlayerState _state = PlayerState.Idle;

    GameObject _lockTarget;

    public PlayerState State
    {
        get { return _state; }
        set
        {
            _state = value;

            // �ִϸ��̼� ó��
            Animator anim = GetComponent<Animator>();

            switch (_state)
            {
                case PlayerState.Die:
                    break;
                case PlayerState.Moving:
                    anim.CrossFade("RUN", 0.1f);
                    break;
                case PlayerState.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case PlayerState.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0);
                    break;
                default:
                    break;
            }
        }
    }

    void Start()
    {
        #region MyVector ����
        // MyVector to = new MyVector(10.0f, 0.0f, 0.0f);
        // MyVector from = new MyVector(5.0f, 0.0f, 0.0f);
        // MyVector dir = to - from; // ���� ����

        // dir = dir.normalized; // (1.0f, 0.0f, 0.0f)

        // MyVector newPos = from + dir * _stat.MoveSpeed;

        // ���� ����
        // 1. �Ÿ�(ũ��)    // 5
        // 2. ���� ����     // ����
        #endregion

        _stat = gameObject.GetComponent<PlayerStat>();

        // Managers.Input.KeyAction -= OnKeyboard; // �ߺ� ��� ����
        // Managers.Input.KeyAction += OnKeyboard; // Ű���� �Է��� ���� �޴´�.
        Managers.Input.MouseAction -= OnMouseEvent; // �ߺ� ��� ����
        Managers.Input.MouseAction += OnMouseEvent; // ���콺 �Է��� ���� �޴´�.

        Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    void UpdateDie()
    {
        // ����� �ƹ��͵� ���� �ʴ´�.
    }

    void UpdateMoving()
    {
        // ���Ͱ� �� �����Ÿ����� ������ ����
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= 1)
            {
                State = PlayerState.Skill;
                return;
            }
        }

        // ���� ���͸� ���Ѵ�.
        Vector3 dir = _destPos - transform.position;

        // ������ ����
        if (dir.magnitude < 0.1f)
        {
            State = PlayerState.Idle;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            nma.Move(dir.normalized * moveDist);

            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            // ����ĳ��Ʈ�� �̿��� Block ���̾ �����ٸ� �̵��� �����.
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0) == false)
                    State = PlayerState.Idle;
                return;
            }

            // transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10.0f);
        }
    }

    void UpdateIdle()
    {

    }

    void UpdateSkile()
    {
        if (_lockTarget != null)
        {
            // ���� ���� ����
            Vector3 dir = _lockTarget.transform.position - transform.position;
            // ȸ�� ����
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    void OnHitEvent()
    {
        Debug.Log("OnHitEvent");

        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            PlayerStat myStat = gameObject.GetComponent<PlayerStat>();

            // ���� ����
            int damage = Mathf.Max(0, myStat.Attack - targetStat.Defense);
            Debug.Log(damage);

            targetStat.Hp -= damage;
        }

        if (_stopSkill)
        {
            State = PlayerState.Idle;
        }
        else
        {
            State = PlayerState.Skill;
        }
    }

    void Update()
    {
        #region ���� > ����, ���� > ���� ��ȯ�� ����ϴ� �Լ� ���
        // Local > World
        // TransforDirection()
        // World > Local
        // InverseTransformDirection()
        #endregion

        #region ȸ�� ����
        // ���� ȸ����
        //transform.eulerAngles += new Vector3(0.0f, _rotY, 0.0f);

        // ��� ȸ����(+- delta)
        // transform.Rotate(new Vector3(0.0f, _rotY, 0.0f));
        // transform.rotation = Quaternion.Euler(0.0f, _rotY, 0.0f);
        #endregion

        switch (State)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Skill:
                UpdateSkile();
                break;
        }
    }

    #region Ű���� ������
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

    bool _stopSkill = false;
    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case PlayerState.Die:
                break;
            case PlayerState.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Skill:
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
                        // �������� �����ߴٰ� �̵��Ѵ�.
                        _destPos = hit.point;
                        State = PlayerState.Moving;
                        _stopSkill = false;

                        // ������, �������� �浹 ������ �ľ��Ѵ�.
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