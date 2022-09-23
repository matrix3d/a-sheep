using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotAudio : MonoBehaviour
{
	public void PlayOneShot(AudioClip clip, float volumeScale=1)
	{
		if (clip == null)
		{
			Destroy(gameObject);
			return;
		}

		var ass = GetComponent<AudioSource>();
		ass.PlayOneShot(clip, volumeScale);
		StartCoroutine(AudioPlayFinished(clip.length));
	}

	//执行协成函数 并且返回时间
	private IEnumerator AudioPlayFinished(float time)
	{
		yield return new WaitForSeconds(time);
		Destroy(gameObject);
	}

	public static void playOneShot(string url,float volumeScale=1)
	{
		var osa = GameObject.Instantiate(Resources.Load<OneShotAudio>("One shot audio"));
		osa.PlayOneShot(Resources.Load<AudioClip>("sound/" + url), volumeScale);
	}
}