using UnityEngine;
using TMPro;
using UnityEngine.UI;
using I2.Loc;

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
        UpdateLevel(table, curLevel, isMaxLevel, isEquiped, isUnLock, isEnughGold);
        SetWeaponImage(table.Image);
        SetName(table.Name);
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

    private void SetWeaponImage(string imageKey)
	{
        var weaponImageResources = TRScriptableManager.Instance.GetSprite("WeaponImageResources").spriteDictionary;
        var weaponImage = weaponImageResources[imageKey];
        weapon_img.sprite = weaponImage; 
	}

    private void SetName(string titleKey)
	{
        var titleString = LocalizationManager.GetTranslation(titleKey);
        title_txt.text = titleString;
    }
}