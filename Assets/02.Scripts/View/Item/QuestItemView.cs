using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AlphabetNumber;

public class QuestItemView : MonoBehaviour
{
    public Image quest_img;
    public TMP_Text title_txt;
    public Slider quest_slider;
    public UpgradeButtonView upgradeButtonView;
    public TMP_Text time_txt;
    public TMP_Text reward_txt;
    public TMP_Text level_txt;

    public void Init(Sprite sprite, string title, float endTime, int level, int reward)
    {
        quest_img.sprite = sprite;
        title_txt.text = title;
        time_txt.text = $"{TimeSpan.FromSeconds(endTime)}";
        reward_txt.text = new ANumber(reward).ToAlphaString();
        level_txt.text = $"Lv.{level}";
    }

    public void ProgressUpdate(int curSecond, int endTime)
    {
        quest_slider.value = (float)curSecond / (float)endTime;
        time_txt.text = $"{TimeSpan.FromSeconds(endTime - curSecond)}";
    }

    public void LevelUpdate(string level, string reward)
    {
        reward_txt.text = reward;
        level_txt.text = $"Lv.{level}";
    }
}
