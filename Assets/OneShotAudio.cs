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

	//ִ��Э�ɺ��� ���ҷ���ʱ��
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