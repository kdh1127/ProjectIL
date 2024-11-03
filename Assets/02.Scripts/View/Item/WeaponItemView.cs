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
    public Image weaponItemBg_img;
    public Image dim_img;
    public Image weapon_img;
    public TMP_Text title_txt;
    public TMP_Text level_txt;
    public TMP_Text totalAtack_txt;

    readonly private int maxLevel = 5;

    public void Init(WeaponTable table, int curLevel)
    {
        var totalAttack = BigInteger.Parse(table.BaseAtk) + (curLevel * BigInteger.Parse(table.Increase));
        title_txt.text = table.Name;
        level_txt.text = $"Lv. {curLevel}/{maxLevel}";
        totalAtack_txt.text = totalAttack.ToAlphabetNumber();
    }

    public void UpdateLevel(WeaponTable table, int curLevel)
    {
        var totalAttack = BigInteger.Parse(table.BaseAtk) + (curLevel * BigInteger.Parse(table.Increase));
        totalAtack_txt.text = totalAttack.ToAlphabetNumber();
        level_txt.text = curLevel == 5 ?
                $"<color=#FF0000>Lv.Max</color>" :
                $"Lv. {curLevel}/{maxLevel}";
    }

    public void UpdateWeaponViewStatus(EnumList.EWeaponItemUpgradeStatus weaponItemUpgradeStatus, bool isEnughCurrency)
    {
        switch (weaponItemUpgradeStatus)
        {
            case EnumList.EWeaponItemUpgradeStatus.MaxUpgrade:
                dim_img.color = new Color(0, 0, 0, 0);
                upgradeButtonView.gameObject.SetActive(false);

                break;
            case EnumList.EWeaponItemUpgradeStatus.Upgradeable:
                dim_img.color = new Color(0, 0, 0, 0);
                upgradeButtonView.gameObject.SetActive(true);
                upgradeButtonView.SetInteractable(isEnughCurrency);

                break;
            case EnumList.EWeaponItemUpgradeStatus.NotUpgradeable:
                dim_img.color = new Color(0, 0, 0, 1f);
                upgradeButtonView.gameObject.SetActive(true);
                upgradeButtonView.SetInteractable(false);
                break;
        }
    }

    public void UpdateListView(bool isCurWeapon)
    {
        weaponItemBg_img.color = isCurWeapon ? Color.yellow : Color.white;
    }
}