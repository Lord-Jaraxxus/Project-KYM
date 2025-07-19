using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KYM
{
    public enum SceneType // 씬 타입을 정의하는 열거형
    {
        None,
        Title, // 타이틀 씬
        Ingame, // 게임 씬
    }

    public class Main : SingletonBase<Main> // Main 클래스는 SingletonBase를 상속받아 싱글톤 패턴을 구현
    {
        private bool isInitialized = false; // 초기화 여부를 나타내는 변수
        private SceneType currentSceneType = SceneType.None; // 현재 씬 타입을 저장하는 변수
        private void Start()
        {
            Initailize(); // 초기화 메서드 호출
        }

        public void Initailize()
        {
            if (isInitialized)
                return;

            UIManager.Singleton.Initialize(); // UIManager 초기화
            GameDataModel.Singleton.Initialize(); // GameDataModel 초기화
            UserDataModel.Singleton.Initialize(); // UserDataModel 초기화

#if UNITY_EDITOR
            UnityEngine.SceneManagement.Scene activeScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene(); // 현재 활성화된 씬 가져오기

            if (activeScene.name == "Main")
            {
                ChangeScene(SceneType.Title);
            }

#else
                ChangeScene(SceneType.Title);
#endif
            isInitialized = true; // 초기화 완료 플래그 설정
        }

        public void ChangeScene(SceneType sceneType, System.Action sceneLoadAfterCallback = null) // 씬 변경 메서드
        {
            if (currentSceneType == sceneType) // 현재 씬 타입과 변경하려는 씬 타입이 같으면 아무 작업도 하지 않음
                return;

            Time.timeScale = 1f; // 시간 스케일 초기화

            switch (sceneType)
            {
                case SceneType.Title: // 타이틀 씬으로 변경
                    ChangeScene<TitleScene>(sceneType, sceneLoadAfterCallback); // TitleScene 씬으로 변경
                    break;
                case SceneType.Ingame: // 게임 씬으로 변경
                    ChangeScene<IngameScene>(sceneType, sceneLoadAfterCallback); // IngameScene 씬으로 변경
                    break;
                default:
                    throw new System.NotImplementedException($"SceneType {sceneType} is not implemented.");
            }
        }

        private void ChangeScene<T>(SceneType sceneType, System.Action sceneLoadAfterCallback = null) where T : SceneBase // 제네릭 씬 변경 메서드
        {
            StartCoroutine(ChangeSceneAsync<T>(sceneType, sceneLoadAfterCallback)); // 비동기 씬 변경 코루틴 시작
        }

        private SceneBase sceneInstance; // 현재 씬 인스턴스를 저장하는 변수

        private IEnumerator ChangeSceneAsync<T>(SceneType sceneType, System.Action sceneLoadAfterCallback = null) where T : SceneBase
        {
            var loadingUI = UIManager.Show<LoadingUI>(UIList.LoadingUI); // 로딩 UI 표시
            loadingUI.SetLoadingProgress(0f); // 로딩 진행률 초기화

            yield return null; // 다음 프레임까지 대기

            UIManager.Singleton.HideAll(); // 모든 UI 숨김

            if (sceneInstance)
            {
                yield return StartCoroutine(sceneInstance.OnEnd()); // 현재 씬 종료 처리
                Destroy(sceneInstance.gameObject); // 현재 씬 오브젝트 파괴
                sceneInstance = null; // 씬 인스턴스 초기화
            }

            loadingUI.SetLoadingProgress(0.25f); // 로딩 진행률 업데이트
            yield return null; // 다음 프레임까지 대기

            var async = SceneManager.LoadSceneAsync("Empty", LoadSceneMode.Single); // 빈 씬 로드 시작
            while (!async.isDone) // 씬 로드가 완료될 때까지 대기
            {
                yield return null; // 다음 프레임까지 대기
            }

            loadingUI.SetLoadingProgress(0.5f); // 로딩 진행률 업데이트
            yield return null;

            GameObject sceneInstanceGO = new GameObject(typeof(T).Name); // 새로운 씬 인스턴스 오브젝트 생성
            sceneInstanceGO.transform.SetParent(transform); // 현재 Main 오브젝트의 자식으로 설정
            sceneInstance = sceneInstanceGO.AddComponent<T>(); // 씬 인스턴스 컴포넌트 추가
            currentSceneType = sceneType; // 현재 씬 타입 업데이트

            yield return StartCoroutine(sceneInstance.OnStart()); // 씬 시작 처리
            loadingUI.SetLoadingProgress(1f); // 로딩 진행률 업데이트
            yield return null; // 다음 프레임까지 대기

            UIManager.Hide<LoadingUI>(UIList.LoadingUI); // 로딩 UI 숨김
            sceneLoadAfterCallback?.Invoke(); // 씬 로드 후 콜백 호출
        }
        public void SystemQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 플레이 모드 종료
#else
            Application.Quit(); // 빌드된 애플리케이션 종료
#endif
        }
    }
}
