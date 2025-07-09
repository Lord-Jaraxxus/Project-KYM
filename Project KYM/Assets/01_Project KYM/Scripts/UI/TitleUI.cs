using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM 
{
    public class TitleUI : UIBase
    {
        public void OnClickStartButton() 
        {
            var loadingUI = UIManager.Show<LoadingUI>(UIList.LoadingUI); // LoadingUI 표시
            loadingUI.ShowLoadingUI(() =>
            {
                UIManager.Hide<TitleUI>(UIList.TitleUI); // TitleUI 숨김
                UnityEngine.SceneManagement.SceneManager.LoadScene("Ingame"); // "Ingame"으로 씬 전환
                UIManager.Show<PlayerHUD>(UIList.PlayerHUD); // PlayerHUD 표시
            });
        }
        public void OnClickQuitButton() 
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 플레이 모드 종료
#else
            Application.Quit(); // 애플리케이션 종료
#endif
        }
    }
}
