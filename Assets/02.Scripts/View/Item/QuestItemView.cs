using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestItemView : MonoBehaviour
{
    public Image quest_img;
    public TMP_Text title_txt;
    public Slider quest_slider;
    public Button quest_btn;
    public TMP_Text increse_txt;
    public TMP_Text cost_txt;

    public void Init(Sprite sprite, string title, string increse, string cost)
    {
        quest_img.sprite = sprite;
        title_txt.text = title;
        increse_txt.text = increse;
        cost_txt.text = cost;
    }

    public bool ProgressUpdate(int curSecond, int time)
    {
        quest_slider.value = (float)curSecond / (float)time;
        Debug.Log($"{curSecond} / {time}");

        return quest_slider.value >= 1;
    }
}
