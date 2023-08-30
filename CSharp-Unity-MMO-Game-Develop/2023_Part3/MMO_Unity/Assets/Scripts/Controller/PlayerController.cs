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
    PlayerStat _stat;
    Vector3 _destPos;

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
    }

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,
    }

    PlayerState _state = PlayerState.Idle;

    void UpdateDie()
    {
        // ����� �ƹ��͵� ���� �ʴ´�.
    }

    void UpdateMoving()
    {
        // ���Ͱ� �� �����Ÿ����� ������ ����
        if (_lockTarget != null)
        {
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= 1)
            {
                _state = PlayerState.Skill;
                return;
            }
        }

        // ���� ���͸� ���Ѵ�.
        Vector3 dir = _destPos - transform.position;

        // ������ ����
        if (dir.magnitude < 0.1f)
        {
            _state = PlayerState.Idle;
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
                    _state = PlayerState.Idle;
                return;
            }

            // transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10.0f);
        }

        // �ִϸ��̼� ó��
        Animator anim = GetComponent<Animator>();
        // ���� ���� ���¿� ���� ������ �Ѱ��ش�.
        anim.SetFloat("speed", _stat.MoveSpeed);
    }
    void UpdateIdle()
    {
        // �ִϸ��̼� ó��
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0);
    }

    void UpdateSkile()
    {
        Debug.Log("UpdateSkile");
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

        switch (_state)
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

    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    GameObject _lockTarget;
    void OnMouseEvent(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die)
            return;

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
                        _state = PlayerState.Moving;

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
                    if (_lockTarget != null)
                        _destPos = _lockTarget.transform.position;
                    else if (raycastHit)
                        _destPos = hit.point;
                }
                break;
        }
    }
}