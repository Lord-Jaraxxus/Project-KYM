using UnityEngine;

namespace KYM
{
    public class DoorController : MonoBehaviour, IInteractable
    {
        public Transform doorHinge;
        public float openAngle = -90f;
        public float openSpeed = 2f;

        private bool isOpen = false;
        private bool isMoving = false;
        private float currentLerpTime = 0f;

        private Quaternion closedRotation;
        private Quaternion openedRotation;

        public string Key => throw new System.NotImplementedException();
        public Sprite InteractionIcon => throw new System.NotImplementedException();
        public string InteractionMessage => throw new System.NotImplementedException();
        public bool IsOnceInteractable => false; // 여러 번 상호작용 가능

        private void Awake()
        {
            // 시작 시 회전 기준값 설정
            closedRotation = doorHinge.rotation;
            openedRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
        }

        private void Update()
        {
            if (!isMoving) return;

            currentLerpTime += Time.deltaTime * openSpeed;
            float t = Mathf.Clamp01(currentLerpTime);

            doorHinge.rotation = Quaternion.Lerp(
                isOpen ? closedRotation : openedRotation,
                isOpen ? openedRotation : closedRotation,
                t
            );

            if (t >= 1f)
            {
                isMoving = false;
            }
        }

        public void Interact()
        {
            if (isMoving) return; // 이동 중엔 무시
            isOpen = !isOpen;
            currentLerpTime = 0f;
            isMoving = true;
        }
    }
}
