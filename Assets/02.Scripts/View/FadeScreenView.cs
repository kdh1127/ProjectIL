using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeScreenView : MonoBehaviour
{
	[SerializeField] private Image fadeScreen_img;
	
	public void FadeIn(float transitionTime, Action onComplete = null)
	{
		fadeScreen_img.DOKill();
		fadeScreen_img.DOFade(0, transitionTime).OnComplete(() =>
		{
			onComplete?.Invoke();
		});
	}

	public void FadeOut(float transitionTime, Action onComplete = null)
	{
		fadeScreen_img.DOKill();
		fadeScreen_img.DOFade(1, transitionTime).OnComplete(() =>
		{
			onComplete?.Invoke();
		});
	}
}
