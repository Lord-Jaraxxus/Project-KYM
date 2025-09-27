using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class GameEventListener : MonoBehaviour
    {
        public static GameEventListener Instance { get; private set; }

        public System.Action<string, string> OnReceiveGameEvent;

        private void Awake() => Instance = this;
        public void OnReceiveEvent(string eventName, string eventData = null)
        {
            OnReceiveGameEvent?.Invoke(eventName, eventData);
        }
    }
}
