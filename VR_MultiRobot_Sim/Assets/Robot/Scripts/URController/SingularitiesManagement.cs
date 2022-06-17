using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

public class SingularitiesManagement : MonoBehaviour
{
    public GameObject Warning;
    public URController robot;
    public UrdfRobot Robot;
    //public GameObject Robot;
    void Start()
    {
        //if (GameObject.Find("Warning Signal") != null)
        //    GameObject.Find("Warning Signal").SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Warning.activeSelf)
            Warning.transform.GetChild(0).transform.Rotate(Vector3.forward);

        if (robot.IsSingularJointPosition(new DenseVector(Robot.Values.ToArray()), robot.DHParams))
            SingularityEnter();
        else
            SingularityExit();
    }

    public void SingularityEnter()
    {
        Warning.SetActive(true);
    }

    public void SingularityExit()
    {
        Warning.SetActive(false);
    }
}
