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
    public GameObject dim_img;
    public Image weapon_img;
    public TMP_Text title_txt;
    public TMP_Text level_txt;
    public TMP_Text totalAtack_txt;
    public GameObject selectFrame;

    readonly private int maxLevel = 5;

    public void Init(WeaponTable table, int curLevel, bool isMaxLevel, bool isEquiped, bool isUnLock, bool isEnughGold)
    {
        title_txt.text = table.Name;
        UpdateLevel(table, curLevel, isMaxLevel, isEquiped, isUnLock, isEnughGold);

    }

    public void UpdateLevel(WeaponTable table, int curLevel, bool isMaxLevel, bool isEquiped, bool isUnLock, bool isEnughGold)
    {
        UpdateTotalAttackText(table, curLevel);
        UpdateLevelText(curLevel, isMaxLevel);
        UpdateEquipedState(isEquiped);
        UpdateState(isUnLock, isMaxLevel, isEnughGold);
    }

    private void UpdateTotalAttackText(WeaponTable table, int curLevel)
	{
        var calcLevel = curLevel == 0 ? 0 : curLevel - 1;
        var totalAttack = table.BaseAtk.ToBigInt() + (calcLevel * table.Increase.ToBigInt());
        totalAtack_txt.text = totalAttack.ToAlphabetNumber();
    }

    private void UpdateLevelText(int curLevel, bool isMaxLevel)
	{
        level_txt.text = isMaxLevel ?
            $"<color=#FF0000>Lv.Max</color>" :
            $"Lv. {curLevel}/{maxLevel}";
    }

    private void UpdateEquipedState(bool isEquiped)
	{
        selectFrame.SetActive(isEquiped);
    }


    private void UpdateState(bool isUnLock, bool isMaxLevel, bool isEnughGold)
	{
        if (!isUnLock)
        {
            dim_img.SetActive(true);
            upgradeButtonView.SetInteractable(false);
            upgradeButtonView.gameObject.SetActive(true);
        }
        else if (isMaxLevel)
        {
            dim_img.SetActive(false);
            upgradeButtonView.gameObject.SetActive(false);
        }
        else
        {
            dim_img.SetActive(false);
            upgradeButtonView.gameObject.SetActive(true);
            upgradeButtonView.SetInteractable(isEnughGold);
        }
    }
}