using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace KYM 
{
    public class ReloadStateMachineBehaviour : StateMachineBehaviour
    {
        private CharacterBase linkedCharacter;
        public void setCharacter(CharacterBase character) 
        { 
            this.linkedCharacter = character;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            this.linkedCharacter.SetReloadComplete();
        }
    }
}
