using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : MonoBehaviour
{
    public Grid _grid;
    public float _speed = 5.0f;
    
    Vector3Int _cellPos = Vector3Int.zero;  // 현재 셀 위치
    MoveDir _dir = MoveDir.None;            // 이동 방향
    bool _isMoving = false;                 // 이동 중인지 여부
    
    void Start()
    {
        // 셀 위치를 월드 위치로 변환
        Vector3 pos = _grid.CellToWorld(_cellPos) + new Vector3(0.5f, -2.5f);
        transform.position = pos;
    }

    void Update()
    {
        GetDirInput();
        UpdatePosition();
        UpdateIsMoving();
    }

    // 키보드 입력을 받아 이동 방향을 결정하는 함수
    void GetDirInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            // 1인 게임 개발에서는 이렇게 이동해도 상관없지만,
            // MMO와 같은 멀티플레이 게임에서는 이렇게 이동해서는 안된다.
            // 연동도 힘들어지고, 다른 플레이어의 움직임을 처리할 수 없기 때문이다.
            // 따라서 이동은 서버에서 처리하고, 클라이언트는 서버에서 받은 정보를 토대로 플레이어를 이동시킨다.
            // transform.position += Vector3.up * Time.deltaTime * _speed;
            
            _dir = MoveDir.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _dir = MoveDir.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _dir = MoveDir.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _dir = MoveDir.Right;
        }
        else
        {
            _dir = MoveDir.None;
        }
    }
    
    // 이동 중인지 여부를 체크하는 함수
    void UpdatePosition()
    {
        if (_isMoving == false)
            return;

        // 서버에서는 이미 이동했지만, 클라이언트 내에서 스르륵 이동을 위한 함수
        Vector3 destPos = _grid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f);
        Vector3 moveDir = destPos - transform.position;
        
        // 도착 여부 체크
        float dist = moveDir.magnitude;
        if (dist < _speed * Time.deltaTime)
        {
            // 도착한 상태
            transform.position = destPos;
            _isMoving = false;
        }
        else
        {
            // 이동 중
            transform.position += moveDir.normalized * _speed * Time.deltaTime;
            _isMoving = true;
        }
    }
    
    // 이동 가능 상태일 때, 실제 이동을 처리하는 함수
    void UpdateIsMoving()
    {
        if (_isMoving == false)
        {
            switch (_dir)
            {
                case MoveDir.Up:
                    _cellPos += Vector3Int.up;
                    // 완전히 이동하는 애니메이션이 끝나기 전까지 또 이동할 수 없다.
                    _isMoving = true;
                    break;
                case MoveDir.Down:
                    _cellPos += Vector3Int.down;
                    _isMoving = true;
                    break;
                case MoveDir.Left:
                    _cellPos += Vector3Int.left;
                    _isMoving = true;
                    break;
                case MoveDir.Right:
                    _cellPos += Vector3Int.right;
                    _isMoving = true;
                    break;
            }
        }
    }
}
