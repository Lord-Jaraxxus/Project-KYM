using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class NPC_Dialogue : MonoBehaviour, IInteractable
    {
        public string Key => "NPC_Dialogue";
        [SerializeField] private Sprite interactionIcon;
        public Sprite InteractionIcon => interactionIcon;
        public string InteractionMessage => "NPC_Dialogue";

        private Animator animator;

        private bool isOpen = false;

        public void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void Interact()
        {
            if (!isOpen)
            { 
                UIManager.Show<DialogueUI>(UIList.DialogueUI); 
                isOpen = true; // UI가 열려있음을 표시
            }
            else
            {
                UIManager.Hide<DialogueUI>(UIList.DialogueUI);
                isOpen = false; // UI가 닫혀있음을 표시
            }

            //animator.SetTrigger("IsDead"); // 나중에 대화 애니메이션으로?
        }
    }
}
