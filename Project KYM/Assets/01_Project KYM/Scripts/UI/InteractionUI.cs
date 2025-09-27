using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace KYM
{
    public class InteractionUI : UIBase
    {
        [SerializeField] private InfiniteScroll infiniteScroll;
        [SerializeField] private InteractionUI_ListItem listItemPrefab;

        private Dictionary<string, InteractionUI_ListData> interactionDataMap = new();
  

        private void Awake() 
        {
            listItemPrefab.gameObject.SetActive(false);
        }


        private void OnEnable()
        {
            GameEventListener.Instance.OnReceiveGameEvent += OnReceiveGameEvent;
        }

        private void OnDisable()
        {
            GameEventListener.Instance.OnReceiveGameEvent -= OnReceiveGameEvent;
        }

        private void OnReceiveGameEvent(string eventName, string eventData) 
        {
            if (eventName == "UpdateInventoryUI") 
            {
                if (interactionDataMap.TryGetValue(eventData, out InteractionUI_ListData data))
                {
                    // eventData가 Key인데.... data는 InteractionUI_ListData 타입이고.
                    RemoveInteractionData(data.Source);
                }
            }
        }




        public void AddInteractionData(IInteractable interactable) 
        {
            string key = interactable.Key;

            if (interactionDataMap.ContainsKey(key))
            {
                interactionDataMap[key].IncreaeCount();
                infiniteScroll.UpdateData(interactionDataMap[key]);
            }
            else
            {
                var newData = new InteractionUI_ListData(interactable, interactable.InteractionIcon, interactable.InteractionMessage);
                interactionDataMap.Add(key, newData);
                infiniteScroll.InsertData(newData);
            }
        }

        public void RemoveInteractionData(IInteractable interactable) 
        {
            if (interactionDataMap.TryGetValue(interactable.Key, out InteractionUI_ListData value)) 
            {
                if (value.Count > 1)
                {
                    value.DecreaseCount();
                    infiniteScroll.UpdateData(value);
                }
                else 
                {
                    infiniteScroll.RemoveData(value);
                    interactionDataMap.Remove(interactable.Key);
                }
            }
        }
    }
}
