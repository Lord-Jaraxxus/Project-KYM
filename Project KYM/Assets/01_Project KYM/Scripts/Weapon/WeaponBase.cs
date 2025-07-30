using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KYM
{
    public class WeaponBase : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red; // Gizmos 색상 설정
            Gizmos.DrawLine(bulletSpawnPoint.position, bulletSpawnPoint.position + bulletSpawnPoint.forward * 100f); // 총알 발사 위치에서 앞으로 100 단위 길이의 선 그리기
            
            Gizmos.color = Color.yellow; // Gizmos 색상 변경
            Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * 100f); // 카메라 위치에서 앞으로 100 단위 길이의 선 그리기
        }

        public int MaxAmmo => maxAmmo;
        public int CurAmmo => curAmmo;
        public int ReserveAmmo => reserveAmmo;

        [SerializeField] private Bullet bulletPrefeb; // 총알 프리팹
        [SerializeField] private Transform bulletSpawnPoint; // 총알 발사 위치


        private float fireRate = 0f; // 발사 속도
        private float lastFireTime = 0f; // 마지막 발사 시간

        private int maxAmmo = 0; // 최대 탄약 수
        private int curAmmo = 30; // 현재 탄약 수
        private int reserveAmmo = 40; // 소지 탄약 수

        private CharacterBase onwerCharacter; // 총기의 소유자 캐릭터

        public event System.Action<int> OnFired; // 발사 이벤트 (Callback) 
        private CrosshairUI crosshairUI; // 크로스헤어 UI

        public void Init(CharacterBase owner, int curAmmo, int reserveAmmo)
        {
            this.onwerCharacter = owner; // 총기의 소유자 캐릭터 설정
            this.curAmmo = curAmmo; // 현재 탄약 수 설정
            this.reserveAmmo = reserveAmmo; // 소지 탄약 수 설정

            this.maxAmmo = GameDataModel.Singleton.WeaponDataDto.weaponDataSO.MaxAmmo; // 최대 탄약 수 설정
            this.fireRate = GameDataModel.Singleton.WeaponDataDto.weaponDataSO.FireRate; // 발사 속도 설정
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

                OnFired?.Invoke(curAmmo); // 발사 이벤트 호출 (현재 탄약 수 전달)

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
