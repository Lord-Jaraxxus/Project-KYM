using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class DropItem : MonoBehaviour, IInteractable
    {
        [field : SerializeField] ItemDataSO itemDataSO; // 드롭 아이템의 데이터 (ScriptableObject)

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

            Destroy(gameObject); // 아이템 획득 시 오브젝트 제거 
        }
    }
}
