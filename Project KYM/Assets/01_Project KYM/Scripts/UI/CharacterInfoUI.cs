using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KYM 
{
    public class CharacterInfoUI : UIBase
    {
        [SerializeField] private Button exitButton;
        private void Awake()
        {
            gameObject.SetActive(false); // Ω√¿€ Ω√ ≤®µ“
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }
        public void OnclickExitButton()
        {
            UIManager.Hide<CharacterInfoUI>(UIList.CharacterInfoUI); // UIManager∏¶ ≈Î«ÿ º˚±Ë
            // gameObject.SetActive(false);
        }
    }
}

