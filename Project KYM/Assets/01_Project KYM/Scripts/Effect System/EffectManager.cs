using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    [System.Serializable]
    public class EffectData
    {
        public string key; // 이펙트 키
        public float lifeTime = 5f; // 이펙트 생명 주기
        public GameObject prefab; // 이펙트 프리팹
    }

    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance { get; private set; }

        private void Awake() => Instance = this;

        private void OnDestroy() => Instance = null;

        [SerializeField] private List<EffectData> effectList = new List<EffectData>(); // 이펙트 데이터 리스트

        public void SpawnEffect(string key, Vector3 position, Quaternion rotation)
        {
            var targetEffectData = effectList.Find(x => x.key.Equals(key));
            if (targetEffectData == null)
                return; // 해당 키를 가진 이펙트가 없으면 종료

            var newEffect = Instantiate(targetEffectData.prefab, position, rotation); // 이펙트 프리팹을 인스턴스화
            Destroy(newEffect.gameObject, targetEffectData.lifeTime);
        }
    }
}
