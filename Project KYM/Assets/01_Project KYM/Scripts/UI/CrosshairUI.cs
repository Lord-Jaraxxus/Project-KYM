using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class CrosshairUI : UIBase
    {
        [SerializeField] RectTransform crossTop;
        [SerializeField] RectTransform crossBottom;
        [SerializeField] RectTransform crossLeft;
        [SerializeField] RectTransform crossRight;

        public float minSpread = 40; // 최소 스프레드 거리
        public float maxSpread = 140; // 최대 스프레드 거리

        public float targetSpread;
        public float currentSpread;
        public float recoverySpread;

        public void Init(WeaponBase weaponBase) // 얘를 어디서??? 무기 낄때니까 저기 웨폰베이스에서 불러야 하나??
        {
            if (weaponBase == null) return; // 무기가 없으면 초기화 중지
            weaponBase.OnFired += OnFired; // 무기 발사 이벤트 구독
        }
        private void OnEnable()
        {
            currentSpread = targetSpread = minSpread; // 초기 스프레드 설정
        }
        void Update()
        {
            if (currentSpread > minSpread)
            {
                currentSpread -= recoverySpread * Time.deltaTime; // 스프레드 회복
                currentSpread = Mathf.Max(currentSpread, minSpread); // 최소 스프레드 이하로 내려가지 않도록
                UpdateCrosshair();
            }
        }

        void OnFired(int curAmmo)
        {
            // CrosshairUI의 이미지 업데이트
            Spread(50f); // 발사 시 스프레드 적용
        }

        void Spread(float accuracy)
        {
            float spreadAmount = (100f - accuracy) / 100f; // 정확도에 따른 스프레드 계산
            targetSpread = Mathf.Lerp(minSpread, maxSpread, spreadAmount); // 최소와 최대 스프레드 사이에서 보간
            currentSpread = Mathf.Max(currentSpread, targetSpread); // 현재 스프레드가 목표 스프레드보다 작으면 업데이트
            UpdateCrosshair(); // 크로스헤어 업데이트
        }

        private void UpdateCrosshair()
        {
            crossLeft.anchoredPosition = new Vector2(-currentSpread, 0); // 왼쪽 크로스헤어 위치 업데이트
            crossRight.anchoredPosition = new Vector2(currentSpread, 0); // 오른쪽 크로스헤어 위치 업데이트
            crossTop.anchoredPosition = new Vector2(0, currentSpread); // 위쪽 크로스헤어 위치 업데이트
            crossBottom.anchoredPosition = new Vector2(0, -currentSpread); // 아래쪽 크로스헤어 위치 업데이트
        }
    }
}
