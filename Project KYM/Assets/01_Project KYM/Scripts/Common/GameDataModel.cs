using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class GameDataModel : SingletonBase<GameDataModel>
    {
        [field: SerializeField] public PlayerStatDto PlayerStatDto { get; private set; } = new(); // 플레이어 스탯 DTO
        [field: SerializeField] public Dictionary<string /*Weapon name*/, WeaponDataDto> WeaponDataMap { get; private set; } = new(); // 무기 데이터 DTO
        [field: SerializeField] public MonsterStatDto MonsterDataDto { get; private set; } = new(); // 몬스터 스탯 DTO

        public void Initialize()
        {
            CharacterStatDataSO playerStatSo = Resources.Load<CharacterStatDataSO>("Character/CharacterStat/PlayerCharacterStatData");
            WeaponDataSO[] weaponDataSOs = Resources.LoadAll<WeaponDataSO>("Weapon/WeaponData");
            foreach (WeaponDataSO so in weaponDataSOs)
            {
                string key = so.WeaponId; // Id를 키로 사용

                // DTO 만들고 초기화
                var dto = new WeaponDataDto();
                dto.initialize(so);

                WeaponDataMap.Add(key, dto); // 무기 데이터 DTO 맵에 추가
            }

            CharacterStatDataSO[] arrMonsterStatSO = Resources.LoadAll<CharacterStatDataSO>("Character/MonsterStat/");
            foreach (CharacterStatDataSO monsterStatSo in arrMonsterStatSO)
            {
                MonsterStatDto.MonsterStatData monsterData = new MonsterStatDto.MonsterStatData(monsterStatSo.ID, monsterStatSo);
                MonsterDataDto.MonsterStatDatas.Add(monsterData.MonsterID, monsterData); // 몬스터 스탯 데이터 추가
            }

            PlayerStatDto.initailize(playerStatSo); // 플레이어 스탯 데이터 초기화
        }
    }
}
