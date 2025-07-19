using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KYM
{
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField] public Transform CinemachineCameraTarget { get; private set; }

        private CharacterBase linkedCharacter;
        private Camera mainCamera;

        private void Awake()
        {
            linkedCharacter = GetComponent<CharacterBase>();
            mainCamera = Camera.main;
        }

        private float cameraThreshold = 0.01f; // 카메라 회전 임계값
        private float cinemachineTargetYaw;
        private float cinemachineTargetPitch;
        private float cameraTopClamp = 85.0f; // 카메라 상단 회전 제한
        private float camereaBottomClamp = -85.0f; // 카메라 하단 회전 제한


        private void Start()
        {
            Vector3 spawnPosition = UserDataModel.Singleton.PlayerInfoDto.LastPosition;
            Quaternion spawnRotation = Quaternion.Euler(UserDataModel.Singleton.PlayerInfoDto.LastRotation);
            transform.SetPositionAndRotation(spawnPosition, spawnRotation); // 플레이어 위치와 회전 설정

            linkedCharacter.Initialize(GameDataModel.Singleton.PlayerStatDto.playerCharacterStatSO); // 캐릭터 스텟 초기화

            var playerHUD = UIManager.Singleton.GetUI<PlayerHUD>(UIList.PlayerHUD); // PlayerHUD 가져옴

            playerHUD.RefreshAmmoText(linkedCharacter.CurAmmo, linkedCharacter.MaxAmmo, linkedCharacter.ReserveAmmo); // 탄약 텍스트 초기화
            playerHUD.RefreshHpUI(linkedCharacter.CurHP, linkedCharacter.MaxHP); // 체력 UI 초기화
            playerHUD.RefreshSpUI(linkedCharacter.CurSP, linkedCharacter.MaxSP); // 스태미너 UI 초기화

            linkedCharacter.OnAmmoChanged += playerHUD.RefreshAmmoText; // 탄약 변경 이벤트 구독
            linkedCharacter.OnHpChanged += playerHUD.RefreshHpUI; // 체력 변경 이벤트 구독
            linkedCharacter.OnSpChanged += playerHUD.RefreshSpUI; // 스태미너 변경 이벤트 구독
        }

        private void OnDestroy()
        {
            Cursor.visible = true;  // 커서 보이기
            Cursor.lockState = CursorLockMode.None; // 커서 잠금 해제

            if (linkedCharacter) 
            {
                var playerHUD = UIManager.Singleton.GetUI<PlayerHUD>(UIList.PlayerHUD); // PlayerHUD 가져옴
                linkedCharacter.OnAmmoChanged -= playerHUD.RefreshAmmoText; // 탄약 변경 이벤트 구독 해제
                linkedCharacter.OnHpChanged -= playerHUD.RefreshHpUI; // 체력 변경 이벤트 구독 해제
                linkedCharacter.OnSpChanged -= playerHUD.RefreshSpUI; // 스태미너 변경 이벤트 구독 해제
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                Cursor.visible = true; // Alt 키를 누르고 있으면 커서 보이기
                Cursor.lockState = CursorLockMode.None; // 커서 잠금 해제
            }
            else
            {
                Cursor.visible = false; // 커서 숨김
                Cursor.lockState = CursorLockMode.Locked; // 커서 잠금 상태 설정
            }

            if (linkedCharacter == null) { return; }

            Vector2 inputMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            bool isAim = Input.GetMouseButton(1); // 마우스 오른쪽 버튼 클릭 시 조준 모드 활성화

            if (Input.GetMouseButton(0)) // 마우스 좌클릭이 눌러져있으면 계속 true
            {
                linkedCharacter.Shoot();
            }

            if (Input.GetKeyDown(KeyCode.R)) // R 키를 눌렀을 때
            {
                linkedCharacter.Reload(); // 재장전 처리
            }

            if (Input.GetKeyDown(KeyCode.LeftControl)) // 왼쪽 Ctrl 키를 눌렀을 때
            {
                linkedCharacter.IsCrouch = !linkedCharacter.IsCrouch; // 크롤링 상태 토글
            }

            linkedCharacter.IsAiming = isAim; // 조준 모드 설정
            linkedCharacter.SetMovementForward(mainCamera.transform.forward); // 카메라의 전방 방향을 설정
            linkedCharacter.Move(inputMove); // 캐릭터 이동 처리
            linkedCharacter.Rotate(CameraSystem.Instance.AimingPoint); // 카메라 시스템에서 조준 지점을 가져와 회전 처리
            linkedCharacter.AimingPoint = CameraSystem.Instance.AimingPoint; // 캐릭터의 조준 지점을 카메라 시스템에서 가져옴

            if(Input.GetKeyDown(KeyCode.LeftShift)) // 왼쪽 Shift 키를 누르고 있으면 달리기
            {
                linkedCharacter.IsWalk = false; // 달리기 상태로 설정
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                linkedCharacter.IsWalk = true; 
            }

            if (Input.GetKeyDown(KeyCode.F9)) // Save
            {
                UserDataModel.Singleton.PlayerInfoDto.SetPositionAndRotation(transform.position, transform.rotation); // 현재 위치와 회전 저장
                UserDataModel.Singleton.PlayerInfoDto.SaveData(); // 데이터 저장
            }
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        void CameraRotation()
        {
            Vector2 inputLook = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            if (inputLook.sqrMagnitude >= cameraThreshold) // 카메라 회전 임계값 체크
            {
                float yaw = inputLook.x;
                float pitch = inputLook.y;

                cinemachineTargetYaw += inputLook.x;
                cinemachineTargetPitch -= inputLook.y;
            }

            cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, camereaBottomClamp, cameraTopClamp); // 카메라 상하 회전 제한

            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch, cinemachineTargetYaw, 0f);
        }

        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) angle += 360f;
            if (angle > 360f) angle -= 360f;

            return Mathf.Clamp(angle, min, max);
        }


    }
}
