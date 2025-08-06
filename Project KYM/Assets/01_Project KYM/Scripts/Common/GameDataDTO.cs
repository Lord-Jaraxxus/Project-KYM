using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class GameDataDTO { }

    [System.Serializable]
    public class  PlayerStatDto : GameDataDTO
    {
        [field: SerializeField] public CharacterStatDataSO playerCharacterStatSO { get; private set; } // 플레이어 캐릭터 스탯 데이터 (ScriptableObject)

        public void initailize(CharacterStatDataSO dataSO)
        {
            this.playerCharacterStatSO = dataSO;
        }
    }

    [System.Serializable]
    public class  MonsterStatDto : GameDataDTO
    {
        [System.Serializable]
        public class MonsterStatData
        {
            public MonsterStatData(string monsterID, CharacterStatDataSO monsterStat)
            {
                MonsterID = monsterID;
                MonsterStat = monsterStat;
            }

            [field: SerializeField] public string MonsterID { get; private set; } // 몬스터 ID
            [field: SerializeField] public CharacterStatDataSO MonsterStat { get; private set; } // 몬스터 스탯 데이터 (ScriptableObject)
        }
        [field: SerializeField] public UDictionary<string, MonsterStatData> MonsterStatDatas { get; private set; } = new(); // 몬스터 스탯 데이터 딕셔너리 (몬스터 ID를 키로 사용)

        public MonsterStatData GetMonsterStatData(string monsterID)
        {
            if (MonsterStatDatas.TryGetValue(monsterID, out MonsterStatData monsterStatData))
            {
                return monsterStatData;
            }
            else
            {
                Debug.LogError($"MonsterStatDto: 몬스터 ID '{monsterID}'에 해당하는 스탯 데이터가 없습니다.");
                return null;
            }
        }
    }

    public class WeaponDataDto : GameDataDTO
    {
        [field: SerializeField] public WeaponDataSO weaponDataSO { get; private set; } // 무기 데이터 (ScriptableObject)

        public void initialize(WeaponDataSO dataSO)
        {
            this.weaponDataSO = dataSO;
        }
    }
}
