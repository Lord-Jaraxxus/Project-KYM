using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KYM
{
    public class  InteractionUI_ListData : InfiniteScrollData
    {
        public IInteractable Source { get; private set; } // 원본 인터랙터블 객체 참조 (필요시)
        public Sprite IconSprite { get; private set; }
        public string Message { get; private set; }
        public bool IsSelected { get; private set; }
        public int Count { get; private set; } 

        public InteractionUI_ListData(IInteractable source ,Sprite Icon, string message, bool isSelected = false) 
        {
            this.Source = source;
            this.IconSprite = Icon;
            this.Message = message;
            this.IsSelected = isSelected;
            this.Count = 1;
        }

        public void IncreaeCount() => Count++;
        public void DecreaseCount() 
        {
            if (Count > 0) Count--;
        }
    }


    public class InteractionUI_ListItem : InfiniteScrollItem
    {
        [SerializeField] private Image interactionIcon;
        [SerializeField] private TMPro.TextMeshProUGUI interactionText;

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
        }
    }
}
