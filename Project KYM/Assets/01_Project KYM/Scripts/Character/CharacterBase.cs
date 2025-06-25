using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging; // Rigging ���� ���ӽ����̽� �߰� (�ʿ��� ���)

namespace KYM
{
    public class CharacterBase : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefeb; // �Ѿ� ������
        [SerializeField] private Transform bulletSpawnPoint; // �Ѿ� �߻� ��ġ

        [SerializeField] private Rig aimingRig; // ���� Rig (�ʿ��� ���)
        [SerializeField] private Transform aimingPoint; // ���� ����Ʈ (�ʿ��� ���)

        public Vector3 AimingPoint 
        {
            set 
            {
                aimingPoint.transform.position = value; // ���� ����Ʈ ��ġ ����
            }
        }

        public bool IsWalk { get; set; } = false;
        public bool IsCrouch { get; set; } = false;
        public bool IsAiming { get; set; } = false;
        public Vector3 Aimtarget { get; set; } 
        public bool IsReloading { get; set; } = false;

        private Animator animator; // Animator ������Ʈ
        private CharacterController characterController; // CharacterController ������Ʈ

        private float walkblend;
        private float crouchblend;
        private float aimingblend;

        private Vector3 movementForward;
        private float verticalVelocity;
        private float targetRotation;
        private float rotationVelocity;
        private float roatationSmoothTime = 0.15f; // ȸ�� �ε巯�� �ð�
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

            aimingRig.weight = aimingblend; // ���� Rig�� ����ġ ���� (�ʿ��� ���)

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
            aimTarget.y = transform.position.y; // Y���� ���� ĳ���� ��ġ�� ����
            Vector3 pos = transform.position;
            Vector3 aimDirection = (aimTarget - pos).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }

        public void Shoot()
        {
            GameObject newBullet = Instantiate(bulletPrefeb);
            bulletPrefeb.gameObject.SetActive(true); // �Ѿ� ������ Ȱ��ȭ

            // newBullet.transform.position = bulletSpawnPoint.position; // �Ѿ� �߻� ��ġ ����
            // newBullet.transform.rotation = bulletSpawnPoint.rotation; // �Ѿ� �߻� ���� ����
            newBullet.transform.SetPositionAndRotation(bulletSpawnPoint.position, bulletSpawnPoint.rotation); // �Ѿ� �߻� ��ġ�� ���� ����

            //Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
            //bulletRigidbody.AddForce(bulletSpawnPoint.forward * 100f, ForceMode.Impulse); // �Ѿ˿� ���� ���Ͽ� �߻�
        }
    }
}
