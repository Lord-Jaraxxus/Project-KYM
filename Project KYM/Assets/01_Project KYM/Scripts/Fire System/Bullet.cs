using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f; // �Ѿ� �ӵ�

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime; // �Ѿ��� ������ �̵���ŵ�ϴ�.
    }

    private void OnCollisionEnter(Collision collision)
    {
        IHittable hittable = collision.gameObject.GetComponent<IHittable>();
        if (hittable != null)
        {
            hittable.OnHit(1); // IHittable �������̽��� ������ ��ü�� �浹 ó�� ��û
        }
    }
}
