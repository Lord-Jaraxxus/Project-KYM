using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class BulletinBoard : MonoBehaviour, IInteractable
    {
        public string Key => "BulletinBoard";

        [SerializeField] private Sprite interactionIcon;
        public Sprite InteractionIcon => interactionIcon;
        public string InteractionMessage => "Bulletin Board";

        private bool isOpen = false;

        public void Start()
        {
            GameEventListener.Instance.OnReceiveGameEvent += SetIsOpen; 
        }

        public void SetIsOpen(string eventName, string eventData)
        {
            if (eventName != "CloseBulletinBoardUI") return; // CloseBulletinBoardUI 이벤트가 아니면 무시
            else if (!isOpen) return; // UI가 열려있지 않으면 무시
            else if (eventData != null) return; // 이벤트 데이터가 있으면 무시

            isOpen = false; // UI가 닫혀있음을 표시

            // Debug.Log("CloseBulletinBoardUI 이벤트 전달됨");
        }

        public void Interact()
        {
            if (isOpen) return; // 중복해서 UI 띄우는 것 방지 
            UIManager.Show<BulletinBoardUI>(UIList.BulletinBoardUI); 
            isOpen = true; // UI가 열려있음을 표시
        }
    }
}
