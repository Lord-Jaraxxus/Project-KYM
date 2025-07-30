using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class CrosshairUI : UIBase
    {
        WeaponBase linkedWeapon; // 무기와 연결된 변수

        [SerializeField] RectTransform crossTop;
        [SerializeField] RectTransform crossBottom;
        [SerializeField] RectTransform crossLeft;
        [SerializeField] RectTransform crossRight;

        private Vector2 topDefaultPos;
        private Vector2 bottomDefaultPos;
        private Vector2 leftDefaultPos;
        private Vector2 rightDefaultPos;

        [SerializeField] private float spreadDistance = 5f; // 스프레드 값 (얼마나 퍼지는지)
        [SerializeField] private float recoverySpeed = 20f; // 퍼졌다가 돌아오는 속도

        bool isSpread = false; // 스프레드 상태 여부

        private void Start()
        {
            // 네모들 원래 위치 저장
            topDefaultPos = crossTop.anchoredPosition;
            bottomDefaultPos = crossBottom.anchoredPosition;
            leftDefaultPos = crossLeft.anchoredPosition;
            rightDefaultPos = crossRight.anchoredPosition;
        }

        void Update()
        {
            // 여기서 벌어진 걸 다시 천천히 되돌리는..
            if (!isSpread) return;

            crossTop.anchoredPosition = Vector2.MoveTowards(crossTop.anchoredPosition, topDefaultPos, recoverySpeed * Time.deltaTime);
            crossBottom.anchoredPosition = Vector2.MoveTowards(crossBottom.anchoredPosition, bottomDefaultPos, recoverySpeed * Time.deltaTime);
            crossLeft.anchoredPosition = Vector2.MoveTowards(crossLeft.anchoredPosition, leftDefaultPos, recoverySpeed * Time.deltaTime);
            crossRight.anchoredPosition = Vector2.MoveTowards(crossRight.anchoredPosition, rightDefaultPos, recoverySpeed * Time.deltaTime);

            if (IsCrosshairAtDefault()) 
            {
                isSpread = false; // 네모들이 원래 위치로 돌아오면 스프레드 상태 해제
            }
        }

        public void Init(WeaponBase weaponBase) // 얘를 어디서??? 무기 낄때니까 저기 웨폰베이스에서 불러야 하나??
        {
            if (weaponBase == null) return; // 무기가 없으면 초기화 중지
            weaponBase.OnFired += OnFired; // 무기 발사 이벤트 구독
        }

        void OnFired(int curAmmo)
        {
            // CrosshairUI의 이미지 업데이트
            Spread(); // 발사 시 스프레드 적용
            isSpread = true; // 스프레드 상태로 변경

            Debug.Log($"Current Ammo: {curAmmo}");  // 나중에 쓸 일이? 있을수도?
        }

        void Spread() 
        {
            crossTop.anchoredPosition = topDefaultPos + Vector2.up * spreadDistance;
            crossBottom.anchoredPosition = bottomDefaultPos + Vector2.down * spreadDistance;
            crossLeft.anchoredPosition = leftDefaultPos + Vector2.left * spreadDistance;
            crossRight.anchoredPosition = rightDefaultPos + Vector2.right * spreadDistance;
        }

        private bool IsCrosshairAtDefault()
        {
            return crossTop.anchoredPosition == topDefaultPos &&
                   crossBottom.anchoredPosition == bottomDefaultPos &&
                   crossLeft.anchoredPosition == leftDefaultPos &&
                   crossRight.anchoredPosition == rightDefaultPos;
        }
    }
}
