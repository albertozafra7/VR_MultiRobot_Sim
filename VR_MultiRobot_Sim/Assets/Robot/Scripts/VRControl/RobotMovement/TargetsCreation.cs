using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// ---> new
[RequireComponent(typeof(BezierPath))]

public class TargetsCreation : MonoBehaviour
{
	public GameObject rootObj;
	public float n;// global time
	private float ti = 0;//time variable
	int numTarget = 10;
	GameObject duplicate;

	public SteamVR_TrackedObject trackedObject;
	public SteamVR_Controller.Device device;

	// ---> new
	[SerializeField]
	private BezierPath Path;

	public Material Material;
	public Material Red;
	DestroyAndHide Boolean;
	Hand targetSelect;
	public GameObject AttachedRight;
	void Start()
	{

		// Selects the right controller
		//GameObject RController = GameObject.Find("Controller (right)");
		// Gets the component selected by the right controller
		//trackedObject = RController.GetComponent<SteamVR_TrackedObject>();
		// ???

		device = SteamVR_Controller.Input((int)trackedObject.index);
		transform.localPosition = new Vector3(0, 0, 0);


	}

	// Update is called once per frame
	void Update()
	{

		targetSelect = AttachedRight.GetComponent<Hand>();

		if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && !targetSelect.TargetSelected)//&& !targetSelect.TargetSelected-----------------
		{
			
			GameObject duplicate = Instantiate(rootObj);//duplicate the object(FK_Frame)
			duplicate.name = "p" + numTarget;//change the name of the duplicated object
			numTarget += 10;
			GameObject ChildGameObject1 = duplicate.transform.GetChild(0).gameObject;//takes the child (in this case the sphere)
			ChildGameObject1.SetActive(true);//makes appear the sphere
			//*************************************************
			Boolean = GameObject.Find("Targets").GetComponent<DestroyAndHide>();
            if (Boolean.Frame){
				duplicate.transform.GetComponent<FrameVisualizer>().enabled = true;
            }
            else
            {
				duplicate.transform.GetComponent<FrameVisualizer>().enabled = false;

			}

			// Adds the components needed to the gameobject
			SphereCollider collider = duplicate.AddComponent(typeof(SphereCollider)) as SphereCollider;
			collider.radius = 0.05f;

			Rigidbody body = duplicate.AddComponent(typeof(Rigidbody)) as Rigidbody;
			body.isKinematic = true;
			body.useGravity = false;

			FixedJoint joint = duplicate.AddComponent(typeof(FixedJoint)) as FixedJoint;


			duplicate.transform.position = rootObj.transform.position;
			duplicate.transform.rotation = rootObj.transform.rotation;
			Debug.LogWarning("************************************************************************");
			duplicate.transform.parent = GameObject.Find("Targets").transform;//put the new objects in the empty object Targets
			duplicate.tag = "Target";

			


			//---> new
			Array.Resize(ref Path.GetComponent<BezierPath>().Targets, Path.GetComponent<BezierPath>().Targets.Length + 1);

			Path.GetComponent<BezierPath>().Targets[Path.GetComponent<BezierPath>().Targets.Length - 1] = duplicate.transform;

			Array.Resize(ref Path.GetComponent<BezierPath>().PointGRP, Path.GetComponent<BezierPath>().PointGRP.Length + 1);
			Path.GetComponent<BezierPath>().PointGRP[Path.GetComponent<BezierPath>().PointGRP.Length - 1] = duplicate.GetComponent<Target>();
		}

		if (Path.TTargets.Length > 1)
			DrawLine(Path.TTargets[Path.TTargets.Length - 2].position, Path.TTargets[Path.TTargets.Length - 1].position, Path.Points[Path.TTargets.Length - 1].MoveType);

	}

	void DrawLine(Vector3 p1, Vector3 p2, Target.TypeOfMoves Movement)
	{

		GL.Begin(GL.LINES);
		Material.SetPass(0);
		

        switch (Movement)
        {
            case Target.TypeOfMoves.MoveL:
				GL.Color(new Color(0.07058818f, 0.9803922f, 0.9344676f));
				break;
            case Target.TypeOfMoves.MoveJ:
				GL.Color(new Color(1f, 1f, 0f));
                break;
            case Target.TypeOfMoves.MoveP:
				GL.Color(new Color(0.588f, 0f, 0.8039f));
                break;
            default:
				GL.Color(new Color(Material.color.r, Material.color.g, Material.color.b, Material.color.a));
				break;
        }
        
		GL.Vertex3(p1.x, p1.y, p1.z);
		GL.Vertex3(p2.x, p2.y, p2.z);
		GL.End();
	}

	void OnRenderObject()
	{
		DrawPath();
	}

	// To show the lines in the editor
	void OnDrawGizmos()
	{
		DrawPath();
	}

	void DrawPath()
    {
		for (int i = 1; i < Path.TTargets.Length; i++)
		{
			if (Path.TTargets[i - 1] != null & Path.TTargets[i] != null)
				DrawLine(Path.TTargets[i - 1].position, Path.TTargets[i].position, Path.Points[i].MoveType);
		}
	}

}
