using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    [CreateAssetMenu(fileName = "CharacterStatData", menuName = "PROJECT KYM/CharacterStatData")]

    public class CharacterStatDataSO : ScriptableObject
    {
        [field: SerializeField] public string ID { get; set; } // 캐릭터 ID
        [field: SerializeField] public float MaxHP { get; set; } = 1000f; // 최대 체력
        [field: SerializeField] public float MaxSP { get; set; } = 100f;  // 최대 스태미나
        [field: SerializeField] public float SpConsumeRate { get; set; } = 10f; // 스태미나 소모 속도 
        [field: SerializeField] public float SpRecoveryRate { get; set; } = 5f; // 스태미너 회복 속도
    }
}
