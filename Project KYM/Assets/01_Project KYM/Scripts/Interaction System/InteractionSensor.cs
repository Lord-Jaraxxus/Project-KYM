using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class InteractionSensor : MonoBehaviour
    {
        public float SensorRadius 
        {
            get => sensorCollider.radius;
            set => sensorCollider.radius = value;
        }

        private Rigidbody sensorRigid;
        private SphereCollider sensorCollider;

        public event System.Action<IInteractable> OnDetectedInteractable; // 상호작용 가능한 객체 감지 이벤트
        public event System.Action<IInteractable> OnLostInteractable; // 상호작용 가능한 객체 상실 이벤트

        private void Awake()
        {
            sensorRigid = gameObject.AddComponent<Rigidbody>();
            sensorRigid.isKinematic = true; // 물리 영향 받지 않도록 설정
            sensorCollider = gameObject.AddComponent<SphereCollider>();
            sensorCollider.isTrigger = true; // 트리거로 설정

            SensorRadius = 3.0f; // 기본 센서 반경 설정
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IInteractable>(out var interactable)) 
            {
                OnDetectedInteractable?.Invoke(interactable); // 이벤트 호출
            } 
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IInteractable>(out var interactable))
            {
                OnLostInteractable?.Invoke(interactable); // 이벤트 호출
            }
        }
    }
}
