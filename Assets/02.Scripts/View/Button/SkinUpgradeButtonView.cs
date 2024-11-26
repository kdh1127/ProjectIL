using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinUpgradeButtonView : UpgradeButtonView
{
    public void Init(bool isBought, int upgradeValue, int BuyCost, int upgradeCost, Sprite skinImage)
    {
        increase_txt.text = isBought ? $"+{upgradeValue}%" : LocalizationManager.GetTranslation("Buy");
        cost_txt.text = isBought ? $"{upgradeCost}" : $"{BuyCost}";
        cost_img.sprite = skinImage;
    }

    public void UpdateView(bool isBought, int upgradeValue, int BuyCost, int upgradeCost)
    {
        increase_txt.text = isBought ? $"+{upgradeValue}%" : LocalizationManager.GetTranslation("Buy");
        cost_txt.text = isBought ? $"{upgradeCost}" : $"{BuyCost}";
    }
}
