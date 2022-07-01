using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoHome_Custom : MonoBehaviour
{
    public RobotSelection RobotManager;
    private UrdfRobot Robot;
    private Transform EE_Frame;
    private Transform FK_Frame;


    public void GoHome(){
        Robot = RobotManager.SelectedRobot.GetComponent<UrdfRobot>();

        for(int i = 0; i < Robot.Values.Count; i++)
            Robot.Values[i] = 0;
        
        EE_Frame = Robot.transform.parent.Find("EE_Frame").gameObject.transform;
        FK_Frame = Robot.transform.parent.Find("FK_Frame").gameObject.transform;

        EE_Frame.position = FK_Frame.position;
        EE_Frame.rotation = FK_Frame.rotation;


        /*Robot.Values[0] = 0;
        Robot.Values[1] = 0;
        Robot.Values[2] = 0;
        Robot.Values[3] = 0;
        Robot.Values[4] = 0;
        Robot.Values[5] = 0;*/
    }
}
