using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM 
{
    public class CameraSystem : MonoBehaviour
    {
        public static CameraSystem Instance { get; private set; }
        [field: SerializeField] public Vector3 AimingPoint { get; private set; }

        private Camera mainCamera;
        private void Awake()
        {
            Instance = this;
            mainCamera = Camera.main; // 메인 카메라를 가져옵니다.
        }

        private void Update()
        {
            Ray screenCenterRay = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
            if (Physics.Raycast(screenCenterRay, out RaycastHit hitInfo, 1000f))
            {
                AimingPoint = hitInfo.point; // 레이캐스트가 충돌한 지점의 위치를 저장합니다.
            }
            else
            {
                AimingPoint = screenCenterRay.GetPoint(1000f); // 레이캐스트가 충돌하지 않은 경우, 카메라에서 1000 단위 떨어진 지점을 사용합니다.
            }
        }
    }
}

