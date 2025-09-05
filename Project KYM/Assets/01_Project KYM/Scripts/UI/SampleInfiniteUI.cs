using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class SampleInfiniteUI : MonoBehaviour
    {
        public Gpm.Ui.InfiniteScroll infiniteScroll;
        public GameObject listItemPrefab;

        private void Awake()
        {
            listItemPrefab.gameObject.SetActive(false);
        }
        void Start()
        {
            for(int i = 0; i < 1000; i++)
            {
                SampleInfiniteUI_ListData newData = new SampleInfiniteUI_ListData();
                newData.color = Random.ColorHSV();

                int nameIndex = Random.Range(0, System.Enum.GetValues(typeof(ItemName)).Length);
                ItemName itemName = (ItemName)nameIndex;
                newData.name = itemName.ToString();

                int imageIndex = Random.Range(0, System.Enum.GetValues(typeof(ItemImagePath)).Length);
                ItemImagePath itemImage = (ItemImagePath)imageIndex;
                newData.imagePath = itemImage.ToString();

                infiniteScroll.InsertData(newData);
            }

            // infiniteScroll.RemoveData(); // 데이터를 InifiniteScroll에서 삭제할 때
            // infiniteScroll.UpdateData(); // 특정 데이터를 InifiniteScroll에서 갱신할 때
        }
    }
}
