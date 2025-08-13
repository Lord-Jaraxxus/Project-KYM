using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class AISensor : MonoBehaviour
    {
        public CharacterBase DetectedTarget => detectedTarget; // 현재 감지된 캐릭터를 외부에서 접근할 수 있도록 프로퍼티로 노출합니다.

        [SerializeField] private float sensorRadius = 5f;

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
