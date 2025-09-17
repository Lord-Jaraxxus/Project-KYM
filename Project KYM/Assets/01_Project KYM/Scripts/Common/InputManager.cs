using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class InputManager : SingletonBase<InputManager>
    {
        public Vector2 InputMove { get; private set; } // 이동 입력 벡터 (수평, 수직)
        public Vector2 InputLook { get; private set; } // 마우스 이동 입력 벡터 (수평, 수직)
        public float inputMouseScroll { get; private set; } // 마우스 스크롤 입력 값

        public event System.Action OnInputLmc; // 좌클릭 입력 이벤트
        public event System.Action OnInputRmcUp; // 우클릭 입력 이벤트
        public event System.Action OnInputRmcDown; // 우클릭 입력 이벤트 (조준 모드 활성화)
        public event System.Action OnInputReload; // 리로드 입력 이벤트
        public event System.Action OnInputCrouch; // 앉기 입력 이벤트
        public event System.Action OnInputSprintDown; // 달리기 입력 이벤트
        public event System.Action OnInputSprintUp; // 달리기 중지 입력 이벤트
        public event System.Action OnInputSave; // 저장 입력 이벤트
        public event System.Action OnInputInteract; // 상호작용 입력 이벤트
        public event System.Action OnInputPrimaryWeapon; // 1번 무기 변경 입력 이벤트
        public event System.Action OnInputSecondaryWeapon; // 2번 무기 변경 입력 이벤트

        public bool IsForceCursorVisible
        {
            get => IsForceCursorVisible;
            set
            {
                IsForceCursorVisible = value; // 커서 강제 표시 여부 설정
                if (IsForceCursorVisible)
                {
                    SetCursorVisible(true); // 커서 강제 표시가 true면 커서 보이기
                }
                else
                {
                    SetCursorVisible(false); // 커서 강제 표시가 false면 커서 숨김
                }
            }
        }
        private bool isForceCursorVisible = false; // 커서 강제 표시 여부 (기본값은 false)

        public void SetCursorVisible(bool isVisible)
        {
            if (isVisible) 
            {
                Cursor.visible = true;  // 커서 보이기
                Cursor.lockState = CursorLockMode.None; // 커서 잠금 해제
            }
            else 
            {
                Cursor.visible = false; // 커서 숨김
                Cursor.lockState = CursorLockMode.Locked; // 커서 잠금 상태 설정
            }
        }

        private void Update()
        {
            if (!isForceCursorVisible) 
            {
                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    SetCursorVisible(true); // Alt 키를 누르고 있으면 커서 보이기
                }
                else
                {
                    SetCursorVisible(false); // Alt 키를 떼면 커서 숨김
                }
            }

            InputMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // 이동 입력 벡터 설정
            InputLook = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // 마우스 이동 입력 벡터 설정
            inputMouseScroll = Input.GetAxis("Mouse ScrollWheel"); // 마우스 스크롤 입력 값 설정

            if (Input.GetMouseButton(0)) // 마우스 좌클릭이 눌러져있으면 계속 true
            {
                OnInputLmc?.Invoke(); // 좌클릭 입력 이벤트 발생
            }

            if (Input.GetMouseButtonDown(1))  // 마우스 오른쪽 버튼 클릭 시 조준 모드 활성화
            {
                OnInputRmcDown?.Invoke(); // 우클릭 입력 이벤트 발생
            }

            if (Input.GetMouseButtonUp(1))
            {
                OnInputRmcUp?.Invoke(); // 우클릭 입력 이벤트 발생 (조준 모드 비활성화)
            }

            if (Input.GetKeyDown(KeyCode.R)) // R 키를 눌렀을 때
            {
                OnInputReload?.Invoke(); // 리로드 입력 이벤트 발생
            }

            if (Input.GetKeyDown(KeyCode.LeftControl)) // 왼쪽 Ctrl 키를 눌렀을 때
            {
                OnInputCrouch?.Invoke(); // 앉기 입력 이벤트 발생
            }

            if (Input.GetKeyDown(KeyCode.LeftShift)) // 왼쪽 Shift 키를 누르고 있으면 달리기
            {
                OnInputSprintDown?.Invoke(); // 달리기 입력 이벤트 발생
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                OnInputSprintUp?.Invoke(); // 달리기 중지 입력 이벤트 발생
            }

            if (Input.GetKeyDown(KeyCode.F9)) // Save
            {
                OnInputSave?.Invoke(); // 저장 입력 이벤트 발생
            }

            if(Input.GetKeyDown(KeyCode.E)) // E 키를 눌렀을 때
            {
                OnInputInteract?.Invoke(); // 상호작용 입력 이벤트 발생
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) 
            {
                OnInputPrimaryWeapon?.Invoke(); // 1번 무기 변경 입력 이벤트 발생
            }

            if (Input.GetKeyDown(KeyCode.Alpha2)) 
            {
                OnInputSecondaryWeapon?.Invoke(); // 2번 무기 변경 입력 이벤트 발생
            }
        }
    }
}
