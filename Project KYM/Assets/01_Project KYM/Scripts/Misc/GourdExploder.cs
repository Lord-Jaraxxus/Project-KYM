using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class GourdExploder : MonoBehaviour, IHittable
    {
        [SerializeField] private GameObject cubePrefab;     // 생성할 큐브 프리팹
        [SerializeField] private int spawnCount = 30;       // 생성할 큐브의 개수
        [SerializeField] private float spreadRadius = 1f;   // 큐브가 퍼지는 범위

        [SerializeField] private float explosionForce = 500f;   // 폭발력
        [SerializeField] private float explosionRadius = 1f;    // 폭발 반경..? 퍼지는 범위?
        [SerializeField] private float upwardsModifier = 0.5f;  // 살짝 위로 튕기는 느낌 추가

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Bullet>(out var bullet))
            {
                OnHit(1); // 예시로 데미지를 1로 설정
                Debug.Log("Gourd hit by bullet, exploding!");
            }
            else
            {
                Debug.Log("It isn't a bullet.");
            }
        }

        public void OnHit(int damage)
        {
            Expolode(); // 폭발 메서드 호출
        }

        private void Expolode() 
        {
            gameObject.SetActive(false);

            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 randomPos = transform.position + Random.insideUnitSphere * spreadRadius;
                Quaternion randomRot = Quaternion.Euler( Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f) );
                GameObject Cube = Instantiate(cubePrefab, randomPos, randomRot);

                Rigidbody rb = Cube.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, this.transform.position , explosionRadius, upwardsModifier);
                }

                Destroy(Cube, 5f); // 5초 후에 큐브 삭제
            }
        }
    }
}

