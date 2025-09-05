using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging; // Rigging 관련 네임스페이스 추가 (필요한 경우)

namespace KYM
{
    public class CharacterBase : MonoBehaviour, IHasHp, IHittable
    {

        [SerializeField] private Rig aimingRig; // 조준 Rig (필요한 경우)
        [SerializeField] private Transform leftHandIKTarget; // 왼손 IK 타겟
        [SerializeField] private Transform aimingPoint; // 조준 포인트 (필요한 경우)
        [SerializeField] private WeaponBase primaryWeapon;  // 주 무기 프리팹
        [SerializeField] private WeaponBase secondaryWeapon; // 보조 무기 프리팹
        
        public AnimationEventListener AnimationEventListener => animationEventListener; // 애니메이션 이벤트 리스너 반환
        public WeaponBase CurrentWeapon => currentWeapon; // 현재 장착된 무기 반환
        public float MaxHP => characterStat.MaxHP;
        public float CurHP => curHP;
        public float MaxSP => characterStat.MaxSP;
        public float CurSP => curSP;

        public Vector3 AimingPoint 
        {
            set 
            {
                aimingPoint.transform.position = value; // 조준 포인트 위치 설정
            }
        }

        public bool IsWalk { get; set; } = true;
        public bool IsCrouch { get; set; } = false;
        public bool IsAiming { get; set; } = false;
        public Vector3 Aimtarget { get; set; } 
        public bool IsReloading { get; private set; } = false;
        public bool IsDead { get; private set; } = false;

        private Animator animator; // Animator 컴포넌트
        private CharacterController characterController; // CharacterController 컴포넌트
        private CharacterStatDataSO characterStat; // 캐릭터 스탯 데이터 (ScriptableObject)
        private WeaponBase currentWeapon; // 현재 장착된 무기
        private AnimationEventListener animationEventListener; // 애니메이션 이벤트 리스너

        private float runningblend;
        private float crouchblend;
        private float aimingblend;

        private Vector3 movementForward;
        private float verticalVelocity;
        private float targetRotation;
        private float rotationVelocity;
        private float roatationSmoothTime = 0.15f; // 회전 부드러움 시간
        private float smoothHorizontal;
        private float smoothVertical;

        [SerializeField] private float curHP; // 현재 체력
        [SerializeField] private float curSP; // 현재 스태미나

        public float SpConsumeRate => characterStat.SpConsumeRate;
        public float SpRecoverRate => characterStat.SpRecoveryRate;

        private bool isLockRunning = false; // 달리기 잠금 상태 (필요한 경우)

        public event System.Action<int, int, int> OnAmmoChanged; // 탄약 변경 이벤트 (Callback)
        public event System.Action<float, float> OnHpChanged; // 체력 변경 이벤트 (Callback) 
        public event System.Action<float, float> OnSpChanged; // 스태미너 변경 이벤트 (Callback)
        public event System.Action OnCharacterDead; // 사망 이벤트 (Callback)

        public event System.Action<CharacterBase> OnCharacterMoribund; // 캐릭터 위기 상태 이벤트 (Callback) , AI용
        [SerializeField] private float moribundThreshold = 0.3f; // 위기 상태 임계값 (예: 최대 체력의 30% 이하)

        private void Awake()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
            animationEventListener = GetComponent<AnimationEventListener>();

            var reloadState = animator.GetBehaviour<ReloadStateMachineBehaviour>();
            reloadState.setCharacter(this); // 재장전 상태 머신 동작에 캐릭터 설정
        }

        private void Start()
        {
            animationEventListener.OnReceiveAnimationEvent += OnCallbackReceiveAnimationEvent; // 애니메이션 이벤트 리스너 콜백 등록
            InputManager.Singleton.OnInputInteract += OnReceiveInputInteract;
        }
        void OnCallbackReceiveAnimationEvent(string eventName) 
        {
            switch (eventName) 
            {
                case "PlayUnloadSound":
                    currentWeapon?.PlayUnloadSound(); // 재장전 완료 처리
                    break;

                case "PlayLoadSound":
                    currentWeapon?.PlayLoadSound(); // 재장전 완료 처리
                    break;
            }
        }
        void OnReceiveInputInteract() 
        {
            animator.SetTrigger("IsInteract"); // 상호작용 애니메이션 트리거 설정
        }

