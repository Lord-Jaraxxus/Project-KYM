using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KYM
{
    public class GameOverUI : UIBase // 게임 오버 UI 클래스
    {
        [SerializeField] private Button retryButton;
        
        void Start()
        {
            retryButton.onClick.AddListener(OnClickRetryButton); // 재시작 버튼 클릭 이벤트 리스너 등록
        }

        public void OnClickRetryButton()
        {
            Main.Singleton.ChangeScene(SceneType.Ingame); // 인게임 씬 리로드, 되나?
            Debug.Log("Retry button clicked, reloading Ingame scene.");

            UIManager.Hide<GameOverUI>(UIList.GameOverUI); // 게임 오버 UI 숨김
        }
    }
}
