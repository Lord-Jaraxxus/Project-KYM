using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging; // Rigging 관련 네임스페이스 추가 (필요한 경우)

namespace KYM
{
    public class CharacterBase : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefeb; // 총알 프리팹
        [SerializeField] private Transform bulletSpawnPoint; // 총알 발사 위치

        [SerializeField] private Rig aimingRig; // 조준 Rig (필요한 경우)
        [SerializeField] private Transform aimingPoint; // 조준 포인트 (필요한 경우)

        // "=>" 이렇게 적은 것을 Lambda(람다) 표현식이라고 합니다.
        public int MaxAmmo => maxAmmo;
        public int CurAmmo => curAmmo;
        public float MaxHP => maxHP;
        public float CurHP => curHP;
        public float MaxSp => maxSp;
        public float CurSp => curSp;

        public Vector3 AimingPoint 
        {
            set 
            {
                aimingPoint.transform.position = value; // 조준 포인트 위치 설정
            }
        }

        public bool IsWalk { get; set; } = false;
        public bool IsCrouch { get; set; } = false;
        public bool IsAiming { get; set; } = false;
        public Vector3 Aimtarget { get; set; } 
        public bool IsReloading { get; private set; } = false;

        private Animator animator; // Animator 컴포넌트
        private CharacterController characterController; // CharacterController 컴포넌트

        private float walkblend;
        private float crouchblend;
        private float aimingblend;

        private Vector3 movementForward;
        private float verticalVelocity;
        private float targetRotation;
        private float rotationVelocity;
        private float roatationSmoothTime = 0.15f; // 회전 부드러움 시간
        private float smoothHorizontal;
        private float smoothVertical;

        private float fireRate = 0.3f; // 발사 속도
        private float lastFireTime = 0f; // 마지막 발사 시간

        private int maxAmmo = 30; // 최대 탄약 수
        private int curAmmo = 30; // 현재 탄약 수

        private float maxHP = 1000f; // 최대 체력
        private float curHP = 1000f; // 현재 체력
        private float maxSp = 100f; // 최대 스태미나
        private float curSp = 100f; // 현재 스태미나

        public event System.Action<int, int> OnAmmoChanged; // 탄약 변경 이벤트 (Callback)

        private void Awake()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();

            var reloadState = animator.GetBehaviour<ReloadStateMachineBehaviour>();
            reloadState.setCharacter(this); // 재장전 상태 머신 동작에 캐릭터 설정
        }

        private void Update()
        {
            walkblend = Mathf.Lerp(walkblend, IsWalk ? 1f : 0f, Time.deltaTime);
            crouchblend = Mathf.Lerp(crouchblend, IsCrouch ? 1f : 0f, Time.deltaTime * 10f);

            bool isAimingRigEnabled = IsAiming && !IsReloading; // 조준 중이면서 재장전 중이 아닐 때만 조준 Rig 활성화
            aimingblend = Mathf.Lerp(aimingblend, isAimingRigEnabled ? 1f : 0f, Time.deltaTime * 10f);
            aimingRig.weight = aimingblend; // 조준 Rig의 가중치 설정 (필요한 경우)

            animator.SetFloat("Running", walkblend);
            animator.SetFloat("Aiming", aimingblend);
            animator.SetFloat("Crouch", crouchblend);
        }
        
        public void SetMovementForward(Vector3 forward)
        {
            movementForward = forward;
        }

        public void Move(Vector2 input)
        {
            bool isInputSomething = input.magnitude > 0.1f;
            if (!IsAiming) 
            {
                if (isInputSomething) 
                {
                    Vector3 inputDir = new Vector3(input.x, 0, input.y).normalized;
                    Vector3 worldDirection = Quaternion.LookRotation(movementForward) * inputDir;
                    targetRotation = Quaternion.LookRotation(worldDirection).eulerAngles.y;

                    float roatation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, roatationSmoothTime);
                    transform.rotation = Quaternion.Euler(0, roatation, 0);
                }
            }
            
            smoothHorizontal = Mathf.Lerp(smoothHorizontal, input.x, Time.deltaTime * 10f);
            smoothVertical = Mathf.Lerp(smoothVertical, input.y, Time.deltaTime * 10f);

            animator.SetFloat("Magnitude", input.magnitude);
            animator.SetFloat("Horizontal", smoothHorizontal);
            animator.SetFloat("Vertical", smoothVertical);
        }
        public void Rotate(Vector3 targetAimPoint)
        {
            if (!IsAiming)
                return;

            Vector3 aimTarget = targetAimPoint;
            aimTarget.y = transform.position.y; // Y축은 현재 캐릭터 위치로 고정
            Vector3 pos = transform.position;
            Vector3 aimDirection = (aimTarget - pos).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }

        public void Shoot()
        {
            if(Time.time - lastFireTime > fireRate && curAmmo > 0) // 발사 속도 제한 & 현재 탄약이 0보다 큰 경우
            {
                // Time.time : 현재 유니티의 시간을 의미 => 현재 유니티가 플레이 된지 3초 지났다면? => 3.0f
                GameObject newBullet = Instantiate(bulletPrefeb);
                bulletPrefeb.gameObject.SetActive(true); // 총알 프리팹 활성화
                newBullet.transform.SetPositionAndRotation(bulletSpawnPoint.position, bulletSpawnPoint.rotation); // 총알 발사 위치와 방향 설정

                lastFireTime = Time.time; // 마지막 발사 시간 업데이트
                curAmmo--; // 현재 탄약 감소

                OnAmmoChanged?.Invoke(curAmmo, maxAmmo); // 탄약 변경 이벤트 호출
            }
        }

        public void Reload()
        {
            if (IsReloading) return; // 이미 재장전 중이라면 ㄴㄴ

            IsReloading = true; // 재장전 상태 설정
            animator.SetTrigger("Reload Trigger"); // 재장전 애니메이션 트리거 설정
        }

        public void SetReloadComplete()
        {
            curAmmo = maxAmmo; // 현재 탄약을 최대 탄약으로 설정
            IsReloading = false; // 재장전 완료 상태로 설정

            OnAmmoChanged?.Invoke(curAmmo, maxAmmo); // 탄약 변경 이벤트 호출
        }
    }
}
