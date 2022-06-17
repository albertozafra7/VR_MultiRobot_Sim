using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Hand : MonoBehaviour
{

	// Objects needed for the hand control
	public SteamVR_TrackedObject trackedObject;
	public SteamVR_Controller.Device device;

	public SteamVR_TrackedObject trackedObject_left;
	public SteamVR_Controller.Device device_left;


	private FixedJoint m_Joint = null;

	// The gameObject management, an interactable is a gameObject which can be interacted, like the robot wrist or a target
	private GameObject m_CurrentInteractable = null;
	private List<GameObject> m_ContactInteractables = new List<GameObject>();

	// Detects if the grip-button is continuosly pressed
	private bool pressed = false;

	// Determines the real extreme of the robot
	public GameObject FK_Frame;
	//increment position
	public Transform IncrPos;
	//ghostTarget
	public GameObject ghostTarget;
	private GameObject duplicate;

	//it is true when the object is attached the first time (fisrt frame), is false when the first frame has happened
	private bool first = true;

	// Rotate TCP
	public bool RotateTCP = false;

	//target blue
	public bool TargetSelected = false;


	// Start is called before the first frame update
	private void Start()
	{

		// Selects the right controller
		device = SteamVR_Controller.Input((int)trackedObject.index);

		device_left = SteamVR_Controller.Input((int)trackedObject_left.index);

		m_Joint = GetComponent<FixedJoint>();


		//IncrPos.;

	}

	// Update is called once per frame
	private void Update()
	{
		// Grip-button released
		if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))//m_GrabAction.GetStateUp(m_Pose.inputSource))
		{
			first = true;
			pressed = false;
			print("Grip Up");
			Drop();
		}
		// Grip-button pressed
		if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip) || pressed)//m_GrabAction.GetStateDown(m_Pose.inputSource))
		{
			pressed = true;
			print("Grip Down " + pressed);
			Pickup();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		
		
		// Detects if the interactable is the robot or a target
		if(!(other.gameObject.CompareTag("RobotWrist") || other.gameObject.CompareTag("Target")))
			return;
		m_ContactInteractables.Add(other.gameObject);

		GameObject nearest = GetNearestInteractable();
		if (nearest.CompareTag("Target")) {
			nearest.gameObject.GetComponentInChildren<MeshRenderer>().material.color = new Color(0f,1f, 0.9019608f);
			TargetSelected = true;
			nearest.GetComponent<Target>().TargetSelected = true;


		}
		else
        {
			foreach(GameObject interactable in m_ContactInteractables)
            {
				if(interactable.CompareTag("Target"))
					interactable.gameObject.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.9811321f, 0.9606132f, 0.3470986f);
					TargetSelected = false;
					//interactable.gameObject.GetComponent<Target>().TargetSelected = false;

			}
		}
	}
	
	private void OnTriggerExit(Collider other)
	{
		
		
		GameObject ChildGameObject1;
		if (!(other.gameObject.CompareTag("RobotWrist") || other.gameObject.CompareTag("Target")))
			return;
		if (other.CompareTag("Target"))
		{
			ChildGameObject1 = other.gameObject.transform.GetChild(0).gameObject;
			ChildGameObject1.GetComponent<Renderer>().material.color = new Color(0.9811321f, 0.9606132f, 0.3470986f);//amarillo
			TargetSelected = false;
			other.GetComponent<Target>().TargetSelected = false;

		}
		m_ContactInteractables.Remove(other.gameObject);

		if (m_ContactInteractables.ToArray().Length > 0)
		{
			GameObject nearest = GetNearestInteractable();
			if (nearest.CompareTag("Target"))
			{
				nearest.gameObject.GetComponentInChildren<MeshRenderer>().material.color = new Color(0f, 1f, 0.9019608f);
				TargetSelected = true;
				nearest.GetComponent<Target>().TargetSelected = true;
			}
		}
	}

    public void Pickup()
	{
		// Get nearest
		if (first)
		{
			m_CurrentInteractable = GetNearestInteractable();
            if (m_CurrentInteractable != null)
            {
				if (m_CurrentInteractable.CompareTag("Target")&&(FK_Frame.transform.position!=m_CurrentInteractable.transform.position)) {
					duplicate = Instantiate(ghostTarget);
					duplicate.transform.position = m_CurrentInteractable.transform.position;
					duplicate.transform.rotation = m_CurrentInteractable.transform.rotation;
					//GameObject.Find("ghostTarget").GetComponent(FrameVisualizer).enabled = false;
					//Debug.LogWarning("holaaaaaaaaaaaaaa");
					//GameObject varGameObject = GameObject.Find("ghostTarget"); 
					//ghostTarget.SetActive(true);
				}
            }

		}
		// Null check
		if (m_CurrentInteractable != null)
		{

			// Attach
			Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
			m_Joint.connectedBody = targetBody;

			// Positing
			if (!first)
			{
				// RotateTCP
				if ((m_CurrentInteractable.CompareTag("RobotWrist") && !RotateTCP) || (m_CurrentInteractable.CompareTag("Target")))
					m_CurrentInteractable.transform.position = m_CurrentInteractable.transform.position + (transform.position - IncrPos.position);
				m_CurrentInteractable.transform.rotation = m_CurrentInteractable.transform.rotation * (transform.rotation * Quaternion.Inverse(IncrPos.rotation));//Quaternion.Euler(180, 0, 0) );
				//m_CurrentInteractable.transform.rotation = m_CurrentInteractable.transform.rotation * (transform.rotation * Quaternion.Inverse(IncrPos.rotation));
			}
			// Rotation
			//m_CurrentInteractable.transform.rotation =  transform.rotation * Quaternion.Euler(270, 0, 0);

			if (device_left.GetPressDown(SteamVR_Controller.ButtonMask.Grip) && m_CurrentInteractable.CompareTag("Target")) {
				Destroy(duplicate);
				//ghostTarget.SetActive(false);
				TargetDestroy();
			}
			IncrPos.position = transform.position;
			IncrPos.rotation = transform.rotation;
			//it is true when the object is attached the first time (fisrt frame), is false when the first frame has happened
		}
		first = false;
	}

	public void Drop()
	{
		// Null check
		if (!m_CurrentInteractable)
			return;

		if (m_CurrentInteractable.CompareTag("RobotWrist"))
		{
			m_CurrentInteractable.transform.position = FK_Frame.transform.position;
			m_CurrentInteractable.transform.rotation = FK_Frame.transform.rotation;
		}

		// Apply velocity
		//Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
		//targetBody.velocity = m_Pose.GetVelocity();
		//targetBody.angularVelocity = m_Pose.GetAngularVelocity();
		// Detach
		Destroy(duplicate);
		//ghostTarget.SetActive(false);
		m_Joint.connectedBody = null;

		// Clear
		m_CurrentInteractable = null;
		//it is true when the object is attached the fisrt time (fisrt frame)
	}

	private GameObject GetNearestInteractable()
	{
		GameObject nearest = null;
		float minDistance = float.MaxValue;
		float distance = 0.0f;

		foreach (GameObject interactable in m_ContactInteractables)
		{
			distance = (interactable.transform.position - transform.position).sqrMagnitude;

			// Bigger area of collision for the robot
			if (interactable.CompareTag("RobotWrist"))
			{
				if (distance < minDistance + 0.05)
				{
					minDistance = distance;
					nearest = interactable;
				}
			}
			else if (distance < minDistance)
			{
				minDistance = distance;
				nearest = interactable;
			}

		}

		return nearest;
	}

	private void TargetDestroy()
	{
		// Remove from the list of interactables
		m_ContactInteractables.Remove(m_CurrentInteractable);

		// Destroy the game-object
		Destroy(m_CurrentInteractable);
		// Detach
		m_Joint.connectedBody = null;

		// Clear
		m_CurrentInteractable = null;

		// drop
		pressed = false;
	}

	public void ToggleRotateTCP()
    {
		RotateTCP = !RotateTCP;
    }

	

}

