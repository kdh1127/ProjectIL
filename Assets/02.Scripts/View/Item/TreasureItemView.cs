using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreasureItemView : MonoBehaviour
{
    public UpgradeButtonView upgradeButtonView;
    public Image treasure_img;
    public TMP_Text name_txt;
    public TMP_Text increaseType_txt;
    public TMP_Text increase_txt;

    public void Init(Sprite sprite, string name, string increaseTypeString, int level, BigInteger increase)
    {
        var titleString = $"LV.{level} {LocalizationManager.GetTranslation(name)}";
        treasure_img.sprite = sprite;
        name_txt.text = titleString;
        increaseType_txt.text = LocalizationManager.GetTranslation(increaseTypeString);
        increase_txt.text = $"{increase}% 증가";

    }

    public void LevelUpdate(int level, string name, BigInteger totalIncreasePer)
    {
        var titleString = $"LV.{level} {LocalizationManager.GetTranslation(name)}";
        name_txt.text = titleString;
        increase_txt.text = $"{totalIncreasePer}% 증가";
    }
}
