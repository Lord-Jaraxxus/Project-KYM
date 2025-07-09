using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KYM 
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; } = null; // 싱글톤 인스턴스

        public static T Show<T>(UIList uiName) where T : UIBase
        {
            var newUI = Instance.GetUI<T>(uiName); // UIManager에서 UI를 가져옴
            if (newUI == null) return null; // UI가 없으면 null 반환

            newUI.Show(); // UI를 보여줌
            return newUI; // 보여준 UI 반환
        }
        public static T Hide<T>(UIList uiName) where T : UIBase
        {
            var targetUI = Instance.GetUI<T>(uiName); // UIManager에서 UI를 가져옴
            if (!targetUI) return null; // UI가 없으면 null 반환

            targetUI.Hide(); // UI를 숨김
            return targetUI;
        }

        private void Awake()
        {
            Instance = this; // 싱글톤 인스턴스 설정
            DontDestroyOnLoad(this.gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }

        private void OnDestroy()
        {
            Instance = null; // 인스턴스 초기화
        }

        private Transform panelRoot; // 패널 UI를 담을 루트 트랜스폼
        private Transform popupRoot; // 팝업 UI를 담을 루트 트랜스폼
        private Dictionary<UIList, UIBase> panels = new Dictionary<UIList, UIBase>(); // 패널 UI를 저장할 딕셔너리
        private Dictionary<UIList, UIBase> popups = new Dictionary<UIList, UIBase>(); // 팝업 UI를 저장할 딕셔너리

        public void Initialize()
        {
            if ((panelRoot == null))
            {
                GameObject goPanelRoot = new GameObject("Panel Root"); // 패널 루트 생성
                panelRoot = goPanelRoot.transform; // 트랜스폼 설정
                panelRoot.parent = this.transform; // UIManager의 자식으로 설정
                panelRoot.localPosition = Vector3.zero; // 위치 초기화
                panelRoot.localRotation = Quaternion.identity;
                panelRoot.localScale = Vector3.one; // 스케일 초기화
            }

            if (popupRoot == null)
            {
                GameObject goPopupRoot = new GameObject("Popup Root"); // 팝업 루트 생성
                popupRoot = goPopupRoot.transform; // 트랜스폼 설정
                popupRoot.parent = this.transform; // UIManager의 자식으로 설정
                popupRoot.localPosition = Vector3.zero; // 위치 초기화
                popupRoot.localRotation = Quaternion.identity;
                popupRoot.localScale = Vector3.one; // 스케일 초기화
            }

            for (int index = (int)UIList.PANEL_START + 1; index < (int)UIList.PANEL_END; index++)
            {
                panels.Add((UIList)index, null); // 패널 딕셔너리에 초기화
            }

            for (int index = (int)UIList.POPUP_START + 1; index < (int)UIList.POPUP_END; index++)
            {
                popups.Add((UIList)index, null); // 팝업 딕셔너리에 초기화
            }
        }

        public T GetUI<T>(UIList uiName) where T : UIBase
        {
            Dictionary<UIList, UIBase> container = 
                uiName > UIList.POPUP_START &&
                uiName < UIList.POPUP_END ? popups : panels; // 팝업인지 패널인지 확인

           
            Transform root = 
                uiName > UIList.POPUP_START && 
                uiName < UIList.POPUP_END ? popupRoot : panelRoot; // 팝업 루트 또는 패널 루트 설정

            if(!container.ContainsKey(uiName)) // 딕셔너리에 UI가 없으면
            {
                return null; // null 반환
            }

            if (!container[uiName]) // UI가 null이 아니면
            {
                string path = $"UI/Prefabs/UI.{uiName}"; // UI 경로 설정
                GameObject uiPrefab = Resources.Load<GameObject>(path); // 리소스에서 UI 프리팹 로드

                if (!uiPrefab) return null; // 프리팹이 없으면 null 반환

                var component = Instantiate(uiPrefab, root).GetComponent<T>(); // UI 프리팹 인스턴스화
                container[uiName] = component;

                if (container[uiName]) 
                {
                    container[uiName].gameObject.SetActive(false); // UI를 비활성화 상태로 설정
                }
            }
            return (T)container[uiName]; // UI 반환
        }
        void Update() // 시간 정지 테스트용
        {
            Debug.Log("DeltaTime: " + Time.deltaTime);
        }
    }
}
