using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class SampleLogUI : MonoBehaviour
    {
        public Gpm.Ui.InfiniteScroll infiniteScroll;
        public GameObject listItemPrefab;

        [SerializeField] public GameEventListener gameEventListener;

        private void Awake()
        {
            listItemPrefab.gameObject.SetActive(false);
        }

        private void Start()
        {
            GameEventListener.Instance.OnReceiveGameEvent += AddLog;
        }


        public void AddLog(string eventName, string log)
        {
            var newData = new SampleLogUI_ListData();
            newData.log = log;
            infiniteScroll.InsertData(newData);
        }
    }
}
