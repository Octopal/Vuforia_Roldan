using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBuilder : MonoBehaviour {

	NavMeshSurface surface;

	void Start () {
		surface = GetComponent<NavMeshSurface>();
	}
	
	void Update () {
		surface.BuildNavMesh();
	}
}
