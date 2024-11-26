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
    public TMP_Text totalIncrease_txt;
    public Toggle equipStatus_tgl;
    public SkinUpgradeButtonView upgradeButtonView;
    public Sprite equipSprite;
    public Sprite unEquipSprite;
    public TMP_Text equipStatus_txt;
    public Image line_img;
    public Subject<bool> equipSkinSubject = new();

    public void Init(int level, int totalIncrease, string name, string increaseTypeString, bool isEquip, bool isMaxLevel, Sprite image = null)
    {
        var titleString = $"Lv. {level} {LocalizationManager.GetTranslation(name)}";
        var increseStringFormat = I2.Loc.LocalizationManager.GetTranslation(increaseTypeString);
        totalIncrease_txt.text = string.Format(increseStringFormat, totalIncrease);
        equipStatus_txt.text = isEquip ? LocalizationManager.GetTranslation("UnEquip") : LocalizationManager.GetTranslation("Equip");
        title_txt.text = titleString;
        if (image != null) skinImage.sprite = image;
        line_img.rectTransform.sizeDelta = level > 0 && !isMaxLevel ? new Vector2(290f, 25f) : new Vector2(430f, 25f);
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
