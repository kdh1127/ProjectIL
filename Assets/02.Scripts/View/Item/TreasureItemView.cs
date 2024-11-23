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
    public TMP_Text level_txt;
    public TMP_Text increaseType_txt;
    public TMP_Text increase_txt;

    public void Init(/*Sprite sprite, */string name, string increaseTypeString, int level, BigInteger increase)
    {
        //treasure_img.sprite = sprite;
        name_txt.text = LocalizationManager.GetTranslation(name);
        level_txt.text = $"Lv.{level}(MAX.99999)";
        increaseType_txt.text = LocalizationManager.GetTranslation(increaseTypeString);
        increase_txt.text = $"{increase.ToString()}%";

    }

    public void LevelUpdate(int level, BigInteger totalIncreasePer)
    {
        level_txt.text = $"Lv.{level}(MAX.99999)";
        increase_txt.text = $"{totalIncreasePer}%";
    }
}
