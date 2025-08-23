using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KYM
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private LayerMask hitMask; // 충돌 체크를 위한 레이어 마스크 설정
        public CharacterBase shooter;

        public float bulletSpeed = 10f;
        public float lifeTime = 5f;

        private void Start()
        {
            Destroy(gameObject, lifeTime); // 생명 주기 후에 총알을 파괴합니다.
        }

        private void Update()
        {
            float moveDistance = bulletSpeed * Time.deltaTime;

            // Raycast로 충돌 체크
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, moveDistance, hitMask))
            {
                HandleHit(hit.collider);
                Destroy(gameObject); // 충돌하면 총알 제거
                return;
            }

            transform.Translate(Vector3.forward * moveDistance, Space.Self); // 총알을 앞으로 이동시킵니다.
        }

        public void Initialize(CharacterBase owner)
        {
            shooter = owner;
        }

        private void HandleHit(Collider collider)
        {
            // 충돌한 오브젝트에 대한 처리 로직
            // 예: 총알이 벽에 부딪혔을 때의 효과를 생성하거나, 총알을 파괴하는 등의 작업

            IHittable hittable = collider.GetComponent<IHittable>();
            if (hittable != null)
            {
                OnHitCharacter(hittable); // 캐릭터에 충돌했을 때의 처리
            }
            else
            {
                OnHitEnvironment(collider); // 환경에 충돌했을 때의 처리
            }

            Debug.Log($"Bullet hit: {collider.name}"); // 디버그용 로그 출력
        }

        private void OnHitEnvironment(Collider other)
        {
            // 환경에 충돌했을 때의 처리 로직
            // 예: 총알이 벽에 부딪혔을 때의 효과를 생성하거나, 총알을 파괴하는 등의 작업

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

            Destroy(gameObject); // 총알을 파괴합니다.
        }

        private void OnHitCharacter(IHittable hittable)
        {
            // 캐릭터에 충돌했을 때의 처리 로직
            // 예: 캐릭터에게 피해를 입히고, 총알을 파괴하는 등의 작업

            Vector3 hitPoint = transform.position; // 충돌 지점
            Quaternion hitNormal = Quaternion.LookRotation(-transform.forward, transform.up); // 충돌 노멀 방향

            hittable.OnHit(GameDataModel.Singleton.WeaponDataDto.weaponDataSO.Damage); // IHittable 인터페이스를 통해 피해를 입힙니다.
            EffectManager.Instance.SpawnEffect("WaterImpact", hitPoint, hitNormal); // 총알이 적중했을 때의 효과 생성, 일단 임시로 물!

            Destroy(gameObject); // 총알을 파괴합니다.
        }
    }
}

