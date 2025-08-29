using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    enum MinimapType
    {
        Normal, // 일반 미니맵
        Fixed, // 고정 미니맵
    }

    public class MinimapCamera : MonoBehaviour
    {
        [SerializeField] private Camera MainCamera; // 메인 카메라
        [SerializeField] private Camera MinimapCam; // 미니맵 카메라

        private MinimapType currentMinimapType = MinimapType.Normal; // 현재 미니맵 타입
        
        void LateUpdate()
        {
            switch(currentMinimapType) // 현재 미니맵 타입에 따라
            {
                case MinimapType.Normal: // 일반 미니맵일 경우
                    MinimapCam.transform.rotation = Quaternion.Euler(90f, MainCamera.transform.eulerAngles.y, 0f); // 메인 카메라의 Y축 회전값을 따라가기
                    break;

                case MinimapType.Fixed: // 고정 미니맵일 경우
                    MinimapCam.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // 항상 위쪽 바라보기
                    break;
            }
        }

        public void SwitchType() 
        {
            switch(currentMinimapType) // 현재 미니맵 타입에 따라
            {
                case MinimapType.Normal: // 일반 미니맵일 경우
                    currentMinimapType = MinimapType.Fixed; // 고정 미니맵으로 변경
                    break;

                case MinimapType.Fixed: // 고정 미니맵일 경우
                    currentMinimapType = MinimapType.Normal; // 일반 미니맵으로 변경
                    break;
            }
            Debug.Log($"Minimap Type Changed: {currentMinimapType}"); // 변경된 미니맵 타입 로그 출력
        }
    }
}