        private void Update()
        {
            runningblend = Mathf.Lerp(runningblend, (!IsWalk && curSP > 0f && !isLockRunning) ? 1f : 0f, Time.deltaTime);
            crouchblend = Mathf.Lerp(crouchblend, IsCrouch ? 1f : 0f, Time.deltaTime * 10f);

            bool isAimingRigEnabled = IsAiming && !IsReloading; // 조준 중이면서 재장전 중이 아닐 때만 조준 Rig 활성화
            aimingblend = Mathf.Lerp(aimingblend, isAimingRigEnabled ? 1f : 0f, Time.deltaTime * 10f);
            aimingRig.weight = aimingblend; // 조준 Rig의 가중치 설정 (필요한 경우)

            animator.SetFloat("Running", runningblend);
            animator.SetFloat("Aiming", aimingblend);
            animator.SetFloat("Crouch", crouchblend);
        }

        public void Initialize(CharacterStatDataSO statDataSo, bool isPlayer) 
        {
            this.characterStat = statDataSo; // 캐릭터 스탯 데이터 초기화

            if(isPlayer)
            {
                this.curHP = UserDataModel.Singleton.PlayerInfoDto.LastCurHP;// 플레이어의 마지막 체력 불러옴
                this.curSP = UserDataModel.Singleton.PlayerInfoDto.LastCurSP; // 플레이어의 마지막 스태미너 불러옴
            }
        }

        public void InitWeapon(WeaponDataSO weaponDataSO, bool isNpc = false) 
        {
            int curAmmo = isNpc ? weaponDataSO.MaxAmmo : UserDataModel.Singleton.PlayerInfoDto.LastCurAmmo; // 플레이어의 마지막 현재 탄약 불러옴
            int reserveAmmo = isNpc ? int.MaxValue : UserDataModel.Singleton.PlayerInfoDto.LastResAmmo; // 플레이어의 마지막 소지 탄약 불러옴

            Transform rightHandTransform = animator.GetBoneTransform(HumanBodyBones.RightHand); // 오른손 Transform 가져오기 
            currentWeapon = Instantiate(primaryWeapon, rightHandTransform); // 주 무기 인스턴스화
                currentWeapon.transform.SetLocalPositionAndRotation(
                weaponDataSO.InitPosition, // 무기 위치 설정
                Quaternion.Euler(weaponDataSO.InitRotation)); // 무기 회전 설정

            currentWeapon.Init(this, curAmmo, reserveAmmo); // 현재 캐릭터로 무기 초기화

            leftHandIKTarget.SetParent(currentWeapon.transform); // 왼손 IK 타겟을 현재 무기의 자식으로 설정
            leftHandIKTarget.SetLocalPositionAndRotation(
                weaponDataSO.LeftHandIKOffsetPosition, // 왼손 IK 타겟 위치 설정
                Quaternion.Euler(weaponDataSO.LeftHandIKOffsetRotation)); // 왼손 IK 타겟 회전 설정

            var rigBuilder = GetComponentInChildren<RigBuilder>(); // RigBuilder 컴포넌트 가져오기
            rigBuilder.Build(); // RigBuilder 빌드
        }

        
        public void SetMovementForward(Vector3 forward)
        {
            movementForward = forward;
        }

