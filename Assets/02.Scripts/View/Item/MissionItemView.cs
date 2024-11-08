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

    public void Init(Sprite rewardType, int missionNo, string missionName, string reward, bool interactable)
    {
        reward_img.sprite = rewardType;
        missionNo_txt.text = $"{missionNo}번째 미션";
        missionName_txt.text = missionName;
        reward_txt.text = reward;
        completeButtonView.SetInteractable(interactable);
    }
}
