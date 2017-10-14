using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBehaviour : MonoBehaviour {

	[SerializeField] private Transform raycastPoint;
	[SerializeField] private GameObject fireBreathFX;

	[SerializeField] private float detectionRadius = 3;

	[SerializeField] private float fireDuration = 1;
	[SerializeField] private float fireCooldown = 1;

	private PaladinBehaviour paladin;
	private Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Update () 
	{
		if(paladin == null)
			SearchForPaladin();
		
		print(paladin == null);
	}

	void SearchForPaladin()
	{
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
			if(hitColliders[i].transform.gameObject.name == "Paladin")
			{
				paladin = hitColliders[i].transform.GetComponent<PaladinBehaviour>();
				StartCoroutine(BreatheFire(paladin));
			}
            i++;
        }

		// RaycastHit hit;

        // if (Physics.Raycast(raycastPoint.position, raycastPoint.forward, out hit, 100.0f))
		// 	print(hit.transform.name);
        //     if(hit.transform.gameObject.name == "Paladin")
		// 	{
		// 		paladin = hit.transform.GetComponent<PaladinBehaviour>();
		// 		StartCoroutine(BreatheFire(paladin));
		// 	}
	}

	IEnumerator BreatheFire(PaladinBehaviour paladin)
	{
		transform.LookAt(paladin.transform.position);
		anim.SetBool("Breathe Fire", true);
		fireBreathFX.SetActive(true);
		StartCoroutine(paladin.SetOnFire());
		yield return new WaitForSeconds(fireDuration);
		anim.SetBool("Breathe Fire", false);
		fireBreathFX.SetActive(false);
		yield return new WaitForSeconds(fireCooldown);

		if(paladin.health > 0)	
			StartCoroutine(BreatheFire(paladin));
		else
			paladin = null;
	}

	public IEnumerator TakeDamage()
	{
		yield return new WaitForSeconds(1);
	}
}
