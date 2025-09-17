using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class SampleLogUI_ListData : Gpm.Ui.InfiniteScrollData
    {
        public string log;
    }

    public class  SampleLogUI_ListItem : Gpm.Ui.InfiniteScrollItem
    {
        public TMPro.TextMeshProUGUI logText;

        public override void UpdateData(InfiniteScrollData scrollData)
        {
            var convertData = scrollData as SampleLogUI_ListData;

            logText.text = convertData.log;
        }

    }
}
