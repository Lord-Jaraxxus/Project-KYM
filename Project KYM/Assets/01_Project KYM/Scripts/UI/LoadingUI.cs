using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM 
{
    public class LoadingUI : UIBase
    {
        [Header("Components")]
        [SerializeField] private UnityEngine.UI.Image loadingBar; // 로딩 바 이미지

        public void SetLoadingProgress(float progress)
        {
            progress = Mathf.Clamp01(progress); // 0과 1 사이로 제한
            loadingBar.fillAmount = progress; // 로딩 바의 fillAmount를 설정
        }
    }
}
