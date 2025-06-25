using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class Bullet : MonoBehaviour
    {
        public float bulletSpeed = 10f;
        public float lifeTime = 5f;

        private void Start()
        {
            Destroy(gameObject, lifeTime); // ���� �ֱ� �Ŀ� �Ѿ��� �ı��մϴ�.
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime, Space.Self); // �Ѿ��� ������ �̵���ŵ�ϴ�.
        }
    }
}

