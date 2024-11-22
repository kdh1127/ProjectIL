using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestItemView : MonoBehaviour
{
    public Image quest_img;
    public TMP_Text title_txt;
    public Slider quest_slider;
    public UpgradeButtonView upgradeButtonView;
    public TMP_Text time_txt;
    public TMP_Text reward_txt;
    public TMP_Text level_txt;
    public Material progressBarMaterial;

    public void Init(Sprite sprite, string title, float endTime, int level, BigInteger reward)
    {
        quest_img.sprite = sprite;
        title_txt.text = title;
        time_txt.text = $"{TimeSpan.FromSeconds(endTime)}";
        reward_txt.text = reward.ToAlphabetNumber();
        level_txt.text = $"Lv.{level}";
    }

    public void ProgressUpdate(int curSecond, int endTime)
    {
        var progress = (float)curSecond / (float)endTime;
        quest_slider.value = progress;
        progressBarMaterial.SetFloat("_Progress", Mathf.Clamp01(progress));
        time_txt.text = $"{TimeSpan.FromSeconds(endTime - curSecond)}";
    }

    public void UpdateLevel(string level, string reward)
    {
        reward_txt.text = reward;
        level_txt.text = $"Lv.{level}";
    }
}
