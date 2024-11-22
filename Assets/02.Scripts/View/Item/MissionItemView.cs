using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionItemView : MonoBehaviour
{
    public Image reward_img;
    public TMP_Text reward_txt;
    public TMP_Text progress_txt;
    public TMP_Text title_txt;
    public CompleteButtonView completeButtonView;
    public GameObject disableButton;
    public GameObject clear_img;

    public void Init(MissionTable table, bool interactable, int curProgress)
    {
        var missionRewardImageResources = TRScriptableManager.Instance.GetSprite("MissionRewardImageResources").spriteDictionary;

        reward_img.sprite = missionRewardImageResources[table.RewardType];
        reward_txt.text = $"{table.Amount.ToBigInt().ToAlphabetNumber()}";
        SetTitle(table);
        SetState(interactable);
        UpdateProgress(curProgress, table.CompleteCount);
    }

    public void UpdateView(MissionTable table, bool interactable, int curProgress)
    {
        var missionRewardImageResources = TRScriptableManager.Instance.GetSprite("MissionRewardImageResources").spriteDictionary;

        reward_img.sprite = missionRewardImageResources[table.RewardType];
        reward_txt.text = $"{table.Amount.ToBigInt().ToAlphabetNumber()}";
        SetTitle(table);
        SetState(interactable);
        UpdateProgress(curProgress, table.CompleteCount);
    }

    public void SetState(bool isClear)
	{
        clear_img.SetActive(isClear);
        completeButtonView.gameObject.SetActive(isClear);
        completeButtonView.SetInteractable(isClear);
        disableButton.SetActive(!isClear);
	}

    public void UpdateProgress(int curValue, int maxValue)
	{
        var progressStringFormat = LocalizationManager.GetTranslation("Progress_String_Format");
        var progressString = string.Format(progressStringFormat, curValue, maxValue);
        progress_txt.text = progressString;
	}

    public void SetTitle(MissionTable table)
	{
        var nameStringFormat = LocalizationManager.GetTranslation(table.Name);
        var missionTargetString = GetMissionTargetString(table);
        var titleString = string.Format(nameStringFormat, missionTargetString, table.CompleteCount);
        title_txt.text = titleString;

    }

    public string GetMissionTargetString(MissionTable table)
	{
        var missionType = table.MissionType.ToEnum<EMissionType>();
		switch (missionType)
		{
			case EMissionType.QuestUpgrade:          
			case EMissionType.QuestClear:
                return LocalizationManager.GetTranslation(QuestTableList.Get()[table.TargetNo].Name);
            case EMissionType.WeaponUpgrade:
                return LocalizationManager.GetTranslation(WeaponTableList.Get()[table.TargetNo].Name);
            case EMissionType.DungeonClear:
                return LocalizationManager.GetTranslation("Stage");
        }
        return null;
	}
}