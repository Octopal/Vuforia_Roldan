﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PaladinBehaviour : MonoBehaviour {
	
	[SerializeField] private int maxHealth = 3;
	[SerializeField] private GameObject onFireFX;
	private float chanceToBlock = 50;

	private float meleeTimestamp = 0;
	private float meleeCooldown = 1;

	private Animator anim;
	private NavMeshAgent agent;
	private DragonBehaviour targetDragon;
	private int detectionRadius = 10;
	private AttackAnim attackAnim;
	private TauntAnim tauntAnim;
	private bool hasPathToDragon = false;
	private bool isDead = false;

	public int Health { get; set; }
	public bool AttackingDragon { get; set; }

	void Awake () 
	{
		anim = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
		RandomizeAttackAnim();
		RandomizeTauntAnim();
	}
	
	void RandomizeAttackAnim()
	{
		attackAnim = (AttackAnim)Random.Range(0, 7);
	}

	void RandomizeTauntAnim()
	{
		tauntAnim = (TauntAnim)Random.Range(0, 3);
	}

	void Start()
	{
		Health = maxHealth;
	}
	
	void Update () 
	{
		if(!isDead)
		{
			//If we don't see a dragon, look for a dragon.
			if(targetDragon == null)
				SearchForDragon();

			//If pally reaches dragon, switch to melee and start swinging.
			else if(targetDragon != null && agent != null)
			{
				print(agent.isStopped);
				if(!AttackingDragon && agent.remainingDistance > 0 && agent.remainingDistance - agent.stoppingDistance <= 0 && hasPathToDragon)
				{
					AttackingDragon = true;
					anim.SetBool("AttackingDragon", AttackingDragon);
				}
			}

			//Melee attack if it's time
			if(AttackingDragon && Time.time > meleeTimestamp)
			{
								
				meleeTimestamp = Time.time + meleeCooldown;
				if(Health > 0)
					StartCoroutine(MeleeAttackDragon());
			}
		}
	}

	void SearchForDragon()
	{
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
			if(hitColliders[i].transform.gameObject.CompareTag("Dragon"))
			{
				DragonBehaviour dragon = hitColliders[i].transform.GetComponent<DragonBehaviour>();
				if(dragon.Health > 0)
				{
					targetDragon = dragon;
					StartCoroutine(EngageDragon());
				}
			}
            i++;
        }
	}

	IEnumerator EngageDragon()
	{
		transform.LookAt(targetDragon.transform.position);
		anim.SetTrigger(tauntAnim.ToString());
		yield return new WaitForSeconds(1.2f);
		agent.SetDestination(targetDragon.transform.position);
		hasPathToDragon = true;
		anim.SetTrigger("Run");
	}

	IEnumerator MeleeAttackDragon()
	{
		if(targetDragon.Health > 0)
		{
			anim.SetTrigger(attackAnim.ToString());
			RandomizeAttackAnim();
			yield return new WaitForSeconds(.3f);
			targetDragon.TakeDamage(1);
		}
		else
		{
			RandomizeTauntAnim();
			anim.SetTrigger(tauntAnim.ToString());
		}
	}
	
	IEnumerator BlockFire()
	{
		agent.isStopped = true;
		anim.SetTrigger("BlockFire");
		yield return new WaitForSeconds(2.2f);
		if(!AttackingDragon)
			anim.SetTrigger("Run");
		else
			anim.SetTrigger("BattleIdle");
		agent.isStopped = false;
		agent.SetDestination(targetDragon.transform.position);
	}

	IEnumerator SetAblaze()
	{
		yield return new WaitForSeconds(.5f);
		onFireFX.SetActive(true);
		agent.isStopped = true;
		anim.SetTrigger("OnFire");
		Health = 0;
		isDead = true;
	}
	
	public void SetOnFire()
	{
		if(Random.Range(1, 101) < chanceToBlock)
		{
			StartCoroutine(BlockFire());
		}
		else
		{
			StartCoroutine(SetAblaze());
		}
	}
}

public enum AttackAnim
{
	Slash,
	Slash2,
	Slash3,
	Slash4,
	Slash5,
	Slash6,
	Kick
}

public enum TauntAnim
{
	Taunt,
	Taunt2,
	Taunt3
}