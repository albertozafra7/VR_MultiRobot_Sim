using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SimplePathMover))]

public class PathSimulator : MonoBehaviour
{

	[SerializeField]
	private SimplePathMover PathMover;
	
	private bool SimRunning = false;

	public void Simulation(){
		
		SimRunning = !SimRunning;
		
		PathMover.GetComponent<SimplePathMover>().Run = SimRunning;
	}
}
