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
            Destroy(gameObject, lifeTime); // 생명 주기 후에 총알을 파괴합니다.
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime, Space.Self); // 총알을 앞으로 이동시킵니다.
        }
    }
}

