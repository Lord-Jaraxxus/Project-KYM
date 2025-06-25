using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging; // Rigging 관련 네임스페이스 추가 (필요한 경우)

namespace KYM
{
    public class CharacterBase : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefeb; // 총알 프리팹
        [SerializeField] private Transform bulletSpawnPoint; // 총알 발사 위치

        [SerializeField] private Rig aimingRig; // 조준 Rig (필요한 경우)
        [SerializeField] private Transform aimingPoint; // 조준 포인트 (필요한 경우)

        public Vector3 AimingPoint 
        {
            set 
            {
                aimingPoint.transform.position = value; // 조준 포인트 위치 설정
            }
        }

        public bool IsWalk { get; set; } = false;
        public bool IsCrouch { get; set; } = false;
        public bool IsAiming { get; set; } = false;
        public Vector3 Aimtarget { get; set; } 
        public bool IsReloading { get; set; } = false;

        private Animator animator; // Animator 컴포넌트
        private CharacterController characterController; // CharacterController 컴포넌트

        private float walkblend;
        private float crouchblend;
        private float aimingblend;

        private Vector3 movementForward;
        private float verticalVelocity;
        private float targetRotation;
        private float rotationVelocity;
        private float roatationSmoothTime = 0.15f; // 회전 부드러움 시간
        private float smoothHorizontal;
        private float smoothVertical;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            walkblend = Mathf.Lerp(walkblend, IsWalk ? 1f : 0f, Time.deltaTime);
            crouchblend = Mathf.Lerp(crouchblend, IsCrouch ? 1f : 0f, Time.deltaTime * 10f);
            aimingblend = Mathf.Lerp(aimingblend, IsAiming ? 1f : 0f, Time.deltaTime * 10f);

            aimingRig.weight = aimingblend; // 조준 Rig의 가중치 설정 (필요한 경우)

            animator.SetFloat("Running", walkblend);
            animator.SetFloat("Aiming", aimingblend);
            animator.SetFloat("Crouch", crouchblend);
        }
        
        public void SetMovementForward(Vector3 forward)
        {
            movementForward = forward;
        }

        public void Move(Vector2 input)
        {
            bool isInputSomething = input.magnitude > 0.1f;
            if (!IsAiming) 
            {
                if (isInputSomething) 
                {
                    Vector3 inputDir = new Vector3(input.x, 0, input.y).normalized;
                    Vector3 worldDirection = Quaternion.LookRotation(movementForward) * inputDir;
                    targetRotation = Quaternion.LookRotation(worldDirection).eulerAngles.y;

                    float roatation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, roatationSmoothTime);
                    transform.rotation = Quaternion.Euler(0, roatation, 0);
                }
            }
            
            smoothHorizontal = Mathf.Lerp(smoothHorizontal, input.x, Time.deltaTime * 10f);
            smoothVertical = Mathf.Lerp(smoothVertical, input.y, Time.deltaTime * 10f);

            animator.SetFloat("Magnitude", input.magnitude);
            animator.SetFloat("Horizontal", smoothHorizontal);
            animator.SetFloat("Vertical", smoothVertical);
        }
        public void Rotate(Vector3 targetAimPoint)
        {
            if (!IsAiming)
                return;

            Vector3 aimTarget = targetAimPoint;
            aimTarget.y = transform.position.y; // Y축은 현재 캐릭터 위치로 고정
            Vector3 pos = transform.position;
            Vector3 aimDirection = (aimTarget - pos).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }

        public void Shoot()
        {
            GameObject newBullet = Instantiate(bulletPrefeb);
            bulletPrefeb.gameObject.SetActive(true); // 총알 프리팹 활성화

            // newBullet.transform.position = bulletSpawnPoint.position; // 총알 발사 위치 설정
            // newBullet.transform.rotation = bulletSpawnPoint.rotation; // 총알 발사 방향 설정
            newBullet.transform.SetPositionAndRotation(bulletSpawnPoint.position, bulletSpawnPoint.rotation); // 총알 발사 위치와 방향 설정

            //Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
            //bulletRigidbody.AddForce(bulletSpawnPoint.forward * 100f, ForceMode.Impulse); // 총알에 힘을 가하여 발사
        }
    }
}
