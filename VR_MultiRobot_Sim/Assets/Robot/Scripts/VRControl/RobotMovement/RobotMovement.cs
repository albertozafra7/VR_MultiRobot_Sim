using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// When the trigger is pressed the robot should follow the movement of the controller
public class RobotMovement : MonoBehaviour {

	private SteamVR_TrackedObject trackedObject;
	public SteamVR_Controller.Device device;
	public GameObject target;

    public bool bol=false;
	
	
	float Xobject;
	float Yobject;
	float Zobject;
	float Xtarget;
	float Ytarget;
	float Ztarget;
	

    void Start() {
		
		// Selects the right controller
    	GameObject RController = GameObject.Find("Controller (right)");
		// Gets the component selected by the right controller
		trackedObject = RController.GetComponent<SteamVR_TrackedObject>();
		// ???
		device = SteamVR_Controller.Input((int)trackedObject.index);
		transform.localPosition = new Vector3(0, 0, 0);
    }


    void Update() {
		
		Xobject= transform.position.x;
		Yobject= transform.position.y;
		Zobject= transform.position.z;
		Xtarget= target.transform.position.x;
		Ytarget= target.transform.position.y;
		Ztarget= target.transform.position.z;
		
		Xobject=Math.Abs(Xobject);
		Yobject=Math.Abs(Yobject);
		Zobject=Math.Abs(Zobject);
		Xtarget=Math.Abs(Xtarget);
		Ytarget=Math.Abs(Ytarget);
		Ytarget=Math.Abs(Ytarget);
		
		
		if(Math.Abs(Xobject-Xtarget)<0.05 && Math.Abs(Yobject-Ytarget)<0.05 && Math.Abs(Zobject-Ztarget)<0.05)
			bol=true;
		else
			bol=false;
			
		bool l = false;
		if(device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
			l = true;
		
		// Selects the right controller
    	GameObject RController = GameObject.Find("Controller (right)");
	
        if (l && bol) {
		
			device.TriggerHapticPulse(10000);
			target.transform.position =  RController.transform.position;
  		}
  		
		if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {
			l = false;
			transform.localPosition = new Vector3(0, 0, 0);
  		}
    }
	
}
