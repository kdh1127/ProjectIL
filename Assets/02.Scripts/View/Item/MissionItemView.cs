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
    public TMP_Text name_txt;
    public CompleteButtonView completeButtonView;
    public GameObject disableButton;
    public GameObject clear_img;

    public void Init(MissionTable table, bool interactable, int curProgress)
    {
        var missionRewardImageResources = TRScriptableManager.Instance.GetSprite("MissionRewardImageResources").spriteDictionary;

        reward_img.sprite = missionRewardImageResources[table.RewardType];
        name_txt.text = table.Name;
        reward_txt.text = $"{table.Amount.ToBigInt().ToAlphabetNumber()}";
        SetState(interactable);
        UpdateProgress(curProgress, table.CompleteCount);
    }

    public void UpdateView(MissionTable table, bool interactable, int curProgress)
    {
        var missionRewardImageResources = TRScriptableManager.Instance.GetSprite("MissionRewardImageResources").spriteDictionary;

        reward_img.sprite = missionRewardImageResources[table.RewardType];
        name_txt.text = table.Name;
        reward_txt.text = $"{table.Amount.ToBigInt().ToAlphabetNumber()}";
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
        progress_txt.text = $"ÁøÇàµµ: {curValue} / {maxValue}";
	}


}
