using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(URController))]

public class GoHome : MonoBehaviour
{

	public GameObject HomePos;
	public GameObject EE_Frame;
	
	[SerializeField]
	private URController URcontroller;
	
    // Start is called before the first frame update
    void Start()
    {
		
		Go_Home();
        
    }


	void Go_Home()
	{
		
		//URcontroller = ;
		
		// Disable the movement
		URcontroller.GetComponent<URController>().RunLinear = false;
		// Change the coordenates of the End Effector
		EE_Frame.transform.position = HomePos.transform.position;
		EE_Frame.transform.rotation = HomePos.transform.rotation;
		// Enable the the SnapToTarget
		URcontroller.GetComponent<URController>().SnapToTarget = true;
	}
	
	void ResetDefaultConfig()
	{
		
		URcontroller.GetComponent<URController>().SnapToTarget = false;
		URcontroller.GetComponent<URController>().RunLinear = true;
	}
}
