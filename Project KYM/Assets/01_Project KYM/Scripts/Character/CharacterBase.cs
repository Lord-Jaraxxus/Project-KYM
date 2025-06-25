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

        // "=>" �̷��� ���� ���� Lambda(����) ǥ�����̶�� �մϴ�.
        public int MaxAmmo => maxAmmo;
        public int CurAmmo => curAmmo;
        public float MaxHP => maxHP;
        public float CurHP => curHP;
        public float MaxSp => maxSp;
        public float CurSp => curSp;

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
        public bool IsReloading { get; private set; } = false;

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

        private float fireRate = 0.3f; // �߻� �ӵ�
        private float lastFireTime = 0f; // ������ �߻� �ð�

        private int maxAmmo = 30; // �ִ� ź�� ��
        private int curAmmo = 30; // ���� ź�� ��

        private float maxHP = 1000f; // �ִ� ü��
        private float curHP = 1000f; // ���� ü��
        private float maxSp = 100f; // �ִ� ���¹̳�
        private float curSp = 100f; // ���� ���¹̳�

        public event System.Action<int, int> OnAmmoChanged; // ź�� ���� �̺�Ʈ (Callback)

        private void Awake()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();

            var reloadState = animator.GetBehaviour<ReloadStateMachineBehaviour>();
            reloadState.setCharacter(this); // ������ ���� �ӽ� ���ۿ� ĳ���� ����
        }

        private void Update()
        {
            walkblend = Mathf.Lerp(walkblend, IsWalk ? 1f : 0f, Time.deltaTime);
            crouchblend = Mathf.Lerp(crouchblend, IsCrouch ? 1f : 0f, Time.deltaTime * 10f);

            bool isAimingRigEnabled = IsAiming && !IsReloading; // ���� ���̸鼭 ������ ���� �ƴ� ���� ���� Rig Ȱ��ȭ
            aimingblend = Mathf.Lerp(aimingblend, isAimingRigEnabled ? 1f : 0f, Time.deltaTime * 10f);
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
            if(Time.time - lastFireTime > fireRate && curAmmo > 0) // �߻� �ӵ� ���� & ���� ź���� 0���� ū ���
            {
                // Time.time : ���� ����Ƽ�� �ð��� �ǹ� => ���� ����Ƽ�� �÷��� ���� 3�� �����ٸ�? => 3.0f
                GameObject newBullet = Instantiate(bulletPrefeb);
                bulletPrefeb.gameObject.SetActive(true); // �Ѿ� ������ Ȱ��ȭ
                newBullet.transform.SetPositionAndRotation(bulletSpawnPoint.position, bulletSpawnPoint.rotation); // �Ѿ� �߻� ��ġ�� ���� ����

                lastFireTime = Time.time; // ������ �߻� �ð� ������Ʈ
                curAmmo--; // ���� ź�� ����

                OnAmmoChanged?.Invoke(curAmmo, maxAmmo); // ź�� ���� �̺�Ʈ ȣ��
            }
        }

        public void Reload()
        {
            if (IsReloading) return; // �̹� ������ ���̶�� ����

            IsReloading = true; // ������ ���� ����
            animator.SetTrigger("Reload Trigger"); // ������ �ִϸ��̼� Ʈ���� ����
        }

        public void SetReloadComplete()
        {
            curAmmo = maxAmmo; // ���� ź���� �ִ� ź������ ����
            IsReloading = false; // ������ �Ϸ� ���·� ����

            OnAmmoChanged?.Invoke(curAmmo, maxAmmo); // ź�� ���� �̺�Ʈ ȣ��
        }
    }
}
