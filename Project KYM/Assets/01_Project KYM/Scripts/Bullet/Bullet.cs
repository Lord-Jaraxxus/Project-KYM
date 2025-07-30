using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KYM
{
    public class Bullet : MonoBehaviour
    {
        public CharacterBase shooter;

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

        public void Initialize(CharacterBase owner)
        {
            shooter = owner;
        }


        private void OnTriggerEnter(Collider other)
        {

            Vector3 hitPoint = transform.position; // 충돌 지점
            Quaternion hitNormal = Quaternion.LookRotation(-transform.forward, transform.up); // 충돌 노멀 방향
            string impactKey = string.Empty; // 임팩트 키

            // Physics Material의 이름을 이용해서, 어디에 부딪혔는지 구분
            if (other.material.name.Contains("Dirt"))
            {
                impactKey = "DirtImpact";
            }
            if (other.material.name.Contains("Wood"))
            {
                impactKey = "WoodImpact";
            }
            if (other.material.name.Contains("Metal"))
            {
                impactKey = "MetalImpact";
            }
            if (other.material.name.Contains("Leaf"))
            {
                impactKey = "LeafImpact";
            }
            if (other.material.name.Contains("Water"))
            {
                impactKey = "WaterImpact";
            }

            EffectManager.Instance.SpawnEffect(impactKey, hitPoint, hitNormal);
        }
    }
}

