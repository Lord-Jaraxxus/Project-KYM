using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class PatrolState : AIStateBase
    {
        public override AIStateType StateType => AIStateType.Patrol;

        [SerializeField] private Transform[] patrolPoints; // 순찰 지점들을 저장할 배열
        [SerializeField] private int currentPatrolIndex = 0; // 현재 순찰 지점 인덱스

        private AIBrain ownerBrain; // AIBrain 인스턴스를 저장할 변수

        public override void onEnterState(AIBrain brain)
        {
            ownerBrain = brain; // AIBrain 인스턴스를 저장
            brain.AIController.OnDestinationReachedEvent -= OnDestinationReached; // 이벤트 중복 등록 방지
            brain.AIController.OnDestinationReachedEvent += OnDestinationReached; // 목적지 도달 이벤트 등록

            currentPatrolIndex = 0; // 순찰 시작 시 첫 번째 지점으로 초기화
            int index = currentPatrolIndex;
            Vector3 destination = patrolPoints[index].position;
            brain.AIController.SetDestination(destination); // AIController를 통해 NavMeshAgent의 목표 위치 설정
        }

        public override void onExitState(AIBrain brain)
        {
            brain.AIController.OnDestinationReachedEvent -= OnDestinationReached; // 순찰 상태를 종료할 때 이벤트 구독 해제
        }

        public override void onUpdateState(AIBrain brain)
        {
        }


        void OnDestinationReached() 
        {
            currentPatrolIndex++; // 다음 순찰 지점으로 이동
            int indwx = currentPatrolIndex % patrolPoints.Length; // 순찰 지점 인덱스가 배열 길이를 초과하지 않도록 처리
            Vector3 destination = patrolPoints[indwx].position; // 다음 순찰 지점의 위치 가져오기
            ownerBrain.AIController.SetDestination(destination); // AIController를 통해 NavMeshAgent의 목표 위치 설정
        }
    }
}
