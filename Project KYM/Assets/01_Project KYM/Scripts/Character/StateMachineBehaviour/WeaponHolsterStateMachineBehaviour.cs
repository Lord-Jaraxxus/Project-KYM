using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class WeaponHolsterStateMachineBehaviour : StateMachineBehaviour
    {
        private CharacterBase linkedCharacter;
        public void SetCharacter(CharacterBase character) => linkedCharacter = character;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            linkedCharacter?.SetHolsterProgressComplete();
        }
    }
}
