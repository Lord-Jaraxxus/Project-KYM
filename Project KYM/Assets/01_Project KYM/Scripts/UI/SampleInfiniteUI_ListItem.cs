using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class SampleInfiniteUI_ListData : Gpm.Ui.InfiniteScrollData
    {
        public Color color;
        public string message;
    }
    public class SampleInfiniteUI_ListItem : Gpm.Ui.InfiniteScrollItem
    {
        public UnityEngine.UI.Image background;
        public TMPro.TextMeshProUGUI messageText;

        public override void UpdateData(InfiniteScrollData scrollData)
        {
            var convertData = scrollData as SampleInfiniteUI_ListData;

            background.color = convertData.color;
            messageText.text = convertData.message;
        }
    }
}
