using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(URController))]

public class JumpHome : MonoBehaviour
{

	private SteamVR_TrackedObject trackedObject;
	public SteamVR_Controller.Device device;
	public GameObject HomePos;
	public GameObject EE_Frame;
	
	[SerializeField]
	private URController URcontroller;
	
    // Start is called before the first frame update
   /* void Start()
    {
		
		/////-------- Temporal ------------
		
		// Selects the right controller
    	GameObject RController = GameObject.Find("Controller (left)");
		// Gets the component selected by the right controller
		trackedObject = RController.GetComponent<SteamVR_TrackedObject>();
		// ???
		device = SteamVR_Controller.Input((int)trackedObject.index);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
			GoHome();
		if(device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
			ResetDefaultConfig();
    }
	
	void SetHome()
	{
		
	}*/
	
	public void GoHome()
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
	
	public void ResetDefaultConfig()
	{
		
		URcontroller.GetComponent<URController>().SnapToTarget = false;
		URcontroller.GetComponent<URController>().RunLinear = true;
	}
}
