using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace KYM
{
    public class InteractionUI : UIBase
    {
        [SerializeField] private InfiniteScroll infiniteScroll;
        [SerializeField] private InteractionUI_ListItem listItemPrefab;

        private Dictionary<string, InteractionUI_ListData> interactionDataMap = new();
        private int selectedIndex = -1;

        private void Awake() 
        {
            listItemPrefab.gameObject.SetActive(false);
        }


        private void OnEnable()
        {
            InputManager.Singleton.onInputMouseScroll += OnMouseScrollWhell;
            InputManager.Singleton.OnInputInteract += OnTryInteract;
        }

        private void OnDisable()
        {
            InputManager.Singleton.onInputMouseScroll -= OnMouseScrollWhell;
            InputManager.Singleton.OnInputInteract -= OnTryInteract;
        }

        void OnTryInteract() 
        {
            if (interactionDataMap.Count == 0) return;

            if (selectedIndex >= 0 && selectedIndex < interactionDataMap.Count) 
            {
                var data = infiniteScroll.GetData(selectedIndex);
                var convertData = data as InteractionUI_ListData;
                List<IInteractable> toRemove = new();
                foreach (var interactable in convertData.Interactables) 
                {
                    interactable.Interact();
                    if (interactable.IsOnceInteractable) 
                    {
                        toRemove.Add(interactable);
                    }
                }

                convertData.Interactables.RemoveAll(i => toRemove.Contains(i));
                if (convertData.Count <= 0)
                {
                    infiniteScroll.RemoveData(convertData);
                    interactionDataMap.Remove(convertData.Key);

                    if (interactionDataMap.Count > 0)
                    {
                        selectedIndex = (selectedIndex - 1) >= 0 ? selectedIndex - 1 : selectedIndex;
                        var nextData = infiniteScroll.GetData(selectedIndex);
                        var nextConvertData = nextData as InteractionUI_ListData;
                        nextConvertData.IsSelected = true;
                        infiniteScroll.UpdateData(nextConvertData);
                    }
                    else 
                    {
                        selectedIndex = -1;
                    }
                }
            }
        }

        void OnMouseScrollWhell(float value) 
        {
            if (interactionDataMap.Count == 0) return;

            int previousIndex = selectedIndex >= 0 ? selectedIndex : 0;

            if (value > 0f) // 위로 스크롤 
            {
                selectedIndex--;
                if (selectedIndex < 0) selectedIndex = interactionDataMap.Count - 1;
            }
            else if (value < 0f) // 아래로 스크롤 
            {
                selectedIndex++;
                if (selectedIndex >= interactionDataMap.Count) selectedIndex = 0;
            }

            var prevData = infiniteScroll.GetData(previousIndex);
            var prevConvertData = prevData as InteractionUI_ListData;
            prevConvertData.IsSelected = false;
            infiniteScroll.UpdateData(prevConvertData);

            var nextData = infiniteScroll.GetData(selectedIndex);
            var nextConvertData = nextData as InteractionUI_ListData;
            nextConvertData.IsSelected = true;
            infiniteScroll.UpdateData(nextConvertData);

            interactionDataMap[prevConvertData.Key] = prevConvertData;
            interactionDataMap[nextConvertData.Key] = nextConvertData;
        }




        public void AddInteractionData(IInteractable interactable) 
        {
            string key = interactable.Key;

            if (interactionDataMap.ContainsKey(key))
            {
                interactionDataMap[key].AddInteractable(interactable);
                infiniteScroll.UpdateData(interactionDataMap[key]);
            }
            else
            {
                bool isFirstAdded = interactionDataMap.Count == 0;
                var newData = new InteractionUI_ListData(interactable, interactable.InteractionIcon, interactable.InteractionMessage, isFirstAdded);
                interactionDataMap.Add(key, newData);
                infiniteScroll.InsertData(newData);

                selectedIndex = isFirstAdded ? 0 : selectedIndex;
            }
        }

        public void RemoveInteractionData(IInteractable interactable) 
        {
            if (interactionDataMap.TryGetValue(interactable.Key, out InteractionUI_ListData value)) 
            {
                if (value.Count > 1)
                {
                    value.RemoveInteractable(interactable);
                    infiniteScroll.UpdateData(value);
                }
                else 
                {
                    infiniteScroll.RemoveData(value);
                    interactionDataMap.Remove(interactable.Key);

                    // 선택된 아이템이 삭제된 경우, 인덱스 조정
                    if(selectedIndex >= interactionDataMap.Count && interactionDataMap.Count > 0)   // 선택됐던 아이템이 마지막이었고, 남은 아이템이 있을 때
                    {
                        selectedIndex = interactionDataMap.Count - 1;

                        var data = infiniteScroll.GetData(selectedIndex);
                        var convertData = data as InteractionUI_ListData;
                        convertData.IsSelected = true;
                        infiniteScroll.UpdateData(convertData);
                        interactionDataMap[convertData.Key] = convertData;
                    }
                    else if (selectedIndex < interactionDataMap.Count && selectedIndex >= 0)    // 선택됐던 아이템이 마지막이 아닐 때
                    {
                        var data = infiniteScroll.GetData(selectedIndex);
                        var convertData = data as InteractionUI_ListData;
                        // convertData.IsSelected = true;
                        infiniteScroll.UpdateData(convertData);
                        interactionDataMap[convertData.Key] = convertData;
                    }

                    if(interactionDataMap.Count <= 0) 
                    {
                        selectedIndex = -1;
                    }
                }
            }
        }
    }
}
