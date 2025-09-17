using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class GameEventListener : MonoBehaviour
    {
        public System.Action<string, string> OnReceiveGameEvent;
        public void OnReceiveEvent(string eventName, string eventData)
        {
            OnReceiveGameEvent?.Invoke(eventName, eventData);
        }
    }
}
