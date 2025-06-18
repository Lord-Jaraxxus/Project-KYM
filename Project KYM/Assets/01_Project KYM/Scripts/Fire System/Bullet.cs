using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f; // 총알 속도

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime; // 총알을 앞으로 이동시킵니다.
    }

    private void OnCollisionEnter(Collision collision)
    {
        IHittable hittable = collision.gameObject.GetComponent<IHittable>();
        if (hittable != null)
        {
            hittable.OnHit(1); // IHittable 인터페이스를 구현한 객체에 충돌 처리 요청
        }
    }
}
