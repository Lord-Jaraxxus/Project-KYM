using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class AISensor : MonoBehaviour
    {
        public CharacterBase DetectedTarget => detectedTarget; // 현재 감지된 캐릭터를 외부에서 접근할 수 있도록 프로퍼티로 노출합니다.

        [SerializeField] private LayerMask obstacleLayer; // 장애물 레이어
        [SerializeField] private float sensorRadius = 5f;
        [SerializeField] private float viewDistance = 5f; // 시야 거리
        [SerializeField] private float viewAngle = 120f; // 시야 각도 (나중에 추가할지도?)

        [Header("Sensor Components")]
        [SerializeField] private Rigidbody sensorRigidbody;
        [SerializeField] private SphereCollider sensorCollider;

        public System.Action<CharacterBase> OnDetectedCharacterEvent;
        public System.Action<CharacterBase> OnLostCharacterEvent;

        private CharacterBase detectedTarget; // 현재 감지된 캐릭터

        private void Awake()
        {
            if (TryGetComponent(out sensorRigidbody) == false) 
            {
                sensorRigidbody = gameObject.AddComponent<Rigidbody>();
                sensorRigidbody.isKinematic = true; 
            }

            if (TryGetComponent(out sensorCollider) == false) 
            {
                sensorCollider = gameObject.AddComponent<SphereCollider>();
                sensorCollider.isTrigger = true;
            }
        }

        private void Start()
        {
            SetSensorRadius(sensorRadius);
        }

        private void SetSensorRadius(float radius) 
        {
            sensorCollider.radius = radius;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CharacterBase character))
            {
                Vector3 origin = transform.position;
                origin.y = character.transform.position.y; // 수평선상에서만 레이캐스트 검사
                Physics.Raycast(origin, (character.transform.position - origin).normalized, out RaycastHit hitInfo, viewDistance, obstacleLayer);
                if (hitInfo.collider != null && hitInfo.collider.gameObject != character.gameObject) // 장애물이 있으면 감지하지 않음 
                {
                    Debug.Log("장애물에 막혀 타겟이 감지되지 않았습니다.", this);
                    return; 
                } 

                detectedTarget = character; // 감지된 캐릭터 저장
                OnDetectedCharacterEvent?.Invoke(character);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out CharacterBase character))
            {
                detectedTarget = null; // 감지된 캐릭터 초기화   
                OnLostCharacterEvent?.Invoke(character);
            }
        }
    }
}
