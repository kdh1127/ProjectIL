using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Numerics;
using AlphabetNumber;

public class WeaponItemView : MonoBehaviour
{ 
    public UpgradeButtonView upgradeButtonView;
    public Image weapon_img;
    public TMP_Text title_txt;
    public TMP_Text level_txt;
    public TMP_Text totalAtk_txt;

    public void Init(string title, int level, BigInteger baseAtk)
    {
        title_txt.text = title;
        level_txt.text = $"Lv. {level}/5";
        totalAtk_txt.text = new ANumber(baseAtk).ToAlphaString();
    }

    public void LevelUpdate(BigInteger baseAtk,BigInteger increase, int level)
    {
        BigInteger totalAtk = baseAtk + (level * increase);
        totalAtk_txt.text = totalAtk.ToString();
        level_txt.text = $"Lv. {level}/5";
    }
    
}
