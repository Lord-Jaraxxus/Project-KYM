using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KYM 
{
    public class CharacterInfoUI : UIBase
    {
        [SerializeField] private Button exitButton;

        public void Open()
        {
            gameObject.SetActive(true);
        }
        public void OnclickExitButton()
        {
            UIManager.Hide<CharacterInfoUI>(UIList.CharacterInfoUI); // UIManager¸¦ ÅëÇØ ¼û±è
            // gameObject.SetActive(false);
        }
    }
}

