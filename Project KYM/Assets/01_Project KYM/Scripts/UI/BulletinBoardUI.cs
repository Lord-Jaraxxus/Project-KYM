using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KYM
{
    public class BulletinBoardUI : UIBase
    {
        [SerializeField] private Button exitButton;

        private void Awake()
        {
            exitButton.onClick.AddListener(OnclickExitButton); // 버튼 클릭 이벤트에 메서드 등록
        }
        
        public void OnclickExitButton() 
        {
            GameEventListener.Instance.OnReceiveEvent("CloseBulletinBoardUI"); // UI가 닫혔음을 알림
            UIManager.Hide<BulletinBoardUI>(UIList.BulletinBoardUI); // UIManager를 통해 숨김
        }


    }
}
