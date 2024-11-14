using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionItemView : MonoBehaviour
{
    public Image reward_img;
    public TMP_Text missionNo_txt;
    public TMP_Text missionName_txt;
    public TMP_Text reward_txt;
    public CompleteButtonView completeButtonView;

    public void Init(MissionTable table, bool interactable = false)
    {
        var missionRewardImageResources = TRScriptableManager.Instance.GetSprite("MissionRewardImageResources").spriteDictionary;

        reward_img.sprite = missionRewardImageResources[table.RewardType];
        missionNo_txt.text = $"{table.MissionNo}";
        missionName_txt.text = table.Name;
        reward_txt.text = $"{table.RewardType}+ {table.Amount}";
        completeButtonView.SetInteractable(interactable);
    }

    public void UpdateView(MissionTable table, bool interactable = false)
    {
        var missionRewardImageResources = TRScriptableManager.Instance.GetSprite("MissionRewardImageResources").spriteDictionary;

        reward_img.sprite = missionRewardImageResources[table.RewardType];
        missionNo_txt.text = $"{table.MissionNo}";
        missionName_txt.text = table.Name;
        reward_txt.text = $"{table.RewardType}+ {table.Amount}";
        completeButtonView.SetInteractable(interactable);
    }
}
