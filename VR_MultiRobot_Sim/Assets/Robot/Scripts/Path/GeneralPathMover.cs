using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class GeneralPathMover : MonoBehaviour
{

    public List<CurvePath> Paths = new List<CurvePath>();
    public Transform EE_Frame;
    public Transform FK_Frame;
    public bool Run = false;
    public bool circular = false;
    private int i = 0;
    private float t = 0;
    //private float RedFactor = 0.0f;
    //private float initDist = 0.0f;
    public float General_Speed = 1.0f;
    //private float time = 0;
    //private float initTime = 0.0f;
    public enum Zonesteps
    {
        step1,
        step2,
        step3
    }
    private Zonesteps currentStep = Zonesteps.step1;
    public struct transformation
    {
        public Vector3 position;
        public Quaternion rotation;
        public transformation(Transform input)
        {
            position = input.position;
            rotation = input.rotation;
        }
    }
    private transformation wayPoint;
    private bool nextAlso = false;
    public URController robot;

    private float[] robot_speeds = new float[3];
    void Start()
    {
        //RedFactor = 1.0f;
        //initTime = Time.deltaTime;
        // time = initTime;
    }
    void Update()
    {
        UpdateTargetList();

        if (Run)
        {
            if (i == 0 && Paths[0].Points[0].MoveType != Target.TypeOfMoves.MoveJ)
                i++;
            if (i < Paths[0].TTargets.Length)
            {
                switch (Paths[0].Points[i].MoveType)
                {
                    case Target.TypeOfMoves.MoveL:
                        MoveLinear();
                        break;
                    case Target.TypeOfMoves.MoveJ:
                        robot.RunJoint = true;
                        robot.RunLinear = false;
                        MoveJoint();
                        break;
                    case Target.TypeOfMoves.MoveP:
                        MoveLinear();
                        break;
                }
            }
            else
            {
                if (Vector3.Distance(FK_Frame.position, EE_Frame.position) <= 0.001 && Vector3.Distance(FK_Frame.position, EE_Frame.position) >= -0.001)
                    i = 0;
                else
                {
                    if (Paths[0].TTargets.Length > 0)
                    {
                        EE_Frame.position = Paths[0].TTargets[0].position;
                        EE_Frame.rotation = Paths[0].TTargets[0].rotation;
                    }
                }
            }
        }
    }
    public void Simulate()
    {
        Run = !Run;
    }

    public void UpdateTargetList()
    {
        if (Paths[0].TTargets.Length != transform.childCount)
        {
            Paths[0].TTargets = new Transform[transform.childCount];
            Paths[0].Points = new Target[transform.childCount];
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            Paths[0].TTargets[i] = transform.GetChild(i);
            ///Debug.Log(transform.GetChild(i).gameObject.GetComponent<Target>());
            Paths[0].Points[i] = transform.GetChild(i).gameObject.GetComponent<Target>();
        }
    }

    #region Linear Movement
    // Management of the points
    public void MoveLinear()
    {
        if (Paths[0].Points[i].zone == 0.0f)
        {
            RunWithoutZone(new transformation(Paths[0].TTargets[i - 1]), new transformation(Paths[0].TTargets[i]));
        }
        else if (i < Paths[0].TTargets.Length - 1)
        {
            if (!nextAlso)
                RunWithZone(new transformation(Paths[0].TTargets[i - 1]), new transformation(Paths[0].TTargets[i]), new transformation(Paths[0].TTargets[i + 1]), Paths[0].Points[i].zone);
            else
                RunWithZone(wayPoint, new transformation(Paths[0].TTargets[i]), new transformation(Paths[0].TTargets[i + 1]), Paths[0].Points[i].zone);
        }
    }
    #region Run Linear
    // Simulation of the linear path with zone == fine
    public void RunWithoutZone(transformation p0, transformation p1)
    {
        // Move the robot to the next step
        EE_Frame.position = Vector3.Lerp(p0.position, p1.position, t);
        EE_Frame.rotation = Quaternion.Lerp(p0.rotation, p1.rotation, t);
        // Calculates the next step
        if (t < 1.0f)
        {
            if (Paths[0].Points[i].time == 0.0f)
                t += (0.5f * General_Speed * Paths[0].Points[i].vel * Time.deltaTime / Vector3.Distance(p0.position, p1.position)); // * (time - initTime);
            else
                t += (0.5f * General_Speed * Time.deltaTime / (Paths[0].Points[i].time / 2));
        }
        else
        {
            t = 0;
            if (Paths[0].Points[i].zone == 0.0f)
                i++;
            //initTime = time;
        }
    }
    // Simulation of the linear path with zone != fine
    public void RunWithZone(transformation p0, transformation p1, transformation p2, float zone)
    {
        Vector3[] StepPoints = new Vector3[2];
        StepPoints = GetStepPoints(p0.position, p2.position, IntersecPointCalc(p0.position, p1.position, zone), IntersecPointCalc(p1.position, p2.position, zone));
        transformation[] Tr_StepPoints = new transformation[2];
        Tr_StepPoints[0].position = StepPoints[0];
        Tr_StepPoints[0].rotation = p1.rotation;
        Tr_StepPoints[1].position = StepPoints[1];
        Tr_StepPoints[1].rotation = p0.rotation;
        switch (currentStep)
        {
            case Zonesteps.step1:
                RunWithoutZone(p0, Tr_StepPoints[0]);
                if (Vector3.Distance(FK_Frame.position, StepPoints[0]) <= 0.001 || Vector3.Distance(p0.position, StepPoints[0]) <= 0.1)
                    currentStep++;
                break;
            case Zonesteps.step2:
                nextAlso = false;
                if (Vector3.Distance(p1.position, p2.position) < Vector3.Distance(p1.position, Tr_StepPoints[1].position))
                {
                    RunCurve(Tr_StepPoints[0], p1, p2);
                    if (Paths[0].Points[i + 1].zone != 0.0f)
                    {
                        nextAlso = true;
                        wayPoint.position = CalcWayPoint(p1.position, Paths[0].TTargets[i + 2].position, p2.position, p2.position, Paths[0].Points[i + 1].zone);
                        wayPoint.rotation = p2.rotation;
                        i++;
                        currentStep = Zonesteps.step1;
                    }
                    if (Vector3.Distance(FK_Frame.position, p2.position) <= 0.001)
                    {
                        currentStep = Zonesteps.step1;
                        i++;
                    }
                }
                else
                {
                    RunCurve(Tr_StepPoints[0], p1, Tr_StepPoints[1]);
                    if (Vector3.Distance(FK_Frame.position, StepPoints[1]) <= 0.001)
                    {
                        currentStep++;
                        if (Paths[0].Points[i + 1].zone != 0.0f)
                        {
                            nextAlso = true;
                            wayPoint.position = CalcWayPoint(p1.position, Paths[0].TTargets[i + 2].position, p2.position, Tr_StepPoints[1].position, Paths[0].Points[i + 1].zone);
                            wayPoint.rotation = Tr_StepPoints[1].rotation;
                            i++;
                            currentStep = Zonesteps.step1;
                        }
                    }
                }
                /*if (Paths[0].Points[i].zone != 0.0f)
                {
                    nextAlso = true;
                    wayPoint.position = StepPoints[1];
                    wayPoint.rotation = Tr_StepPoints[1].rotation;
                }*/
                break;
            case Zonesteps.step3:
                RunWithoutZone(Tr_StepPoints[1], p2);
                if (Vector3.Distance(FK_Frame.position, p2.position) <= 0.001)
                {
                    currentStep = Zonesteps.step1;
                    i += 2;
                    nextAlso = false;
                }
                break;
        }
    }
    #endregion
    #region Linear Calculus
    // Calculus of the points which are the result of the intersection between a sphere and a line
    private Vector3[] IntersecPointCalc(Vector3 p0, Vector3 p1, float zone)
    {
        float[] lambda = new float[2];
        Vector3[] Points = new Vector3[2];
        float a, b, c;
        // t^2
        a = Mathf.Pow((p0.x - p1.x), 2) + Mathf.Pow((p0.y - p1.y), 2) + Mathf.Pow((p0.z - p1.z), 2);
        // t
        b = 2 * a;
        c = a - Mathf.Pow(1 / zone, 2);
        /*c = Mathf.Pow(p1.x, 2) + Mathf.Pow(p1.y, 2) + Mathf.Pow(p1.z, 2) + Mathf.Pow(p0.x, 2) + Mathf.Pow(p0.y, 2) + Mathf.Pow(p0.z, 2);
        c -= 2 * (p1.x * p0.x + p1.y * p0.y + p1.z * p0.z);
        c -= Mathf.Pow(zone, 2);*/
        lambda[0] = (-b * Mathf.Sqrt(Mathf.Pow(b, 2) - 4 * a * c)) / (2 * a);
        lambda[1] = (-b * ((-1) * Mathf.Sqrt(Mathf.Pow(b, 2) - 4 * a * c))) / (2 * a);
        Points[0] = (p0 - lambda[0] * (p0 - p1));
        Points[1] = (p0 - lambda[1] * (p0 - p1));
        return Points;
    }
    // Selection of the step points of the path, from the intersection points previously calculated
    private Vector3[] GetStepPoints(Vector3 p0, Vector3 p2, Vector3[] Points01, Vector3[] Points12)
    {
        Vector3[] StepPoints = new Vector3[2];
        //Debug.Log(Points01[0] + "     " + Points01[1]);
        StepPoints[0] = GetNearest(p0, Points01);
        StepPoints[1] = GetNearest(p2, Points12);
        //Debug.LogWarning(StepPoints[0]);
        return StepPoints;
    }
    private Vector3 GetNearest(Vector3 mainPoint, Vector3[] points)
    {
        if (Vector3.Distance(mainPoint, points[0]) < Vector3.Distance(mainPoint, points[1]))
            return points[0];
        else
            return points[1];
    }
    private void RunCurve(transformation p0, transformation p1, transformation p2)
    {
        Vector3 L0_pos = Vector3.Lerp(p0.position, p1.position, t);
        Vector3 L1_pos = Vector3.Lerp(p1.position, p2.position, t);
        EE_Frame.position = Vector3.Lerp(L0_pos, L1_pos, t);
        Quaternion L0_rot = Quaternion.Lerp(p0.rotation, p1.rotation, t);
        Quaternion L1_rot = Quaternion.Lerp(p1.rotation, p2.rotation, t);
        EE_Frame.rotation = Quaternion.Lerp(L0_rot, L1_rot, t);
        if (t < 1.0f)
        {
            if (Paths[0].Points[i].time == 0.0f)
                t += (0.5f * General_Speed * Paths[0].Points[i].vel * Time.deltaTime / Vector3.Distance(p0.position, p2.position));
            else
                t += (0.5f * General_Speed * Time.deltaTime / (Paths[0].Points[i].time / 2));
        }
    }
    // Calculus of the third waypoint when there are several zones in a row
    Vector3 CalcWayPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 stepPoint, float zone)
    {
        Vector3 CalculatedWayPoint = Vector3.zero;
        Vector3[] NextStepPoints = new Vector3[2];
        NextStepPoints = GetStepPoints(p0, p1, IntersecPointCalc(p0, p2, zone), IntersecPointCalc(p2, p1, zone));
        NextStepPoints[1] = stepPoint;
        if (GetNearest(p1, NextStepPoints) == NextStepPoints[1])
            CalculatedWayPoint = stepPoint;
        else
            CalculatedWayPoint = (NextStepPoints[0] + NextStepPoints[1]) / 2;
        return CalculatedWayPoint;
    }
    #endregion
    #endregion
    #region Joint Movement
    // Management of the points
    public void MoveJoint()
    {
        UpdateSpeed();
        EE_Frame.position = Paths[0].TTargets[i].position;
        EE_Frame.rotation = Paths[0].TTargets[i].rotation;
        if (Vector3.Distance(EE_Frame.position, FK_Frame.position) < 0.001)
        {
            i++;
            robot.PositionVelocity = robot_speeds[0];
            robot.RotationVelocity = robot_speeds[1];
            robot.JointVelocity = robot_speeds[2];
        }
    }

    public void UpdateSpeed()
    {
        robot_speeds[0] = robot.PositionVelocity;
        robot_speeds[1] = robot.RotationVelocity;
        robot_speeds[2] = robot.JointVelocity;

        robot.PositionVelocity = 0.5f * Paths[0].Points[i].Jvel/1.05f;
        robot.RotationVelocity = 0.5f * 90 * Paths[0].Points[i].Jvel/1.05f;
        robot.JointVelocity = 0.5f * 360 * Paths[0].Points[i].Jvel / 1.05f;
    }
    #endregion
    #region Drawing
   /* void OnRenderObject()
    {
        Handles.color = Color.white;
        for (int i = 1; i < Paths[0].TTargets.Length; i++)
            Handles.DrawLine(Paths[0].TTargets[i - 1].position, Paths[0].TTargets[i].position);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (int i = 1; i < Paths[0].TTargets.Length; i++)
            Gizmos.DrawLine(Paths[0].TTargets[i - 1].position, Paths[0].TTargets[i].position);
    }*/
    #endregion
}