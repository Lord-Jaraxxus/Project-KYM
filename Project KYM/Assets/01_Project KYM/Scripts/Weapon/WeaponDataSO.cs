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

    }
}
