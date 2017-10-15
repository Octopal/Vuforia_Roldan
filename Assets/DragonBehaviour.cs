﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBehaviour : MonoBehaviour {

	[SerializeField] private GameObject fireBreathFX;

	[SerializeField] private float detectionRadius = 3;
	[SerializeField] private int maxHealth = 10;
	[SerializeField] private float fireDuration = 2;
	[SerializeField] private float fireCooldown = 4;


	public int Health { get; set; }

	private PaladinBehaviour targetPaladin;
	private Animator anim;
	private bool breathingFire;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Start()
	{
		Health = maxHealth;
	}

	void Update () 
	{
		if(targetPaladin == null)
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
				print("Found Pally");
				PaladinBehaviour paladin = hitColliders[i].transform.GetComponent<PaladinBehaviour>();
				if(!breathingFire && paladin.Health > 0 && !paladin.OnFire)
				{
					targetPaladin = paladin;
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
		transform.LookAt(targetPaladin.transform.position);

		//Anim
		anim.SetBool("Breathe Fire", true);

		//Trigger
		targetPaladin.SetOnFire();

		yield return new WaitForSeconds(fireDuration);

		anim.SetBool("Breathe Fire", false);
		fireBreathFX.SetActive(false);

		yield return new WaitForSeconds(fireCooldown);

		breathingFire = false;
		targetPaladin = null;
	}

	public IEnumerator TakeDamage()
	{
		yield return new WaitForSeconds(1);
	}

	//WHY DON'T ANIMATION EVENTS TAKE BOOLS AS A PARAM?!?!?
	void ToggleFireBreathingFX(string s)
	{
		//I'M SORRY 
		if(s == "true")
			fireBreathFX.SetActive(true);
		else if(s == "false")
			fireBreathFX.SetActive(false);
		else
			Debug.LogWarning("Animation Event string: \"" + s + "\" invalid");
	}


}
