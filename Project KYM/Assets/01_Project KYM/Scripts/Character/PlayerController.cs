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
            SoundManager.PlayBGM("BGM_Ingame"); // 메인 테마 음악 재생

            Vector3 spawnPosition = UserDataModel.Singleton.PlayerInfoDto.LastPosition;
            Quaternion spawnRotation = Quaternion.Euler(UserDataModel.Singleton.PlayerInfoDto.LastRotation);
            transform.SetPositionAndRotation(spawnPosition, spawnRotation); // 플레이어 위치와 회전 설정

            linkedCharacter.Initialize(GameDataModel.Singleton.PlayerStatDto.playerCharacterStatSO, true); // 캐릭터 스텟 초기화
            linkedCharacter.InitWeapon(GameDataModel.Singleton.WeaponDataDto.weaponDataSO); // 무기 초기화 (나중에 다른 곳으로 갈 수도?)

            var crosshairUI = UIManager.Singleton.GetUI<CrosshairUI>(UIList.CrosshairUI); // 크로스헤어 UI 가져오기
            UIManager.Show<CrosshairUI>(UIList.CrosshairUI); // 크로스헤어 UI 표시, 부트스트랩에서 안 키더라도 무기가 초기화되면 같이 생기도록
            if (crosshairUI == null) // 크로스헤어 UI가 설정되지 않은 경우
                crosshairUI = FindObjectOfType<CrosshairUI>(); // 현재 씬에서 CrosshairUI 찾기..? 이거맞나?
            crosshairUI.Init(linkedCharacter.CurrentWeapon); // 크로스헤어 UI 초기화 (발사 이벤트 연결함)

            var playerHUD = UIManager.Singleton.GetUI<PlayerHUD>(UIList.PlayerHUD); // PlayerHUD 가져옴
            playerHUD.RefreshAmmoText(linkedCharacter.CurrentWeapon.CurAmmo, linkedCharacter.CurrentWeapon.MaxAmmo, linkedCharacter.CurrentWeapon.ReserveAmmo); // 탄약 텍스트 초기화
            playerHUD.RefreshHpUI(linkedCharacter.CurHP, linkedCharacter.MaxHP); // 체력 UI 초기화
            playerHUD.RefreshSpUI(linkedCharacter.CurSP, linkedCharacter.MaxSP); // 스태미너 UI 초기화

            linkedCharacter.OnAmmoChanged += playerHUD.RefreshAmmoText; // 탄약 변경 이벤트 구독
            linkedCharacter.OnHpChanged += playerHUD.RefreshHpUI; // 체력 변경 이벤트 구독
            linkedCharacter.OnSpChanged += playerHUD.RefreshSpUI; // 스태미너 변경 이벤트 구독
            linkedCharacter.OnCharacterDead += OnCharacterDeadEvent; // 캐릭터 사망 이벤트 구독

            InputManager.Singleton.OnInputLmc += OnReceiveInputLmc;
            InputManager.Singleton.OnInputRmcUp += OnReceiveInputRmcUp;
            InputManager.Singleton.OnInputRmcDown += OnReceiveInputRmcDown;
            InputManager.Singleton.OnInputReload += OnReceiveInputReload;
            InputManager.Singleton.OnInputCrouch += OnreceiveInputCrouch;
            InputManager.Singleton.OnInputSprintUp += OnReceiveInputSprintUp;
            InputManager.Singleton.OnInputSprintDown += OnReceiveInputSprintDown;
            InputManager.Singleton.OnInputSave += OnReceieveInputSave;
            InputManager.Singleton.OnInputInteract += OnReceiveInputInteract;
        }

        private void OnDestroy()
        {
            if (linkedCharacter)
            {
                var playerHUD = UIManager.Singleton.GetUI<PlayerHUD>(UIList.PlayerHUD); // PlayerHUD 가져옴
                linkedCharacter.OnAmmoChanged -= playerHUD.RefreshAmmoText; // 탄약 변경 이벤트 구독 해제
                linkedCharacter.OnHpChanged -= playerHUD.RefreshHpUI; // 체력 변경 이벤트 구독 해제
                linkedCharacter.OnSpChanged -= playerHUD.RefreshSpUI; // 스태미너 변경 이벤트 구독 해제
            }

            InputManager.Singleton.OnInputLmc -= OnReceiveInputLmc;
            InputManager.Singleton.OnInputRmcUp -= OnReceiveInputRmcUp;
            InputManager.Singleton.OnInputRmcDown -= OnReceiveInputRmcDown;
            InputManager.Singleton.OnInputReload -= OnReceiveInputReload;
            InputManager.Singleton.OnInputCrouch -= OnreceiveInputCrouch;
            InputManager.Singleton.OnInputSprintUp -= OnReceiveInputSprintUp;
            InputManager.Singleton.OnInputSprintDown -= OnReceiveInputSprintDown;
            InputManager.Singleton.OnInputSave -= OnReceieveInputSave;
            InputManager.Singleton.OnInputInteract -= OnReceiveInputInteract;
        }


        void OnReceiveInputLmc() => linkedCharacter.Shoot(); // 좌클릭 입력 처리
        void OnReceiveInputRmcUp() => linkedCharacter.IsAiming = false; // 우클릭 입력 해제 (조준 모드 비활성화)
        void OnReceiveInputRmcDown() => linkedCharacter.IsAiming = true; // 우클릭 입력 처리 (조준 모드 활성화)
        void OnReceiveInputReload() => linkedCharacter.Reload(); // 리로드 입력 처리
        void OnreceiveInputCrouch() => linkedCharacter.IsCrouch = !linkedCharacter.IsCrouch; // 앉기 입력 처리 (크롤링 상태 토글)
        void OnReceiveInputSprintDown() => linkedCharacter.IsWalk = false; // 달리기 입력 (왼쪽 Shift 키 누름)
        void OnReceiveInputSprintUp() => linkedCharacter.IsWalk = true; // 달리기 입력 해제 (왼쪽 Shift 키 뗌)
        void OnReceieveInputSave() => Save(); // 저장 입력 처리
        void OnReceiveInputInteract() // 상호작용 입력 처리
        {
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward); // 카메라 위치에서 전방으로 레이 생성
            LayerMask layerMask = LayerMask.GetMask("Default"); // 레이어 마스크 설정 (예: Default 레이어만 감지)
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 3f, layerMask)) // 레이캐스트로 3미터 이내의 오브젝트 감지, 레이어는 default
            {
                var interactable = hitInfo.collider.GetComponent<IInteractable>(); // 감지된 오브젝트에서 IInteractable 인터페이스 가져오기
                interactable?.Interact(); // 인터페이스가 존재하면 상호작용 메서드 호출

                Debug.Log($"Interacted with: {hitInfo.collider.name}"); // 디버그 로그 출력
            }
        }

        public void Save()
        {
            UserDataModel.Singleton.PlayerInfoDto.SetPositionAndRotation(transform.position, transform.rotation); // 현재 위치와 회전 저장
            UserDataModel.Singleton.PlayerInfoDto.SetLastCurHPSP(linkedCharacter.CurHP, linkedCharacter.CurSP); // 현재 체력과 스태미너 저장
            UserDataModel.Singleton.PlayerInfoDto.SetLastCurResAmmo(linkedCharacter.CurrentWeapon.CurAmmo, linkedCharacter.CurrentWeapon.ReserveAmmo); // 현재 탄약과 예비 탄약 저장
            UserDataModel.Singleton.PlayerInfoDto.SaveData(); // 데이터 저장
        }


        private void Update()
        {
            if (linkedCharacter == null) { return; }

            Vector2 inputMove = InputManager.Singleton.InputMove; // 이동 입력 벡터 가져오기

            linkedCharacter.SetMovementForward(mainCamera.transform.forward); // 카메라의 전방 방향으로 설정
            linkedCharacter.Move(inputMove); // 캐릭터 이동 처리
            linkedCharacter.Rotate(CameraSystem.Instance.AimingPoint); // 카메라 시스템에서 조준 지점을 가져와 회전 처리
            linkedCharacter.AimingPoint = CameraSystem.Instance.AimingPoint; // 캐릭터의 조준 지점을 카메라 시스템에서 가져옴
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        void CameraRotation()
        {
            Vector2 inputLook = InputManager.Singleton.InputLook; // 마우스 이동 입력 벡터 가져오기
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

        void OnCharacterDeadEvent()
        {
            UIManager.Show<GameOverUI>(UIList.GameOverUI); // 게임 오버 UI 표시

            StartCoroutine(CharacterDeadRoutine());
        }

        IEnumerator CharacterDeadRoutine()
        {
            float deathDuration = 3f; // 죽음 애니메이션 지속 시간
            Time.timeScale = 0.3f; // 30% 속도로 느리게

            yield return new WaitForSeconds(deathDuration);
            Time.timeScale = 1f; // 시간 정상화

            UIManager.Hide<GameOverUI>(UIList.GameOverUI); // 게임 오버 UI 숨김
            Main.Singleton.ReloadScene(SceneType.Ingame); // 현재 씬을 다시 로드 (인게임 씬 리로드)
        }
    }
}
