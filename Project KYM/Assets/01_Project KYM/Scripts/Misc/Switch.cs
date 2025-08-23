using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class Switch : MonoBehaviour, IHittable
    {
        [SerializeField] private GameObject TargetObj; // 스위치가 작동할 대상 오브젝트


        private void OnTriggerEnter(Collider other)
        {
            // 충돌한 오브젝트가 총알인지 확인
            if (other.TryGetComponent<Bullet>(out var bullet))
            {
                // 총알이 스위치에 충돌했을 때 OnHit 메서드 호출
                OnHit(1); // 예시로 데미지를 1로 설정
            }
        }

        public void OnHit(float damage)
        {
            if (TargetObj != null)
            {
                IInteractable interactable = TargetObj.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(); // 대상 오브젝트의 Interact 메서드 호출
                    Debug.LogWarning("스위치가 총알에 맞음");
                }
                else
                {
                    Debug.LogWarning("TargetObj does not implement IInteractable interface.");
                }
            }
            else
            {
                Debug.LogWarning("TargetObj is not assigned in the Switch component.");
            }
        }
    }
}
