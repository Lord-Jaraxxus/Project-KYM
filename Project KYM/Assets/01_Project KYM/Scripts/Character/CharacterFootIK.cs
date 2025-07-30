using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class CharacterFootIK : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayer; // 지면 레이어 마스크
        [SerializeField] private float rayDistance = 0.5f; // 발 위치를 결정하기 위한 레이캐스트 거리
        [SerializeField] private Vector3 footBackOffset = new Vector3(0, 0, -0.1f); // 발 뒤꿈치 오프셋
        [SerializeField] private Vector3 footMidOffset = new Vector3(0, 0, 0.0f); // 발 위치 오프셋

        private Animator animator;
        private Transform leftFoot;
        private Transform rightFoot;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            float leftWeight = animator.GetFloat("LeftFootIKWeight");
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftWeight);
            HandleFootIK(AvatarIKGoal.LeftFoot, leftFoot);

            float rightWeight = animator.GetFloat("RightFootIKWeight");
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightWeight);
            HandleFootIK(AvatarIKGoal.RightFoot, rightFoot);
        }

        private void HandleFootIK(AvatarIKGoal footGoal, Transform footTransform)
        {
            Vector3 originBack = footTransform.position + footTransform.rotation * footBackOffset + Vector3.up * 0.2f;
            Vector3 orginMid = footTransform.position + footTransform.rotation * footMidOffset + Vector3.up * 0.2f; 

            bool isHitBack = Physics.Raycast(originBack, Vector3.down, out RaycastHit hitInfoBack, rayDistance, groundLayer);
            bool isHitMid = Physics.Raycast(orginMid, Vector3.down, out RaycastHit hitInfoMid, rayDistance, groundLayer);

            if (isHitBack && isHitMid) 
            {
                Vector3 avaragePosition = (hitInfoBack.point + hitInfoMid.point) * 0.5f;
                Vector3 footNormal = (hitInfoBack.normal + hitInfoMid.normal).normalized;

                Quaternion footRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, footNormal), footNormal);

                animator.SetIKPosition(footGoal, avaragePosition);
                animator.SetIKRotation(footGoal, footRotation);
            }
        }
    }
}
