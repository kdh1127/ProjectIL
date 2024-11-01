using System.Numerics;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MonsterView : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private GameObject monsterCanvas;
	public TMP_Text damage_txt;
	public Slider hp_slider;
	public TMP_Text hp_txt;

	public Animator Animator => animator;

	private UnityEngine.Vector2 damageOriginPos;
	private void Awake()
	{
		var canvas_tr = Instantiate(monsterCanvas, transform).transform;
		damage_txt = canvas_tr.Find("Damage_txt").GetComponent<TMP_Text>();
		hp_slider = canvas_tr.Find("Hp_slider").GetComponent<Slider>();
		hp_txt = canvas_tr.Find("Hp_slider/Fill Area/Hp_txt").GetComponent<TMP_Text>();
		damageOriginPos = damage_txt.rectTransform.anchoredPosition;
	}

	public void ShowDamage(CommonClass.AttackInfo attackInfo)
	{
		damage_txt.DOKill();
		damage_txt.rectTransform.DOKill();
		damage_txt.text = attackInfo.damage.ToAlphabetNumber();
		damage_txt.rectTransform.anchoredPosition = damageOriginPos;
		damage_txt.DOFade(0f, 1f).SetEase(Ease.InQuart);
		damage_txt.rectTransform.DOAnchorPosY(0.2f, 1f);
		damage_txt.color = attackInfo.isCritical ? Color.red : Color.yellow;
	}

	public void UpdateHpBar(BigInteger curHp, BigInteger maxHp)
	{
		hp_slider.value = (float)(curHp / maxHp);
		hp_txt.text = curHp.ToAlphabetNumber();
	}
}
