using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFireFX : MonoBehaviour {

	float t = 5;
	Vector3 originalScale;

	void Awake()
	{
		originalScale = transform.localScale;
	}

	void OnEnable () {
		StartCoroutine(DestroyThis());
	}

	IEnumerator DestroyThis()
	{
		yield return new WaitForSeconds(2);
		while(t > 0)
		{
			t -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		    	transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, t);
		}
		Destroy(this.gameObject);
	}
}
