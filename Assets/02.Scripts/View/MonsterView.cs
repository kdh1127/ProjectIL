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

	[SerializeField] private TMP_Text damage_txt;
	[SerializeField] private Slider hp_slider;
	[SerializeField] private TMP_Text hp_txt;

	public Animator Animator => animator;

	private UnityEngine.Vector2 damageOriginPos;
	
	private void Awake()
	{
		var canvas_tr = Instantiate(monsterCanvas, transform).transform;
		damage_txt = canvas_tr.Find("Damage_txt").GetComponent<TMP_Text>();
		hp_slider = canvas_tr.Find("Hp_slider").GetComponent<Slider>();
		hp_txt = canvas_tr.Find("Hp_slider/Fill Area/Hp_txt").GetComponent<TMP_Text>();

		damageOriginPos = damage_txt.rectTransform.anchoredPosition;
		damage_txt.gameObject.SetActive(false);
	}

	public void ShowDamage(AttackInfo attackInfo)
	{
		// 이전 프로세스 종료
		damage_txt.DOKill();
		damage_txt.rectTransform.DOKill();

		// 데미지 텍스트 값 초기화 및 기존 위치 캐싱
		damage_txt.text = attackInfo.damage.ToAlphabetNumber();
		damage_txt.rectTransform.anchoredPosition = damageOriginPos;

		// 데미지 텍스트 활성화
		damage_txt.gameObject.SetActive(true);

		// 트윈 실행
		damage_txt.DOFade(0f, 1f).SetEase(Ease.InQuart);
		damage_txt.rectTransform.DOAnchorPosY(0.2f, 1f);
		damage_txt.color = attackInfo.isCritical ? Color.red : Color.yellow;
	}

	public void UpdateHpBar(BigInteger curHp, BigInteger maxHp)
	{
		hp_slider.value = (float)(curHp / maxHp);
		hp_txt.text = curHp < 0 ? "0" : curHp.ToAlphabetNumber();
	}
}
