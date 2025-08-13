using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KYM
{
    public class AIController : MonoBehaviour
    {
        public CharacterBase LinkedCharacter => character; // AI가 제어하는 캐릭터를 외부에서 접근할 수 있도록 프로퍼티로 노출합니다.

        [field: SerializeField] public string MonsterID; // 몬스터 ID (AI 캐릭터의 ID로 사용)

        private CharacterBase character; // 캐릭터 베이스 컴포넌트
        private NavMeshAgent navAgent;

        [Header("Arrival Settings")]
        [SerializeField] private float stoppingDistance = 0.1f; // 목표 위치에 도달했을 때의 최소 거리
        [SerializeField] private float stopEpsion = 0.03f; // 도착 판단을 위한 오차 범위 (목표 위치와 현재 위치 간의 거리 차이가 이 값보다 작으면 도착으로 간주)
        [SerializeField] private float resumeEpsion = 0.12f; // 다시 움직일 때의 여유 거리

        public System.Action OnDestinationReachedEvent; // 목적지 도달 이벤트

        private bool isStopped; // AI가 멈췄는지 여부를 나타내는 플래그
        private bool arrivalInvokedOnce; // 도착 이벤트가 한 번만 호출되도록 하는 플래그

        private void Awake()
        {
            character = GetComponent<CharacterBase>();
            navAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            navAgent.updatePosition = false;
            navAgent.updateRotation = false;
            navAgent.stoppingDistance = stoppingDistance; // NavMeshAgent의 목표 위치에 도달했을 때의 최소 거리 설정

            var statData = GameDataModel.Singleton.MonsterDataDto.GetMonsterStatData(MonsterID); // 몬스터 스탯 데이터 가져오기
            character.Initialize(statData.MonsterStat); // AI 캐릭터 스탯 초기화
            character.InitWeapon(GameDataModel.Singleton.WeaponDataDto.weaponDataSO, true); // AI 캐릭터 무기 초기화
        }

        void Update()
        {
            navAgent.nextPosition = transform.position; // NavMeshAgent의 다음 위치를 현재 위치로 설정 

            if (navAgent.pathPending ||
                !navAgent.hasPath ||
                navAgent.pathStatus == NavMeshPathStatus.PathInvalid ||
                navAgent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                StopMovement();
                return;
            }

            float remainDistance = navAgent.remainingDistance; // NavMeshAgent의 남은 거리 계산
            if (!isStopped)
            {
                if (remainDistance <= (stoppingDistance + stopEpsion))
                {
                    StopMovement(); // 남은 거리가 설정된 거리 이하이면 이동을 멈춤
                    InvokeArriivalOnce();
                    return;
                }
            }
            else
            {
                if (remainDistance > (stoppingDistance + resumeEpsion))
                {
                    isStopped = false; // 남은 거리가 설정된 거리 이상이면 AI가 다시 움직일 수 있도록 설정
                    navAgent.isStopped = false; // NavMeshAgent를 활성화하여 이동 가능 상태로 설정
                }
                else
                {
                    StopMovement(); // AI가 멈춘 상태라면 이동을 멈춤
                    return;
                }
            }

            // 위에서 return 되지 않은 상태이면 움직여야하는 상태라고 판단
            navAgent.isStopped = false; // NavMeshAgent를 활성화하여 이동 가능 상태로 설정

            Vector3 toCorner = navAgent.steeringTarget - transform.position; // NavMeshAgent의 목표 위치와 현재 위치 간의 벡터 계산
            toCorner.y = 0; // y축은 무시하고 수평 거리만 고려

            if (toCorner.sqrMagnitude < 0.01f)
            {
                StopMovement();
                return;
            }

            Vector3 dir = toCorner.normalized; // 목표 위치 방향 벡터 계산
            Vector2 input = new Vector2(dir.x, dir.z); // 수평 입력 벡터 계산
            character.Move(input); // 캐릭터 이동 처리
        }


        public void SetDestination(Vector3 destination)
        {
            arrivalInvokedOnce = false; // 도착 이벤트가 호출되지 않았음을 초기화
            isStopped = false; // AI가 멈추지 않았음을 초기화

            navAgent.isStopped = false; // NavMeshAgent를 활성화하여 이동 가능 상태로 설정
            navAgent.SetDestination(destination); // NavMeshAgent의 목표 위치 설정
        }

        private void StopMovement()
        {
            isStopped = true; // AI가 멈춘 상태로 설정
            navAgent.isStopped = true; // NavMeshAgent를 비활성화하여 이동 불가능 상태로 설정
            character.Move(Vector2.zero); // 캐릭터 이동을 멈춤
        }

        private void InvokeArriivalOnce()
        {
            if (arrivalInvokedOnce) return;
            arrivalInvokedOnce = true; // 도착 이벤트가 한 번 호출되었음을 표시
            OnDestinationReachedEvent?.Invoke(); // 도착 이벤트 호출
        }
    }
}
