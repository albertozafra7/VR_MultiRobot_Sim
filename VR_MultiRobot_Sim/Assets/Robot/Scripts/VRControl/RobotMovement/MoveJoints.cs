using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MoveJoints : MonoBehaviour
{
	public UrdfRobot UrdfRobot;

	public URController URcontroller;


	public GameObject FK_Frame;
	public GameObject EE_Frame;

	public GeneralPathMover path; 

	public Slider auxJoint0, auxJoint1, auxJoint2, auxJoint3, auxJoint4, auxJoint5;

	void Update()
	{
		if(EE_Frame.transform.GetComponent<OVRGrabbable>().isGrabbed){
			auxJoint0.value = (UrdfRobot.GetComponent<UrdfRobot>().Values[0]) * Mathf.Rad2Deg;
			auxJoint1.value = (UrdfRobot.GetComponent<UrdfRobot>().Values[1]) * Mathf.Rad2Deg;
			auxJoint2.value = (UrdfRobot.GetComponent<UrdfRobot>().Values[2]) * Mathf.Rad2Deg;
			auxJoint3.value = (UrdfRobot.GetComponent<UrdfRobot>().Values[3]) * Mathf.Rad2Deg;
			auxJoint4.value = (UrdfRobot.GetComponent<UrdfRobot>().Values[4]) * Mathf.Rad2Deg;
			auxJoint5.value = (UrdfRobot.GetComponent<UrdfRobot>().Values[5]) * Mathf.Rad2Deg;
		}
		if (GameObject.Find("SlidersJoints") != null && !EE_Frame.transform.GetComponent<OVRGrabbable>().isGrabbed)// && !path.Run)
			EE_Frame.transform.position = FK_Frame.transform.position;

		/*if (FK_Frame.transform.position == EE_Frame.transform.position && FK_Frame.transform.rotation == EE_Frame.transform.rotation)
		{
			URcontroller.GetComponent<URController>().SnapToTarget = false;
			URcontroller.GetComponent<URController>().RunLinear = true;
		}*/
	}

	public void joint0(float value)
	{
		UrdfRobot.GetComponent<UrdfRobot>().Values[0] = value * Mathf.Deg2Rad;
	}
	public void joint1(float value)
	{
		UrdfRobot.GetComponent<UrdfRobot>().Values[1] = value * Mathf.Deg2Rad;
	}
	public void joint2(float value)
	{
		UrdfRobot.GetComponent<UrdfRobot>().Values[2] = value * Mathf.Deg2Rad;
	}
	public void joint3(float value)
	{
		UrdfRobot.GetComponent<UrdfRobot>().Values[3] = value * Mathf.Deg2Rad;
	}
	public void joint4(float value)
	{
		UrdfRobot.GetComponent<UrdfRobot>().Values[4] = value * Mathf.Deg2Rad;
	}
	public void joint5(float value)
	{
		UrdfRobot.GetComponent<UrdfRobot>().Values[5] = value * Mathf.Deg2Rad;
	}

	public void UpdateEE_Frame(){
		if (GameObject.Find("SlidersJoints") != null && !EE_Frame.transform.GetComponent<OVRGrabbable>().isGrabbed){
			EE_Frame.transform.position = FK_Frame.transform.position;
			EE_Frame.transform.rotation = FK_Frame.transform.rotation;
		}
	}

	public void UpdateJoints(){
		auxJoint0.value = (UrdfRobot.GetComponent<UrdfRobot>().Values[0]) * Mathf.Rad2Deg;
		auxJoint1.value = (UrdfRobot.GetComponent<UrdfRobot>().Values[1]) * Mathf.Rad2Deg;
		auxJoint2.value = (UrdfRobot.GetComponent<UrdfRobot>().Values[2]) * Mathf.Rad2Deg;
		auxJoint3.value = (UrdfRobot.GetComponent<UrdfRobot>().Values[3]) * Mathf.Rad2Deg;
		if(UrdfRobot.GetComponent<UrdfRobot>().Values.Count > 4){
			auxJoint4.value = (UrdfRobot.GetComponent<UrdfRobot>().Values[4]) * Mathf.Rad2Deg;
			auxJoint5.value = (UrdfRobot.GetComponent<UrdfRobot>().Values[5]) * Mathf.Rad2Deg;
		}
		UpdateEE_Frame();
	}

	
}
