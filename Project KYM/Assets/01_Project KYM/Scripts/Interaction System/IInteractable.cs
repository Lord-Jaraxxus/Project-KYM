using UnityEngine;

namespace KYM
{
    public interface IInteractable
    {
        public string Key { get; } // 상호작용 객체의 고유 키
        public Sprite InteractionIcon { get; } // 상호작용 아이콘
        public string InteractionMessage { get; } // 상호작용 메시지

        void Interact(); // 상호작용 메소드 정의
    }
}
