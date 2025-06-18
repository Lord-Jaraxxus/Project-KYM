using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField] public Transform CinemachineCameraTarget { get; private set; }
        public BulletSpawner bulletSpawner; // �Ѿ� ������ ��ũ��Ʈ ����

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

        private void Update()
        {
            if (linkedCharacter == null) { return; }

            Vector2 inputMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            bool isAim = Input.GetMouseButton(1); // ���콺 ������ ��ư Ŭ�� �� ���� ��� Ȱ��ȭ

            if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư Ŭ�� �� �Ѿ� �߻� 
            {
                bulletSpawner.SpawnBullet(); // �Ѿ� �����⿡�� �Ѿ� ���� �޼ҵ� ȣ��
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
    }
}
