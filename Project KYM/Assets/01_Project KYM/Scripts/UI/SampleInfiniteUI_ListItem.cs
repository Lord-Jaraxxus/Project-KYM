using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KYM
{
    enum ItemName
    {         
        Sword,
        Dagger,
        Shield,
        Bow,
        Staff,
        Axe,
        Mace,
        Spear,
        Wand,
        Crossbow,
    }

    enum ItemImagePath 
    {
        brave,
        friendship,
        pure,
        love,
        knowledge,
        diligence,
        hope,
        light,
    }

    public class SampleInfiniteUI_ListData : Gpm.Ui.InfiniteScrollData
    {
        public Color color;
        public string name;
        public string imagePath;
    }

    public class SampleInfiniteUI_ListItem : Gpm.Ui.InfiniteScrollItem
    {
        public UnityEngine.UI.Image background;
        public TMPro.TextMeshProUGUI nameText;
        public UnityEngine.UI.Image iconImage;

        public override void UpdateData(InfiniteScrollData scrollData)
        {
            var convertData = scrollData as SampleInfiniteUI_ListData;

            background.color = convertData.color;
            nameText.text = convertData.name;
            iconImage.sprite = Resources.Load<Sprite>($"UI/Images/{convertData.imagePath}");
        }
    }
}
