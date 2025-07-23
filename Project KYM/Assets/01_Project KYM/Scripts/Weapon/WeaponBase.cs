using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class WeaponBase : MonoBehaviour
    {
        // "=>" 이렇게 적은 것을 Lambda(람다) 표현식이라고 합니다.
        public Vector3 LeftHandIKOffsetPosition => leftHnadIKOffsetPosition; // 왼손 IK 오프셋 위치
        public Vector3 LeftHandIKOffsetRotation => leftHandIKOffsetRotation; // 왼손 IK 오프셋 회전

        public int MaxAmmo => maxAmmo;
        public int CurAmmo => curAmmo;
        public int ReserveAmmo => reserveAmmo;

        [SerializeField] private Bullet bulletPrefeb; // 총알 프리팹
        [SerializeField] private Transform bulletSpawnPoint; // 총알 발사 위치
        [SerializeField] private Vector3 leftHnadIKOffsetPosition; // 왼손 IK 오프셋 위치
        [SerializeField] private Vector3 leftHandIKOffsetRotation; // 왼손 IK 오프셋 회전

        private float fireRate = 0.3f; // 발사 속도
        private float lastFireTime = 0f; // 마지막 발사 시간

        private int maxAmmo = 30; // 최대 탄약 수
        private int curAmmo = 30; // 현재 탄약 수
        private int reserveAmmo = 40; // 소지 탄약 수

        private CharacterBase onwerCharacter; // 총기의 소유자 캐릭터

        public void Init(CharacterBase owner, int curAmmo, int reserveAmmo)
        {
            this.onwerCharacter = owner; // 총기의 소유자 캐릭터 설정
            this.curAmmo = curAmmo; // 현재 탄약 수 설정
            this.reserveAmmo = reserveAmmo; // 소지 탄약 수 설정
        }

        public bool Fire()
        {
            if (Time.time - lastFireTime > fireRate && curAmmo > 0) // 발사 속도 제한 & 현재 탄약이 0보다 큰 경우
            {
                // Time.time : 현재 유니티의 시간을 의미 => 현재 유니티가 플레이 된지 3초 지났다면? => 3.0f
                Bullet newBullet = Instantiate(bulletPrefeb);
                bulletPrefeb.gameObject.SetActive(true); // 총알 프리팹 활성화
                newBullet.transform.SetPositionAndRotation(bulletSpawnPoint.position, bulletSpawnPoint.rotation); // 총알 발사 위치와 방향 설정
                newBullet.Initialize(onwerCharacter); // 총알의 소유자를 현재 캐릭터로 설정

                lastFireTime = Time.time; // 마지막 발사 시간 업데이트
                curAmmo--; // 현재 탄약 감소

                return true; // 발사 성공
            }
            return false; // 발사 실패 (발사 속도 제한 또는 현재 탄약이 0인 경우)
        }

        public void Reload()
        {
            int neededAmmo = maxAmmo - curAmmo; // 현재 탄약에서 필요한 탄약 계산
            int loadedAmmo = Mathf.Min(neededAmmo, reserveAmmo); // 필요한 탄약과 남은 탄약 중 최소값을 로드할 탄약으로 설정

            curAmmo += loadedAmmo; // 현재 탄약에 로드된 탄약 추가
            reserveAmmo -= loadedAmmo; // 소지 탄약에서 로드된 탄약 차감 
        }
        public void AddReserveAmmo(int amount) // 소지 탄약 추가 메서드
        {
            reserveAmmo += amount; // 소지 탄약을 amount만큼 추가 
        }
    }
}
