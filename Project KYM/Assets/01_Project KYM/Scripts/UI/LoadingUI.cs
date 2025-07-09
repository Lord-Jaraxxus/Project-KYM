using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM 
{
    public class LoadingUI : UIBase
    {
        [Header("Components")]
        [SerializeField] private CanvasGroup group; // 캔버스 그룹 컴포넌트
        [SerializeField] private GameObject loadingPanel; // 로딩 패널 게임 오브젝트
        [SerializeField] private UnityEngine.UI.Image loadingBar; // 로딩 바 이미지

        [Header("Settings")]
        [SerializeField] private float fadeSpeed = 2f; // 페이드 속도

        private bool isShowing = false; // 로딩 UI가 보이는지 여부
        private bool isHiding = false; // 로딩 UI가 숨겨지는지 여부
        private bool isTaskExecuted = false; // 로딩 작업이 실행되었는지 여부

        private System.Action OnLoadingTask = null; // 로딩 작업을 실행할 델리게이트   

        public void Update()
        {
            if (isShowing) 
            {
                group.alpha += Time.unscaledDeltaTime * fadeSpeed; // 알파값 증가
                if (group.alpha >= 1f) 
                {
                    group.alpha = 1f; // 알파값을 1로 제한
                    isShowing = false; // 로딩 UI 표시 완료

                    if (!isTaskExecuted) 
                    {
                        StartCoroutine(ExecuteTask()); // 로딩 작업 실행
                    } 
                }
            } 
            else if (isHiding) 
            {
                group.alpha -= Time.unscaledDeltaTime * fadeSpeed; // 알파값 감소
                if (group.alpha <= 0f) 
                {
                    group.alpha = 0f; // 알파값을 0으로 제한
                    isHiding = false; // 로딩 UI 숨김 완료
                    
                    UIManager.Hide<LoadingUI>(UIList.LoadingUI); // 로딩 UI 숨김
                }
            }
        }

        IEnumerator ExecuteTask() 
        {
            loadingPanel.SetActive(true); // 로딩 패널 활성화
            yield return new WaitForEndOfFrame(); // 다음 프레임까지 대기
            isTaskExecuted = true; // 로딩 작업이 실행되었음을 표시
            OnLoadingTask?.Invoke(); // 로딩 작업 실행

            HideloadingUI(); // 로딩 UI 숨김
        }

        public void ShowLoadingUI(System.Action task)
        {
            group.alpha = 0f; // 초기 알파값 설정
            loadingPanel.SetActive(false); // 로딩 패널 비활성화
            isShowing = true; // 로딩 UI 표시 상태로 설정
            isTaskExecuted = false; // 로딩 작업이 아직 실행되지 않았음을 표시
            isHiding = false; // 로딩 UI 숨김 상태 해제

            OnLoadingTask = task;
        }
        public void HideloadingUI()
        {
            loadingPanel.SetActive(false);
            isHiding = true; // 로딩 UI 숨김 상태로 설정
            isShowing = false; // 로딩 UI 표시 상태 해제
        }

        public void SetLoadingProgress(float progress)
        {
            progress = Mathf.Clamp01(progress); // 0과 1 사이로 제한
            loadingBar.fillAmount = progress; // 로딩 바의 fillAmount를 설정
        }

    }
}
