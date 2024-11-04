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
    public GameObject dim_img;
    public Image weapon_img;
    public TMP_Text title_txt;
    public TMP_Text level_txt;
    public TMP_Text totalAtack_txt;

    readonly private int maxLevel = 5;

    public void Init(WeaponTable table, int curLevel, bool isMaxLevel, bool isEquiped, bool isUnLock, bool isEnughGold)
    {
        var totalAttack = BigInteger.Parse(table.BaseAtk) + (curLevel * BigInteger.Parse(table.Increase));
        title_txt.text = table.Name;
        level_txt.text = $"Lv. {curLevel}/{maxLevel}";
        totalAtack_txt.text = totalAttack.ToAlphabetNumber();
        dim_img.SetActive(false);

        UpdateLevel(table, curLevel, isMaxLevel, isEquiped, isUnLock, isEnughGold);
    }

    public void UpdateLevel(WeaponTable table, int curLevel, bool isMaxLevel, bool isEquiped, bool isUnLock, bool isEnughGold)
    {
        var totalAttack = BigInteger.Parse(table.BaseAtk) + (curLevel * BigInteger.Parse(table.Increase));
        totalAtack_txt.text = totalAttack.ToAlphabetNumber();
        level_txt.text = isMaxLevel ?
                $"<color=#FF0000>Lv.Max</color>" :
                $"Lv. {curLevel}/{maxLevel}";

        weaponItemBg_img.color = isEquiped ? Color.yellow : Color.white;


        // 상태에 따른 UI 업데이트
        if (!isUnLock)
        {
            // 잠겨 있는 경우: Dim 처리하고 버튼 비활성화
            dim_img.SetActive(true);
            upgradeButtonView.SetInteractable(false);
            upgradeButtonView.gameObject.SetActive(true);
        }
        else if (isMaxLevel)
        {
            // 최대 레벨인 경우: Dim 제거하고 버튼 숨김
            dim_img.SetActive(false);
            upgradeButtonView.gameObject.SetActive(false);
        }
        else
        {
            // 잠금 해제된 경우: Dim 제거하고 버튼 활성화 여부 결정
            dim_img.SetActive(false);
            upgradeButtonView.gameObject.SetActive(true);
            upgradeButtonView.SetInteractable(isEnughGold);
        }
    }
}