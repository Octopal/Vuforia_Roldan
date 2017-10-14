using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBehaviour : MonoBehaviour {

	[SerializeField] private Transform raycastPoint;
	[SerializeField] private GameObject fireBreathFX;

	[SerializeField] private float detectionRadius = 3;

	[SerializeField] private float fireDuration = 1;
	[SerializeField] private float fireCooldown = 1;

	private PaladinBehaviour currentPaladin;
	private Animator anim;
	private bool breathingFire;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Update () 
	{
		if(currentPaladin == null)
			SearchForPaladin();
	}

	void SearchForPaladin()
	{
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
			if(hitColliders[i].transform.gameObject.CompareTag("Paladin"))
			{
				PaladinBehaviour paladin = hitColliders[i].transform.GetComponent<PaladinBehaviour>();
				if(!breathingFire && paladin.health > 0 && !paladin.onFire)
				{
					currentPaladin = paladin;
					StartCoroutine(BreatheFire());
				}
			}
            i++;
        }
	}

	IEnumerator BreatheFire()
	{
		breathingFire = true;

		//Rotate
		transform.LookAt(currentPaladin.transform.position);

		//Anim
		anim.SetBool("Breathe Fire", true);
		
		//FX
		fireBreathFX.SetActive(true);

		//Trigger
		StartCoroutine(currentPaladin.SetOnFire());

		yield return new WaitForSeconds(fireDuration);

		anim.SetBool("Breathe Fire", false);
		fireBreathFX.SetActive(false);

		yield return new WaitForSeconds(fireCooldown);

		breathingFire = false;
		currentPaladin = null;
	}

	public IEnumerator TakeDamage()
	{
		yield return new WaitForSeconds(1);
	}
}
