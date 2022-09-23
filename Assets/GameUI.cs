using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{

	public GameObject winui;
	public GameObject failui;
	public void Show(bool isWin,bool isFail)
    {
        winui.SetActive(isWin);
		failui.SetActive(isFail);
		gameObject.SetActive(true);


		transform.localScale = Vector3.zero;
		//model.pivot = new Vector2(.5f, .5f);
		//model.TweenScale(new Vector2(1, 1), .5f).SetEase(FairyGUI.EaseType.BackOut);
		transform.DOScale(new Vector3(1, 1, 1), .5f).SetEase(Ease.OutBack);//.onComplete += () => { gameObject.SetActive(false); };
	}
}
