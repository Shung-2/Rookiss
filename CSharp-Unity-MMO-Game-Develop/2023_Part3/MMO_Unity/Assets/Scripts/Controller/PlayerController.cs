using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]
    float _speed = 10.0f;

    Vector3 _destPos;

    void Start()
    {
        #region MyVector ����
        MyVector to = new MyVector(10.0f, 0.0f, 0.0f);
        MyVector from = new MyVector(5.0f, 0.0f, 0.0f);
        MyVector dir = to - from; // ���� ����

        dir = dir.normalized; // (1.0f, 0.0f, 0.0f)

        MyVector newPos = from + dir * _speed;

        // ���� ����
        // 1. �Ÿ�(ũ��)    // 5
        // 2. ���� ����     // ����
        #endregion

        // Managers.Input.KeyAction -= OnKeyboard; // �ߺ� ��� ����
        // Managers.Input.KeyAction += OnKeyboard; // Ű���� �Է��� ���� �޴´�.

        Managers.Input.MouseAction -= OnMouseClicked; // �ߺ� ��� ����
        Managers.Input.MouseAction += OnMouseClicked; // ���콺 �Է��� ���� �޴´�.
    }

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
    }

    PlayerState _state = PlayerState.Idle;

    void UpdateDie()
    {
        // ����� �ƹ��͵� ���� �ʴ´�.
    }

    void UpdateMoving()
    {
        // ���� ���͸� ���Ѵ�.
        Vector3 dir = _destPos - transform.position;

        // ������ ����
        if (dir.magnitude < 0.0001f)
        {
            _state = PlayerState.Idle;
        }
        else
        {
            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10.0f);
        }

        // �ִϸ��̼� ó��
        Animator anim = GetComponent<Animator>();
        // ���� ���� ���¿� ���� ������ �Ѱ��ش�.
        anim.SetFloat("speed", _speed);
    }
    void UpdateIdle()
    {
        // �ִϸ��̼� ó��
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0);
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

    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Wall")))
        {
            // �������� �����ߴٰ� �̵��Ѵ�.
            _destPos = hit.point;
            _state = PlayerState.Moving;
        }
    }
}