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
        private void Awake()
        {
            UIManager.Hide<MenuUI>(UIList.MenuUI); // UIManager∏¶ ≈Î«ÿ º˚±Ë
            // gameObject.SetActive(false); // Ω√¿€ Ω√ ≤®µ“
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))   // ∏ﬁ¥∫√¢¿Ã ø≠∑¡¿÷¿ª ∂ß Esc ≈∞∏¶ ¥©∏£∏È
            {
                UIManager.Hide<MenuUI>(UIList.MenuUI); // ∏ﬁ¥∫ UI º˚±Ë
            }
        }

        private void OnEnable()
        {
            Time.timeScale = 0f; // ∞‘¿” ¿œΩ√ ¡§¡ˆ
        }
        private void OnDisable()
        {
            Time.timeScale = 1f; // ∞‘¿” ¿Á∞≥
        }

        

        public void OnclickExitButton() 
        {
            var loadingUI = UIManager.Show<LoadingUI>(UIList.LoadingUI);
            loadingUI.ShowLoadingUI(() =>
            {
                UIManager.Hide<PlayerHUD>(UIList.PlayerHUD); // PlayerHUD º˚±Ë
                UIManager.Hide<CharacterInfoUI>(UIList.CharacterInfoUI); // CharacterInfoUI º˚±Ë
                UIManager.Hide<MenuUI>(UIList.MenuUI); // MenuUI º˚±Ë
                UnityEngine.SceneManagement.SceneManager.LoadScene("Title"); // "Title" æ¿¿∏∑Œ ¿¸»Ø
                UIManager.Show<TitleUI>(UIList.TitleUI);
            });
        }
    }
}

