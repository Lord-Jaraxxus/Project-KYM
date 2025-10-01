using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KYM
{
    public class  InteractionUI_ListData : InfiniteScrollData
    {
        public string Key { get; private set; }
        public int Count => Interactables.Count; 

        public List<IInteractable> Interactables { get; private set; } = new(); // 원본 인터랙터블 객체 참조 (필요시)
        public Sprite IconSprite { get; private set; }
        public string Message { get; private set; }
        public bool IsSelected { get; set; }


        public InteractionUI_ListData(IInteractable source ,Sprite Icon, string message, bool isSelected = false) 
        {
            this.Key = source.Key;
            this.Interactables.Add(source);
            this.IconSprite = Icon;
            this.Message = message;
            this.IsSelected = isSelected;
        }

        public void AddInteractable(IInteractable interactable) => Interactables.Add(interactable); // 인터랙터블 객체 추가
        public void RemoveInteractable(IInteractable interactable) => Interactables.Remove(interactable); // 인터랙터블 객체 제거
    }


    public class InteractionUI_ListItem : InfiniteScrollItem
    {
        [SerializeField] private Image interactionIcon;
        [SerializeField] private TMPro.TextMeshProUGUI interactionText;
        [SerializeField] private GameObject selection;  

        public override void UpdateData(InfiniteScrollData scrollData)
        {
            var listData = scrollData as InteractionUI_ListData;

            interactionIcon.sprite = listData.IconSprite;

            string message = listData.Message;
            if (listData.Count > 1) 
            {
                message += $" x{listData.Count}";
            }

            interactionText.text = message;

            selection.SetActive(listData.IsSelected);
        }
    }
}
