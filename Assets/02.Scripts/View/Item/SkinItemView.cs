using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SkinItemView : MonoBehaviour
{
    public Image skinImage;
    public TMP_Text title_txt;
    public TMP_Text level_txt;
    public TMP_Text totalIncrease_txt;
    public Toggle equipStatus_tgl;
    public SkinUpgradeButtonView upgradeButtonView;
    public Sprite equipSprite;
    public Sprite unEquipSprite;
    public TMP_Text equipStatus_txt;
    public Subject<bool> equipSkinSubject = new();

    public void Init(int level, int totalIncrease, string name = null, Sprite image = null)
    {
        level_txt.text = $"Lv.{level}";
        totalIncrease_txt.text = totalIncrease.ToString();
        equipStatus_txt.text = LocalizationManager.GetTranslation("Equip");
        if (name != null) title_txt.text = LocalizationManager.GetTranslation(name);
        if (image != null) skinImage.sprite = image;
    }

    public void OnChangedToggle(ToggleGroup toggleGroup)
    {
        equipStatus_tgl.onValueChanged.AddListener(isOn =>
        {
            if (isOn) toggleGroup.NotifyToggleOn(equipStatus_tgl);
            equipStatus_tgl.image.sprite = isOn ? equipSprite : unEquipSprite;
            equipStatus_txt.text = isOn ? LocalizationManager.GetTranslation("UnEquip") : LocalizationManager.GetTranslation("Equip");
            equipSkinSubject.OnNext(isOn);
        });
    }
}
