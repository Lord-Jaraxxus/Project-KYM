using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM 
{
    public interface IHasHp
    {
        float CurHP { get; } // 현재 체력
        float MaxHP { get; } // 최대 체력

        void TakeDamage(float damage); // 피해를 받는 메서드
        void Heal(float amount); // 체력을 회복하는 메서드
    }
}