        public void Move(Vector2 input)
        {
            if (IsDead) return; // 캐릭터가 죽은 상태라면 이동하지 않음

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

            if (isInputSomething && !IsWalk && curSP > 0f && !isLockRunning) // 달리기 상태가 아니고 스태미너가 0보다 큰 경우
            {
                ConsumeSp(SpConsumeRate * Time.deltaTime); // 스태미너 소모
            }
            else
            {
                RecoverySp(SpConsumeRate * Time.deltaTime); // 초당 SP 회복
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
            if (IsDead) return; // 캐릭터가 죽은 상태라면 발사하지 않음

            currentWeapon.Fire(); // 현재 무기의 발사 메서드 호출
            OnAmmoChanged?.Invoke(currentWeapon.CurAmmo, currentWeapon.MaxAmmo, currentWeapon.ReserveAmmo); // 탄약 변경 이벤트 호출
        }

        public void Reload()
        {
            if (IsReloading) return; // 이미 재장전 중이라면 ㄴㄴ

            IsReloading = true; // 재장전 상태 설정
            animator.SetTrigger("Reload Trigger"); // 재장전 애니메이션 트리거 설정
        }

        public void AddReserveAmmo(int amount)
        {
            currentWeapon.AddReserveAmmo(amount); // 현재 무기에 소지 탄약 추가 메서드 호출
        }

        public void SetReloadComplete()
        {
            IsReloading = false; // 재장전 완료 상태로 설정
            currentWeapon.Reload(); // 현재 무기 재장전 메서드 호출
            OnAmmoChanged?.Invoke(currentWeapon.CurAmmo, currentWeapon.MaxAmmo, currentWeapon.ReserveAmmo); // 탄약 변경 이벤트 호출
        }

        // IHittable 인터페이스 구현
        public void OnHit(float damage)
        {
            TakeDamage(damage);
        }

        // IHasHp 인터페이스 구현
        public void TakeDamage(float damage)
        {
            curHP -= damage;
            curHP = Mathf.Clamp(curHP, 0f, this.MaxHP);
            OnHpChanged?.Invoke((int)curHP, (int)this.MaxHP);

            if (curHP <= (this.MaxHP * moribundThreshold) && characterStat.CharType == CharacterType.AI) // 체력이 위기 상태 임계값 이하이고 AI 캐릭터인 경우
            {
                OnCharacterMoribund?.Invoke(this); // 캐릭터 위기 상태 이벤트 호출 (AI용)
                // Debug.Log("AI Character is in moribund state."); // 디버그 로그 출력
            }

            if (curHP <= 0 && !IsDead)  // 체력이 0 이하이고 죽은 상태가 아닌 경우
                Die();
        }

        void Die() // 캐릭터가 죽었을 때 호출되는 메서드
        {
            IsDead = true; // 죽은 상태로 설정
            animator.SetTrigger("IsDead"); // 죽음 애니메이션 트리거 설정
            characterController.enabled = false; // 플레이어 캐릭터 컨트롤러 비활성화

            OnCharacterDead?.Invoke(); // 사망 이벤트 호출
            StartCoroutine(DieRoutine()); // 죽음 후 일정 시간 후에 오브젝트 제거
        }
        private IEnumerator DieRoutine()
        {
            float deathDuration = 3f; // 죽음 애니메이션 지속 시간
            yield return new WaitForSeconds(deathDuration);

            Destroy(gameObject); // 캐릭터 오브젝트 제거;
        }

        public void Heal(float amount)
        {
            curHP += amount;
            curHP = Mathf.Clamp(curHP, 0f, this.MaxHP);
            OnHpChanged?.Invoke((int)curHP, (int)this.MaxHP);
        }


        public void ConsumeSp(float amount)
        {
            if (curSP <= 0 || isLockRunning) return; // 현재 스태미너가 0 이하인 경우 소비하지 않음
            curSP -= amount; // 스태미너 감소
            curSP = Mathf.Clamp(curSP, 0, this.MaxSP); // 스태미너를 0과 최대 스태미너 사이로 제한
            OnSpChanged?.Invoke(curSP, this.MaxSP); // 스태미너 변경 이벤트 호출

            if (curSP <= 0) 
            {
                isLockRunning = true; // 스태미너가 0 이하일 때 달리기 잠금 상태 설정
            }
        }

        public void RecoverySp(float amount)
        {
            if (curSP >= this.MaxSP) return; // 이미 풀일 땐 회복 X
            curSP += amount; 
            curSP = Mathf.Clamp(curSP, 0, this.MaxSP); // 스태미너를 0과 최대 스태미너 사이로 제한

            if (curSP >= 20)
            {
                isLockRunning = false ; // 스태미너가 0 이하일 때 달리기 잠금 상태 설정
            }

            OnSpChanged?.Invoke(curSP, this.MaxSP); // 스태미너 변경 이벤트 호출
        }
    }
}
