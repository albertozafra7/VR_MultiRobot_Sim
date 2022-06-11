using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

public class Target : MonoBehaviour
{
    public UrdfRobot Robot;

    public Vector<float> Pose;

    public enum TypeOfMoves
    {
        MoveL,
        MoveJ,
        MoveP
    }

    public TypeOfMoves MoveType = TypeOfMoves.MoveL;

    public float accel = 0.0f;
    public float vel = 0.0f;
    public float time = 0.0f;
    public float zone = 0.0f;
    public string instruction = "";

    private const float def_LnP_accel = 1.2f;
    private const float def_J_accel = 1.4f;

    private const float def_LnP_vel = 0.25f;
    private const float def_J_vel = 1.05f;

    private const string MoveL = "movel";
    private const string MoveJ = "movej";
    private const string MoveP = "movep";


    void Start()
    {
        Pose = new DenseVector(Robot.Values.ToArray());
        ResetDefault();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetDefault()
    {
        switch (MoveType)
        {
            case TypeOfMoves.MoveL:
                accel = def_LnP_accel;
                vel = def_LnP_vel;
                instruction = MoveL + "([" + Pose[0] + "," + Pose[1] + "," + Pose[2] + "," + Pose[3] + "," + Pose[4] + "," + Pose[5] + "], a=" + accel + ", v=" + vel + ", t=" + time + ", r=" + zone + ")";
                break;

            case TypeOfMoves.MoveJ:
                accel = def_J_accel;
                vel = def_J_vel;
                instruction = MoveJ + "([" + Pose[0] + "," + Pose[1] + "," + Pose[2] + "," + Pose[3] + "," + Pose[4] + "," + Pose[5] + "], a=" + accel + ", v=" + vel + ", t=" + time + ", r=" + zone + ")";
                break;

            case TypeOfMoves.MoveP:
                accel = def_LnP_accel;
                vel = def_LnP_vel;
                instruction = MoveP + "([" + Pose[0] + "," + Pose[1] + "," + Pose[2] + "," + Pose[3] + "," + Pose[4] + "," + Pose[5] + "], a=" + accel + ", v=" + vel + ", r=" + zone + ")";
                break;

        }
    }
}
