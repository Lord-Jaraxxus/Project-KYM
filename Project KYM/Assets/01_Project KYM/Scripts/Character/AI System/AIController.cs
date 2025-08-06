using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KYM
{
    public class AIController : MonoBehaviour
    {
        [field:SerializeField] public string MonsterID; // 몬스터 ID (AI 캐릭터의 ID로 사용)

        public Transform targetDestinationPoint; // AI 캐릭터가 이동할 목표 위치 (Transform 타입으로 설정)

        private CharacterBase character; // 캐릭터 베이스 컴포넌트
        private NavMeshAgent navAgent;

        private void Awake()
        {
            character = GetComponent<CharacterBase>();
            navAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            navAgent.updatePosition = false;
            navAgent.updateRotation = false;

            var statData = GameDataModel.Singleton.MonsterDataDto.GetMonsterStatData(MonsterID); // 몬스터 스탯 데이터 가져오기
            character.Initialize(statData.MonsterStat); // AI 캐릭터 스탯 초기화

            navAgent.SetDestination(targetDestinationPoint.position); // NavMeshAgent의 목표 위치 설정
        }

        void Update()
        {
            // navAgent.destination : 진짜 목표지점
            // navAgent.steeringTarget : 가는 길의 첫번째 코너 지점
            // navAgent.nextPosition : 다음 프레임에서 NavMeshAgent가 이동할 위치

            float distance = Vector3.Distance(transform.position, navAgent.destination); // 현재 위치와 목표 위치 간의 거리 계산
            if (distance > 0.1f)
            {
                navAgent.nextPosition = transform.position; // NavMeshAgent의 위치를 현재 캐릭터 위치로 업데이트 (Agent를 땡겨오는 느낌)

                Vector3 normal = (navAgent.steeringTarget - transform.position).normalized; // NavMeshAgent의 목표 위치와 현재 위치의 차이 계산
                Vector2 input = new Vector2(normal.x, normal.z); // 2D 평면에서의 이동 입력 벡터 생성
                character.Move(input); // 캐릭터 이동 업데이트
                //character.Rotate(); // 캐릭터 회전 업데이트
            }
            else 
            {
                // TODO: 멈추기
                // navAgent.SetDestination(transform.position); // 목표 위치가 가까워지면 NavMeshAgent의 목표 위치를 현재 위치로 설정하여 멈춤
            }
        }
    }
}
