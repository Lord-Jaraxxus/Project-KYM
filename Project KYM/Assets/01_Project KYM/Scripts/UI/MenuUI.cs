using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace KYM 
{
    public class MenuUI : UIBase
    {

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))   // 메뉴창이 열려있을 때 Esc 키를 누르면
            {
                UIManager.Hide<MenuUI>(UIList.MenuUI); // 메뉴 UI 숨김
            }
        }

        private void OnEnable()
        {
            Time.timeScale = 0f; // 게임 일시 정지
        }
        private void OnDisable()
        {
            Time.timeScale = 1f; // 게임 재개
        }

        

        public void OnclickExitButton() 
        {
            Main.Singleton.SystemQuit(); // 게임 종료
        }
    }
}

