using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class CombatState : AIStateBase
    {
        public override AIStateType StateType => AIStateType.Combat;

        private CharacterBase target; // 타겟은 결국 "Player"가 될 것임

        public override void onEnterState(AIBrain brain)
        {
            target = brain.AISensor.DetectedTarget; // 감지된 캐릭터를 타겟으로 설정

            brain.AISensor.OnDetectedCharacterEvent -= OnCallbackDetectedCharacter; // 이벤트 중복 등록 방지 
            brain.AISensor.OnDetectedCharacterEvent += OnCallbackDetectedCharacter; // 타겟이 감지되면 이벤트를 통해 콜백을 받습니다.
            brain.AIController.LinkedCharacter.IsAiming = true; // CombatState에 진입하면 AI가 타겟을 바라보는 상태로 설정합니다.
            brain.AIController.SetDestination(transform.position); // CombatState에 진입하면 AI의 목적지를 현재 위치로 설정합니다. (이동을 멈추기 위함)
        }

        public override void onExitState(AIBrain brain)
        {
            brain.AISensor.OnDetectedCharacterEvent -= OnCallbackDetectedCharacter; // CombatState를 종료할 때 이벤트 구독 해제
            brain.AIController.LinkedCharacter.IsAiming = false; // CombatState를 종료하면 AI가 타겟을 바라보는 상태를 해제합니다.
        }

        public override void onUpdateState(AIBrain brain)
        {
            if (target == null) return; // 타겟이 없으면 아무것도 하지 않음

            brain.AIController.LinkedCharacter.IsAiming = true; // 타겟을 바라보는 상태로 설정
            brain.AIController.LinkedCharacter.Rotate(target.transform.position); // 타겟의 위치를 바라보도록 회전
            brain.AIController.LinkedCharacter.AimingPoint = target.transform.position; // 타겟의 위치를 바라보도록 설정

            int curAmmo = brain.AIController.LinkedCharacter.CurrentWeapon.CurAmmo; // 현재 무기의 탄약 수를 가져옵니다.
            if(curAmmo > 0)
            {
                brain.AIController.LinkedCharacter.Shoot(); // 공격을 수행합니다.
            }
            else
            {
                brain.AIController.LinkedCharacter.Reload(); // 탄약이 없으면 재장전을 수행합니다.
            }
        }

        private void OnCallbackDetectedCharacter(CharacterBase character)
        {
            target = character; // 감지된 캐릭터를 타겟으로 설정
        }

    }
}
