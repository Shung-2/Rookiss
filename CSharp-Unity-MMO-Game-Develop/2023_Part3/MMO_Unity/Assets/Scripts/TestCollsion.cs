using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollsion : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision @ {collision.gameObject.name}");
    }

    // 1) 둘 다 Collider가 있어야 한다.
    // 2) 둘 중 하나는 Rigidbody가 있어야 한다.
    // 3) 둘 중 하나는 IsTrigger가 체크되어 있어야 한다. (On)
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger @ {other.gameObject.name} ");
    }

    void Start()
    {
        
    }

    void Update()
    {
        #region 레이캐스트
        //Vector3 look = transform.TransformDirection(Vector3.forward);
        //Debug.DrawRay(transform.position + Vector3.up, transform.forward * 10, Color.red);

        //// 단일 레이캐스트
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position + Vector3.up, look, out hit, 10))
        //{
        //    Debug.Log($"Raycast Hit! {hit.collider.name}");
        //}

        //// 관통 레이캐스트
        //RaycastHit[] hits;
        //hits = Physics.RaycastAll(transform.position + Vector3.up, look, 10);

        //foreach (RaycastHit h in hits)
        //{
        //    Debug.Log($"Raycast Hit! {h.collider.name}");
        //}
        #endregion

        // Local <-> World <-> (Viewport <-> Screen) (화면)

        // Debug.Log(Input.mousePosition); // 스크린 좌표(픽셀)
        // Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition)); // 뷰포트 좌표(화면 비율)
        if (Input.GetMouseButtonDown(0))
        {
            //Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)); // 월드 좌표(3D 좌표)
            //Vector3 dir = mousePos - Camera.main.transform.position;
            //dir = dir.normalized;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Debug.DrawRay(Camera.main.transform.position, dir * 100.0f, Color.red, 1.0f);
            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

            RaycastHit hit;

            //if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 100.0f))
            //{
            //    Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name}");
            //}
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name}");
            }
        }
    }
}
