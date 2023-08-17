using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollsion : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision @ {collision.gameObject.name}");
    }

    // 1) �� �� Collider�� �־�� �Ѵ�.
    // 2) �� �� �ϳ��� Rigidbody�� �־�� �Ѵ�.
    // 3) �� �� �ϳ��� IsTrigger�� üũ�Ǿ� �־�� �Ѵ�. (On)
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger @ {other.gameObject.name} ");
    }

    void Start()
    {
        
    }

    void Update()
    {
        #region ����ĳ��Ʈ
        //Vector3 look = transform.TransformDirection(Vector3.forward);
        //Debug.DrawRay(transform.position + Vector3.up, transform.forward * 10, Color.red);

        //// ���� ����ĳ��Ʈ
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position + Vector3.up, look, out hit, 10))
        //{
        //    Debug.Log($"Raycast Hit! {hit.collider.name}");
        //}

        //// ���� ����ĳ��Ʈ
        //RaycastHit[] hits;
        //hits = Physics.RaycastAll(transform.position + Vector3.up, look, 10);

        //foreach (RaycastHit h in hits)
        //{
        //    Debug.Log($"Raycast Hit! {h.collider.name}");
        //}
        #endregion

        // Local <-> World <-> (Viewport <-> Screen) (ȭ��)

        // Debug.Log(Input.mousePosition); // ��ũ�� ��ǥ(�ȼ�)
        // Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition)); // ����Ʈ ��ǥ(ȭ�� ����)
        if (Input.GetMouseButtonDown(0))
        {
            //Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)); // ���� ��ǥ(3D ��ǥ)
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
