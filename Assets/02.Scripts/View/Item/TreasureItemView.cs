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
    public TMP_Text totalIncreasePer_txt;

    public void Init(/*Sprite sprite, */string name, string increaseType, BigInteger totalIncreasePer, int level)
    {
        //treasure_img.sprite = sprite;
        name_txt.text = name;
        level_txt.text = $"Lv.{level}(MAX.99999)";
        increaseType_txt.text = increaseType;
        totalIncreasePer_txt.text = $"{totalIncreasePer}%";
    }

    public void LevelUpdate(string level, string totalIncreasePer)
    {
        level_txt.text = $"Lv.{level}(MAX.99999)";
        totalIncreasePer_txt.text = $"{totalIncreasePer}%";
    }
}