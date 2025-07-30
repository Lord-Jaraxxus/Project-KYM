using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class GameDataModel : SingletonBase<GameDataModel>
    {
        [field: SerializeField] public PlayerStatDto PlayerStatDto { get; private set; } = new(); // 플레이어 스탯 DTO
        [field: SerializeField] public WeaponDataDto WeaponDataDto { get; private set; } = new(); // 무기 데이터 DTO

        public void Initialize()
        {
            CharacterStatDataSO playerStatSo = Resources.Load<CharacterStatDataSO>("Character/CharacterStat/PlayerCharacterStatData");
            WeaponDataSO weaponDataSO = Resources.Load<WeaponDataSO>("Weapon/WeaponData/WeaponData");
            PlayerStatDto.initailize(playerStatSo); // 플레이어 스탯 데이터 초기화
            WeaponDataDto.initialize(weaponDataSO); // 무기 데이터 초기화
        }
    }
}
