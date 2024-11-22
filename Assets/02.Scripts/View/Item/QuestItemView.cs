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

    public void Init(Sprite sprite, string title, float endTime, int level, BigInteger reward)
    {
        var titleString = LocalizationManager.GetTranslation(title);
        var timeString = $"{TimeSpan.FromSeconds(endTime)}";
        var rewardString = reward.ToAlphabetNumber();
        var levelString = $"Lv.{level}";

        quest_img.sprite = sprite;
        title_txt.text = titleString;
        time_txt.text = timeString;
        reward_txt.text = rewardString;
        level_txt.text = levelString;
    }

    public void ProgressUpdate(int curSecond, int endTime)
    {
        var progress = (float)curSecond / (float)endTime;
        quest_slider.value = progress;
        time_txt.text = $"{TimeSpan.FromSeconds(endTime - curSecond)}";
    }

    public void UpdateLevel(string level, string reward)
    {
        reward_txt.text = reward;
        level_txt.text = $"Lv.{level}";
    }
}
