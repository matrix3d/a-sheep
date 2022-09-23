using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
	public Texture texture;
	public Material material0;
	public Material material1;
	public GameObject showObj;
	//public class ButtonClickedEvent : UnityEvent { }
	public UnityEvent OnClick = new UnityEvent();
	public int layer;
	public int row;
	public int col;
	int _value = 0;

	public int Value { 
		get { return _value; }
		set {	
			_value = value;
			material0=Instantiate(material0);
			material1=Instantiate(material1);
			material0.mainTexture = texture;// SetTexture("_MainTex", texture);
			material1.mainTexture = texture;// .SetTexture("_MainTex", texture);
			showObj.GetComponent<Renderer>().material = material0;
			setAlpha(false);
		}
	}
	bool _mouseEnabled = true;
	public bool mouseEnabled {
		get { return _mouseEnabled; }
		set { 
			_mouseEnabled = value;
			setAlpha(!value,true);
		}
	}
	private void OnMouseDown()
	{
		if(!mouseEnabled)return;
		OnClick.Invoke();
	}
	public void setAlpha(bool value ,bool tween=false)
	{
		material0.DOKill();
		var tc = value ? Color.white * .25f : Color.white;
		if (tween)
		{
			material0.DOColor(tc, 0.3f);
		}
		else
		{
			material0.color = tc;
		}
	}
}
