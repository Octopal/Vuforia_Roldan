using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PaladinBehaviour : MonoBehaviour {

	[SerializeField] private int maxHealth = 3;

	private Animator anim;
	private NavMeshAgent agent;
	private DragonBehaviour targetDragon;
	private int detectionRadius = 10;
	
	public bool OnFire { get; set; }
	public int Health { get; set; }

	void Awake () 
	{
		anim = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
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
	}



	public void SearchForDragon()
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
					agent.SetDestination(dragon.transform.position);
				}
			}
            i++;
        }
	}
}
