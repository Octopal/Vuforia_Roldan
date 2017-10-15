using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PaladinBehaviour : MonoBehaviour {

	[SerializeField] private int maxHealth = 3;
	[SerializeField] private GameObject onFireFX;
	private float chanceToBlock = 50;

	private Animator anim;
	private NavMeshAgent agent;
	private DragonBehaviour targetDragon;
	private int detectionRadius = 10;
	private AttackAnim attackAnim;

	public bool OnFire { get; set; }
	public int Health { get; set; }

	void Awake () 
	{
		anim = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
	}
	
	void RandomizeAttackAnim()
	{
		attackAnim = (AttackAnim)Random.Range(0, 6);
	}

	void Start()
	{
		Health = maxHealth;
	}
	
	void Update () 
	{
		if(targetDragon == null)
			SearchForDragon();
		else
		{
			
		}

		if(targetDragon != null && agent != null)
		{
			//print(Vector3.Distance(transform.position, targetDragon.transform.position) + "  :  isStopped = " + agent.isStopped);
		}
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
		anim.SetTrigger("Point");
		yield return new WaitForSeconds(1);
		agent.SetDestination(targetDragon.transform.position);
		anim.SetTrigger("Run");
	}

	IEnumerator BlockFire()
	{
		agent.isStopped = true;
		anim.SetTrigger("BlockFire");
		yield return new WaitForSeconds(2.2f);
		anim.SetTrigger("Run");
		agent.isStopped = false;
	}

	IEnumerator SetAblaze()
	{
		yield return new WaitForSeconds(.5f);
		onFireFX.SetActive(true);
		agent.isStopped = true;
		anim.SetTrigger("OnFire");
		Health = 0;
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