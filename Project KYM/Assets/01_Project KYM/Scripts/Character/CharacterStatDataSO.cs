using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    [CreateAssetMenu(fileName = "CharacterStatData", menuName = "PROJECT KYM/CharacterStatData")]

    public class CharacterStatDataSO : ScriptableObject
    {
        [field: SerializeField] public float MaxHP { get; set; } = 1000f; // 최대 체력
        [field: SerializeField] public float MaxSP { get; set; } = 100f;  // 최대 스태미나
    }
}
