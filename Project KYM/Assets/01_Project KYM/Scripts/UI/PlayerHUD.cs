using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KYM
{
    public class PlayerHUD : UIBase
    {
        [SerializeField] private TextMeshProUGUI ammoText;
        [SerializeField] private TextMeshProUGUI reserveAmmoText;
        [SerializeField] private TextMeshProUGUI hpText; // 체력 텍스트 UI 
        [SerializeField] private TextMeshProUGUI spText; // 스태미너 텍스트 UI
        [SerializeField] private Image hpBar; // 체력 바 UI
        [SerializeField] private Image spBar; // 스태미너 바 UI 

        [SerializeField] private CharacterInfoUI charaInfoUI;
        [SerializeField] private Button charaInfoButton;



        public void OnclickGoToTitleButton()  
        {
            Main.Singleton.ChangeScene(SceneType.Title); // 타이틀 씬으로 변경
        }

        public void OnclickInfoButton()
        {
            UIManager.Show<CharacterInfoUI>(UIList.CharacterInfoUI); // CharacterInfoUI 표시
        }

        public void OnclickMenuButton() 
        {
            UIManager.Show<MenuUI>(UIList.MenuUI); // MenuUI 표시
        }



        public void RefreshAmmoText(int curAmmo, int maxAmmo, int reserveAmmo)
        {
            ammoText.text = $"{curAmmo} / {maxAmmo}"; // 현재 탄약과 최대 탄약을 텍스트로 표시
            reserveAmmoText.text = $"{reserveAmmo}"; // 소지 탄약을 텍스트로 표시
        }
        public void RefreshHpUI(float CurrentHp, float maxHp)
        {
            hpText.text = $"{CurrentHp} / {maxHp}"; // 체력 텍스트 초기화 
            hpBar.fillAmount = CurrentHp / maxHp; // 체력 바 초기화 
        }
        public void RefreshSpUI(float currentSp, float MaxSp)
        {
            spText.text = $"{currentSp} / {MaxSp}"; // 스태미너 텍스트 갱신
            spBar.fillAmount = currentSp / MaxSp; // 스태미너 바 갱신
        }

    }
}


