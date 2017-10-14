using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinBehaviour : MonoBehaviour {

	public int health;

	[SerializeField] private Transform raycastPoint;

	private Animator anim;
	private DragonBehaviour dragon;

	void Awake () 
	{
		anim = GetComponent<Animator>();
	}
	
	void Update () 
	{
		if(dragon == null)
			SearchForDragon();
	}

	public void SearchForDragon()
	{

	}

	public IEnumerator SetOnFire()
	{
		health--;
		if(health <= 0)
			anim.SetBool("Dead", true);

		yield return new WaitForSeconds(1);
	}
}
