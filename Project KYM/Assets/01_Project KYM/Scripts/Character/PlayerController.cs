using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KYM
{
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField] public Transform CinemachineCameraTarget { get; private set; }

        [SerializeField] private TextMeshProUGUI ammoText;
        [SerializeField] private TextMeshProUGUI hpText; // ü�� �ؽ�Ʈ UI (�߰�)
        [SerializeField] private TextMeshProUGUI spText; // ���¹̳� �ؽ�Ʈ UI (�߰�)
        [SerializeField] private Image hpBar; // ü�� �� UI (�߰�)
        [SerializeField] private Image spBar; // ���¹̳� �� UI (�߰�)

        private CharacterBase linkedCharacter;
        private Camera mainCamera;

        private void Awake()
        {
            linkedCharacter = GetComponent<CharacterBase>();
            mainCamera = Camera.main;
        }

        private float cameraThreshold = 0.1f; // ī�޶� ȸ�� �Ӱ谪
        private float cinemachineTargetYaw;
        private float cinemachineTargetPitch;
        private float cameraTopClamp = 85.0f; // ī�޶� ��� ȸ�� ����
        private float camereaBottomClamp = -85.0f; // ī�޶� �ϴ� ȸ�� ����


        private void Start()
        {
            Cursor.visible = false; // Ŀ�� ����
            Cursor.lockState = CursorLockMode.Locked; // Ŀ�� ��� ���� ����

            ammoText.text = $"{linkedCharacter.CurAmmo} / {linkedCharacter.MaxAmmo}";
            hpText.text = $"{linkedCharacter.CurHP} / {linkedCharacter.MaxHP}"; // ü�� �ؽ�Ʈ �ʱ�ȭ (�߰�)
            spText.text = $"{linkedCharacter.CurSp} / {linkedCharacter.MaxSp}"; // ���¹̳� �ؽ�Ʈ �ʱ�ȭ (�߰�)
            hpBar.fillAmount = linkedCharacter.CurHP / linkedCharacter.MaxHP; // ü�� �� �ʱ�ȭ (�߰�)
            spBar.fillAmount = linkedCharacter.CurSp / linkedCharacter.MaxSp; // ���¹̳� �� �ʱ�ȭ (�߰�)

            linkedCharacter.OnAmmoChanged += RefreshAmmoText; // ź�� ���� �̺�Ʈ ����
        }

        private void Update()
        {
            if (linkedCharacter == null) { return; }

            Vector2 inputMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            bool isAim = Input.GetMouseButton(1); // ���콺 ������ ��ư Ŭ�� �� ���� ��� Ȱ��ȭ

            if (Input.GetMouseButton(0)) // ���콺 ��Ŭ���� ������������ ��� true
            {
                linkedCharacter.Shoot();
            }

            if (Input.GetKeyDown(KeyCode.R)) // R Ű�� ������ ��
            {
                linkedCharacter.Reload(); // ������ ó��
            }

            if (Input.GetKeyDown(KeyCode.LeftControl)) // ���� Ctrl Ű�� ������ ��
            {
                linkedCharacter.IsCrouch = !linkedCharacter.IsCrouch; // ũ�Ѹ� ���� ���
            }

            linkedCharacter.IsWalk = Input.GetKey(KeyCode.LeftShift); // ���� Shift Ű�� ������ �ȱ� ��� Ȱ��ȭ
            linkedCharacter.IsAiming = isAim; // ���� ��� ����
            linkedCharacter.SetMovementForward(mainCamera.transform.forward); // ī�޶��� ���� ������ ����
            linkedCharacter.Move(inputMove); // ĳ���� �̵� ó��
            linkedCharacter.Rotate(CameraSystem.Instance.AimingPoint); // ī�޶� �ý��ۿ��� ���� ������ ������ ȸ�� ó��
            linkedCharacter.AimingPoint = CameraSystem.Instance.AimingPoint; // ĳ������ ���� ������ ī�޶� �ý��ۿ��� ������
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        void CameraRotation()
        {
            Vector2 inputLook = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            if (inputLook.sqrMagnitude >= cameraThreshold) // ī�޶� ȸ�� �Ӱ谪 üũ
            {
                float yaw = inputLook.x;
                float pitch = inputLook.y;

                cinemachineTargetYaw += inputLook.x;
                cinemachineTargetPitch -= inputLook.y;
            }

            cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, camereaBottomClamp, cameraTopClamp); // ī�޶� ���� ȸ�� ����

            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch, cinemachineTargetYaw, 0f);
        }

        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) angle += 360f;
            if (angle > 360f) angle -= 360f;

            return Mathf.Clamp(angle, min, max);
        }

        void RefreshAmmoText(int curAmmo, int maxAmmo)
        {
            ammoText.text = $"{curAmmo} / {maxAmmo}"; // ���� ź��� �ִ� ź���� �ؽ�Ʈ�� ǥ��
        }
    }
}
