using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class AbortState : AIStateBase
    {
        public override AIStateType StateType => AIStateType.Abort;
        private CharacterBase target;
        Vector3 abortDirection;
        public float abortTime = 3.0f; // 도망가는 시간 설정
        public float elapsedTime = 0.0f; // 경과 시간 초기화

        public override void onEnterState(AIBrain brain)
        {
            target = brain.AISensor.DetectedTarget; // 감지된 캐릭터를 타겟으로 설정
            if (target == null)
            {
                abortDirection = -brain.transform.forward; // 타겟이 없으면 현재 바라보는 반대 방향으로 도망감
            }
            else 
            {
                abortDirection = (brain.transform.position - target.transform.position).normalized; // 타겟으로부터 도망가는 방향 계산
            }

            Vector3 abortDestination = brain.transform.position + new Vector3(abortDirection.x, 0, abortDirection.z) * 1000.0f; // 도망갈 목적지 설정 
            brain.AIController.SetDestination(abortDestination); 

            Debug.Log("Abort State Entered. Aborting from target in direction: " + abortDirection);
        }

        public override void onExitState(AIBrain brain)
        {
            elapsedTime = 0.0f; // 상태를 나갈 때 경과 시간 초기화
            target = null;
        }

        public override void onUpdateState(AIBrain brain)
        {
            elapsedTime += Time.deltaTime; // 경과 시간 업데이트

            if (elapsedTime >= abortTime)
            {
                // 도망가는 시간이 끝나면 기본 상태(예: PatrolState)로 전환
                // brain.ChangeState(brain.DefaultState);
             }
        }

    }
}
