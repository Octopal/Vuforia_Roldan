using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnPaladin : MonoBehaviour 
{

	[SerializeField] private GameObject paladinPrefab;

	void Update()
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit)) 
			{
				// Debug.Log ("Name = " + hit.collider.name);
				// Debug.Log ("Tag = " + hit.collider.tag);
				// Debug.Log ("Hit Point = " + hit.point);
				// Debug.Log ("Object position = " + hit.collider.gameObject.transform.position);
				if(hit.collider.CompareTag("PlayArea"))
				{
					if(paladinPrefab != null)
					{
						GameObject paladin = GameObject.Instantiate(paladinPrefab, hit.point, hit.transform.rotation, this.transform);
						paladin.GetComponent<NavMeshAgent>().enabled = true;
					}
					else
					{
						Debug.LogWarning("Missing paladinPrefab from SpawnPaladin component");
					}
				}
			}
		}
	}
}
