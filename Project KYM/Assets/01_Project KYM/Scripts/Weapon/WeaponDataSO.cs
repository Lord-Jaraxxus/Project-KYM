using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "PROJECT KYM/WeaponData")]
    public class WeaponDataSO : ScriptableObject
    {
        [field: SerializeField] public int MaxAmmo { get; set; } // 최대 탄약 수
        [field: SerializeField] public Vector3 LeftHandIKOffsetPosition { get; set; } // 왼손 IK 오프셋 위치
        [field: SerializeField] public Vector3 LeftHandIKOffsetRotation { get; set; } // 왼손 IK 오프셋 회전
        [field: SerializeField] public Vector3 InitPosition { get; set; } // 초기 위치 (왼손을 기준으로 상대적..local?)
        [field: SerializeField] public Vector3 InitRotation { get; set; } // 초기 회전 (왼손을 기준으로 상대적..loacl?)
        [field: SerializeField] public float FireRate { get; set; } // 발사 속도
        [field: SerializeField] public float Damage { get; set; } // 피해량

        // AI가 짜준 예시인데, 나중에 필요할지도? 아니면 그냥 적용을 바로 해버릴까? 일단 좀 나중에.
        // [field: SerializeField] public float BulletSpeed { get; set; } // 총알 속도
        // [field: SerializeField] public float ReloadTime { get; set; } // 재장전 시간
    }
}
