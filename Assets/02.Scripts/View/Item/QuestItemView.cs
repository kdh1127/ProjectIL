using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using I2.Loc;

public class QuestItemView : MonoBehaviour
{
    public Image quest_img;
    public TMP_Text title_txt;
    public Slider quest_slider;
    public UpgradeButtonView upgradeButtonView;
    public TMP_Text time_txt;
    public TMP_Text reward_txt;
    public TMP_Text level_txt;

    string title;
    public void Init(Sprite sprite, string title, float endTime, int level, BigInteger reward)
    {
        this.title = title;
        var titleString = $"Lv.{level} {LocalizationManager.GetTranslation(title)}";
        var timeString = $"{TimeSpan.FromSeconds(endTime)}";
        var rewardString = $"+{reward.ToAlphabetNumber()}";

        quest_img.sprite = sprite;
        title_txt.text = titleString;
        time_txt.text = timeString;
        reward_txt.text = rewardString;
    }

    public void ProgressUpdate(int curSecond, int endTime)
    {
        var progress = (float)curSecond / (float)endTime;
        quest_slider.value = progress;
        time_txt.text = $"{TimeSpan.FromSeconds(endTime - curSecond)}";
    }

    public void UpdateLevel(string level, string reward)
    {
        var titleString = $"Lv.{level} {LocalizationManager.GetTranslation(title)}";
        var rewardString = $"+{reward}";
        title_txt.text = titleString;
        reward_txt.text = rewardString;
    }
}
