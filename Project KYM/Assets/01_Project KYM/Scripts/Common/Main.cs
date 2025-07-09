using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class Main : MonoBehaviour
    {
        public static Main Instance { get; private set; } = null; // 싱글톤 인스턴스
        private void Awake()
        {
                Instance = this; // 싱글톤 인스턴스 설정
                DontDestroyOnLoad(this.gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        private void Start()
        {
            UIManager.Instance.Initialize(); // UIManager 초기화
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
            UIManager.Show<TitleUI>(UIList.TitleUI); // TitleUI 표시
        }
    }
}
