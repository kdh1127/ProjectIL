using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonView : MonoBehaviour
{
    public TMP_Text increase_txt;
    public TMP_Text cost_txt;
    public Image cost_img;
    public Button button;

    public void Init(BigInteger increase, BigInteger cost, Sprite costImage)
    {
        increase_txt.text = $"+{increase.ToAlphabetNumber()}";
        cost_txt.text = cost.ToAlphabetNumber();
        cost_img.sprite = costImage;
    }

    public void SetInteractable(bool isInteractable)
    {
        button.interactable = isInteractable;
    }

}
