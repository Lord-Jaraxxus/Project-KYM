using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "PROJECT KYM/ItemData")]

    public class ItemDataSO : ScriptableObject
    {
        public int Id; // 아이템 ID
        public string ItemName; // 아이템 이름
        public string Description; // 아이템 설명
        public Sprite Icon; // 아이템 아이콘
    }
}
