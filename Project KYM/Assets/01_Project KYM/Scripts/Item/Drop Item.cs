using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class DropItem : MonoBehaviour, IInteractable
    {
        [field : SerializeField] ItemDataSO itemDataSO; // 드롭 아이템의 데이터 (ScriptableObject)

        [field: SerializeField] public GameEventListener gameEventListener; // 게임 이벤트 리스너, 에디터에서 연결 

        public string Key => "DropItem_" + itemDataSO.Id.ToString(); // 고유 키, 아이템 ID 기반
        public Sprite InteractionIcon => itemDataSO.Icon; // 아이템 아이콘
        public string InteractionMessage => itemDataSO.ItemName; // 아이템 이름
        public bool  IsOnceInteractable => true; // 한 번만 상호작용 가능

        private bool isDestroyByInteract = false; // 인터랙션으로 인해 파괴되었는지 여부


        public void Interact() 
        {
            UserDataModel.Singleton.PlayerItemDtoDictionary.TryGetValue(itemDataSO.ItemName, out PlayerItemDTO playerItemDTO);
            if (playerItemDTO != null) 
            {
                playerItemDTO.Count += 1; // 이미 인벤토리에 있는 아이템이면 개수 증가
            }
            else 
            {
                playerItemDTO = new PlayerItemDTO() { itemDataSO = itemDataSO, Count = 1 };
                UserDataModel.Singleton.PlayerItemDtoDictionary.Add(itemDataSO.ItemName, playerItemDTO); // 새 아이템이면 추가
            }

            gameEventListener.OnReceiveEvent("LootItem", itemDataSO.ItemName);
            isDestroyByInteract = true; // 인터랙션으로 인해 파괴됨을 표시
            Destroy(gameObject); // 아이템 획득 시 오브젝트 제거 
        }

        private void OnDisable()
        {
            if (!isDestroyByInteract) 
            {
                var interactionUI = UIManager.Singleton.GetUI<InteractionUI>(UIList.InteractionUI);
                interactionUI?.RemoveInteractionData(this); // 비활성화 시 인터랙션 UI에서 제거
            }
        }
    }
}
