using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UR_Path_Controller : MonoBehaviour
{

    #region ClassProperties

        #region TargetProperties
        [Header("Targets")]
            // Array which contains all the positions of the targets
            public Transform[] TTargets;
            /* get/set methods
            {
            get
            {
                return TTargets;
            }
            set
            {
                TTargets = value;
            }
        }*/

            // Array which contains all the properties of the targets (Those properties are the ones of the class Target)
            public Target[] TProperties;
            /* get/set methods
                    {
                        get
                        {
                            return TProperties;
                        }

                        set
                        {
                            TProperties = value;
                        }
                    }
              */
        #endregion

        #region RobotProperties
        [Header("Robot")]
                // Transform which is going to be moved around the path and will be followed by the robot End-Effector
                public Transform EE_Frame;
                // Transdorm which is the result of the Forward Kinematics of the robot
                public Transform FK_Frame;
                // The robot controller
                public URController robot;
                // Array of floats which contains the speed values of the robot before being modified
                private float[] Rprev_speeds = new float[3];
    #endregion

        #region PathProperties
        [Header("Path")]
            // Bool which determines wether the simulation is running or not
            public bool Run = false;
            // Float which determines the simulation speed
            public float General_Speed = 1.0f;
            // Enum which determines the direction of the simulation
            public enum Direction
            {
                forward,
                backwards
            }
            public Direction direction;
            // Transform which contains the HomePosition
            public Transform HomePos;


            // ------- Internal variables used for the simulation -------

            // Struct which emulates the class Transform, but only contains the position and the orientation of the object
            public struct transformation
                            {
                                public Vector3 position;
                                public Quaternion rotation;

                                // Constructor of the struct
                                public transformation(Transform input)
                                {
                                    position = input.position;
                                    rotation = input.rotation;
                                }
                            }

            // Integrer which contains the index of the current target where the robot is
            private int Tindex = 0;    // Initialy setted at a negative value in order to avoid errors at the start
            // Float which substitudes the time value at the linear interpolations
            private float t = 0;
    #endregion


    #endregion

    // Those are the methods generally used, as could be Awake(), Start() or Update()
    #region MainMethods
        // We look for the public components, just in case that the developer did not add them manually through the inspector
        void Awake()
        {
            if(EE_Frame == null)
                EE_Frame = GameObject.Find("EE_Frame").GetComponent<Transform>();
            if(FK_Frame == null)
                FK_Frame = GameObject.Find("FK_Frame").GetComponent<Transform>();
            if(robot == null)
                robot = GameObject.FindGameObjectWithTag("Robot").GetComponent<URController>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateTargetList();
            if (Run && Tindex < TTargets.Length && Vector3.Distance(EE_Frame.position, FK_Frame.position) <= 0.001)
            {
                switch (TProperties[Tindex].MoveType)
                {
                    case Target.TypeOfMoves.MoveL:
                        MoveL();
                        break;

                    case Target.TypeOfMoves.MoveJ:
                        MoveJ();
                        break;

                    case Target.TypeOfMoves.MoveP:
                        MoveP();
                        break;
                }
            }
        
        }

    #endregion

    // Those are the methos generated for the path simulation or specific uses
    #region CustomMethods

        #region SimulationControl

            public void StartSimulation()
            {
                Run = true;
            }


            public void PauseSimulation()
            {
                Run = false;
            }

            public void StopSimulation()
            {
                Run = false;
                JumpHome();
            }

            public void GoForwards()
            {
                direction = Direction.forward;
            }
            public void GoBackwards()
            {
                direction = Direction.backwards;

            }

            public float GetSimulationSpeed()
            {
                return General_Speed;
            }

            public void SetSimulationSpeed(float value)
            {
                General_Speed = value;
            }

            // Saves the current robot speeds
            private void StoreCurrentSpeed()
            {
                Rprev_speeds[0] = robot.PositionVelocity;
                Rprev_speeds[1] = robot.RotationVelocity;
                Rprev_speeds[2] = robot.JointVelocity;
            }
    
            // Calculates the Robot Speeds depending on the joint speed
            private void UpdateRobotSpeeds()
            {
                robot.PositionVelocity = 0.5f * TProperties[Tindex].Jvel / 1.05f;
                robot.RotationVelocity = 0.5f * 90 * TProperties[Tindex].Jvel / 1.05f;
                robot.JointVelocity = 0.5f * 360 * TProperties[Tindex].Jvel / 1.05f;
            }

            // Reset the previous speeds of the robot
            private void ResetPrevSpeed()
            {
                robot.PositionVelocity = Rprev_speeds[0];
                robot.RotationVelocity = Rprev_speeds[1];
                robot.JointVelocity = Rprev_speeds[2];
            }

        #endregion

        #region TargetManagement
            private int FindTargetIndex(Transform target)
            {
                int foundIndex = -1;
                for (int c = 0; c < TTargets.Length; c++)
                {
                    if (target.position == TTargets[c].transform.position && target.rotation == TTargets[c].transform.rotation) //transform.transform.position?? o solo transform.position?
                        foundIndex = c;

                }
                //If it is not found logs warning 
                if (foundIndex < 0)
                {
                    Debug.LogWarning("Target does not exist");
                    return -1;
                }
                else
                    return foundIndex;

            }

            private int FindTargetIndex(transformation target)
            {
                int foundIndex = -1;
                for (int c = 0; c < TTargets.Length; c++)
                {
                    if (target.position == TTargets[c].transform.position && target.rotation == TTargets[c].transform.rotation) //transform.transform.position?? o solo transform.position?
                        foundIndex = c;

                }
                //If it is not found logs warning 
                if (foundIndex < 0)
                {
                    Debug.LogWarning("Target does not exist");
                    return -1;
                }
                else
                    return foundIndex;

            }

            public Transform GetCurrentTarget()
            {
                return TTargets[Tindex];
            }

            public void SetCurrentTarget(Transform target)
            {
                Tindex = FindTargetIndex(target);
            }
            public void SetCurrentTarget(transformation target)
            {
                Tindex = FindTargetIndex(target);
            }

            //Return  next or prev target transform of the parameter target
            public Transform NextTarget(Transform target)
            {
                int targetIndex = FindTargetIndex(target);
                targetIndex++;
                if (targetIndex > TTargets.Length)
                {
                    Debug.LogWarning("There is no Next Target");
                    return null;
                }

                else
                {
                    Transform nextTarget = TTargets[targetIndex];
                    return nextTarget;
                }


            }
            public Transform NextTarget(transformation target)  // Method overload
            {
                int targetIndex = FindTargetIndex(target);
                targetIndex++;
                if (targetIndex > TTargets.Length)
                {
                    Debug.LogWarning("There is no Next Target");
                    return null;
                }

                else
                {
                    Transform nextTarget = TTargets[targetIndex];
                    return nextTarget;
                }
            }
            public Transform PrevTarget(Transform target)
            {
                int targetIndex = FindTargetIndex(target);
                targetIndex--;
                if (targetIndex < 0)
                {
                    Debug.LogWarning("There is no Previous Target"); //abuela no está seguro de que haya que devolver eso, yo no sé qué es points exactamente así que luego lo revise
                    return null;
                }

                else
                {
                    Transform prevTarget = TTargets[targetIndex];
                    return prevTarget;
                }


            }
            public Transform PrevTarget(transformation target)  // Method overload
            {
                int targetIndex = FindTargetIndex(target);
                targetIndex--;
                if (targetIndex < 0)
                {
                    Debug.LogWarning("There is no Previous Target"); //abuela no está seguro de que haya que devolver eso, yo no sé qué es points exactamente así que luego lo revise
                    return null;
                }

                else
                {
                    Transform prevTarget = TTargets[targetIndex];
                    return prevTarget;
                }


            }

            //Return the current target next or prev target transform 
            public Transform NextTarget()
            {
                return NextTarget(GetCurrentTarget());

            }
            public Transform PrevTarget()
            {
                return PrevTarget(GetCurrentTarget());
            }

            // If a target is added or deleted this method is called
            public void UpdateTargetList()
            {
                if (TTargets.Length != transform.childCount)
                {
                    TTargets = new Transform[transform.childCount];
                    TProperties = new Target[transform.childCount];
                }
                for (int i = 0; i < transform.childCount; i++)
                {
                    TTargets[i] = transform.GetChild(i);
                    ///Debug.Log(transform.GetChild(i).gameObject.GetComponent<Target>());
                    TProperties[i] = transform.GetChild(i).gameObject.GetComponent<Target>();
                }
            }


        #endregion

        // Methods which makes the robot move towards a defined position
        #region TargetMovement

            public void SnapToTarget(Transform target)
            {
                //To find the target array index
                int targetIndex = FindTargetIndex(target);

                //If it is not found logs warning 
                if (targetIndex < 0)
                    Debug.LogWarning("Could not Snap to Target");
                //If found jumps to Target
                else
                {
                    // Changes the robot movement to SnapToTarget
                    SetSnapToTarget();
                    // Moves the robot
                    SetEE_Frame(target);
                    Debug.Log("Jump to Target Succesfully");
                    Tindex = targetIndex + 1;
                    // -----------> hace falta volver a resetear runlinear? <-----------
                }

            }

            public void MoveToNextTarget()
            {
                //Saves the current path direction
                //bool currentDirection = backwards;
                Direction currentDirection = direction; 
                //Changes the direction to forward
                GoForwards();
                Transform nextTarget = NextTarget();
                Transform currentTarget = GetCurrentTarget();
                int nextIndex = FindTargetIndex(nextTarget); //sabemos cual es en realidad, pero lo hago así por si se cambia lo de i-1 
                int currentIndex = FindTargetIndex(currentTarget);
                //If the current target is the last one
                if (nextTarget == null)
                {

                    // RunWithoutZone(new transformation(TTargets[currentIndex]), new transformation(TTargets[0]), backwards);
                    Tindex = 0; ///ESTO DEBERÍA ESTAR EN RUNWITHOUT ZONE pq sino dará error porque el indice sera mayor que la length????


                }
                //else
                // RunWithoutZone(new transformation(Paths[0].TTargets[currentIndex]), new transformation(Paths[0].TTargets[nextIndex]), backwards);

                //Resets the direction to the one being used when "next target" button was pressed
                direction = currentDirection;
                MoveToTarget(NextTarget());
                Tindex = nextIndex;
            }

            public void MoveToPrevTarget()
            {
        //Saves the current path direction
                Direction currentDirection = direction;
                GoBackwards();
                Transform prevTarget = PrevTarget();
                Transform currentTarget = GetCurrentTarget();
                int prevIndex = FindTargetIndex(prevTarget); //sabemos cual es en realidad, pero lo hago así por si se cambia lo de i-1 
                int currentIndex = FindTargetIndex(currentTarget);
                //If the current target is the first one (does not exist a previous one)
                if (prevTarget == null)
                {
                    prevIndex = TTargets.Length;
                    /*//RunWithoutZone(new transformation(Paths[0].TTargets[currentIndex]), new transformation(Paths[0].TTargets[prevIndex]), backwards);
                    MoveLinear(new transformation(GetCurrentTarget()), new transformation(TTargets[prevIndex]));
                    Tindex = TTargets.Length; //ESTO DEBERÍA ESTAR EN RUNWITHOUT ZONE pq sino dará error porque el indice sera negativo ????*/

                }
                MoveToTarget(TTargets[prevIndex]);
                Tindex = prevIndex;
                //RunWithoutZone(new transformation(Paths[0].TTargets[currentIndex]), new transformation(Paths[0].TTargets[prevIndex]), backwards);

                //Resets the direction to the one being used when "previous target" button was pressed
                direction = currentDirection;
            }

            public void MoveToTarget(transformation target)
            {
                MoveLinear(new transformation(GetCurrentTarget()), target);
            }
       
            public void MoveToTarget(Transform target)
            {
                MoveLinear(new transformation(GetCurrentTarget()), new transformation(target));
            }

            public void GoInitialTarget()
            {
                Tindex = 0;
                MoveToTarget(TTargets[Tindex]);

            }
            public void GoFinalTarget()
            {
                Tindex = TTargets.Length - 1;
                MoveToTarget(TTargets[Tindex]);
            }

        #endregion


        #region RobotMethods

            #region MovementsToggle
    private void SetRunLinear()
                    {
                        robot.RunJoint = false;
                        robot.RunLinear = true;
                        robot.SnapToTarget = false;
                    }

                    private void SetRunJoint()
                    {
                        robot.RunJoint = true;
                        robot.RunLinear = false;
                        robot.SnapToTarget = false;
                    }

                    private void SetSnapToTarget()
                    {
                        robot.RunJoint = false;
                        robot.RunLinear = false;
                        robot.SnapToTarget = true;
                    }

                    private void ResetMovement()
                    {
                        robot.RunJoint = false;
                        robot.RunLinear = false;
                        robot.SnapToTarget = false;
                    }
        #endregion


            #region EE_FrameManagement
                // Stablishes the values of the EE_Frame according to a destination point
                public void SetEE_Frame(transformation destination)
                {
                    EE_Frame.position = destination.position;
                    EE_Frame.rotation = destination.rotation;
                }

                public void SetEE_Frame(Transform destination)
                {
                    SetEE_Frame(new transformation(destination));
                }

                public void SetEE_Frame(Vector3 position, Quaternion rotation)
                {
                    EE_Frame.position = position;
                    EE_Frame.rotation = rotation;
                }

                public transformation GetEE_Frame()
                {
                    return new transformation(EE_Frame);
                }

            #endregion


            #region HomeManagement
                public void JumpHome()
                {
                    Tindex = 0;
                    SnapToTarget(HomePos);
                }

                public void SetHome(transformation target)
                {
                    HomePos.position = target.position;
                    HomePos.rotation = target.rotation;
                }

                public void SetHome(Transform target)
                {
                    SetHome(new transformation(target));
                }

                public transformation GetHomePos()
                {
                    return new transformation(HomePos);
                }

    #endregion

        #endregion

        #region MoveTypes

            #region Comprobation
                public void MoveL()
                {
                    SetRunLinear();

                    if (Tindex + 1 < TTargets.Length && direction == Direction.forward)
                        MoveLinear(TTargets[Tindex], TTargets[Tindex + 1]);
                    else if (Tindex - 1 >= 0 && direction == Direction.backwards)
                        MoveLinear(TTargets[Tindex], TTargets[Tindex - 1]);
                    else if (Tindex - 1 < 0 && direction == Direction.backwards)
                        MoveLinear(TTargets[Tindex], TTargets[TTargets.Length - 1]);
                    else if (Tindex + 1 >= TTargets.Length && direction == Direction.forward)
                        MoveLinear(TTargets[Tindex], TTargets[0]);
                }

                public void MoveJ()
                {
                    SetRunJoint();

                    if (Tindex == 0 && Vector3.Distance(FK_Frame.position, HomePos.position) <= 0.001)
                        MoveJoint(TTargets[0]);
                    // In order to avoid jumping directly to the second target, if the robot is at the end of the path it goes to the start position with MoveL, if the last target is not a MoveJ
                    else if (Tindex == 0 && TProperties[TTargets.Length - 1].MoveType != Target.TypeOfMoves.MoveJ && Vector3.Distance(FK_Frame.position, TTargets[TTargets.Length - 1].position) <= 0.001 && direction == Direction.forward)
                    {
                        MoveLinear(TTargets[TTargets.Length - 1], TTargets[Tindex]);
                        Tindex = TTargets.Length - 1;
                    }
                    else if (Tindex == 0 && Vector3.Distance(FK_Frame.position, TTargets[TTargets.Length - 1].position) <= 0.001 && direction == Direction.forward)
                        MoveJoint(TTargets[0]);
                    // Same but inverse, when it goes backwards
                    else if (Tindex == TTargets.Length - 1 && Vector3.Distance(FK_Frame.position, TTargets[0].position) <= 0.001 && direction == Direction.backwards)
                        MoveJoint(TTargets[TTargets.Length - 1]);
                    else if (Tindex + 1 < TTargets.Length && direction == Direction.forward)
                        MoveJoint(TTargets[Tindex + 1]);
                    else if (Tindex + 1 >= TTargets.Length && direction == Direction.forward)
                        MoveJoint(TTargets[0]);
                    else if (Tindex - 1 >= 0 && direction == Direction.backwards)
                        MoveJoint(TTargets[Tindex - 1]);
                    else if (Tindex - 1 < 0 && direction == Direction.backwards)
                        MoveJoint(TTargets[TTargets.Length - 1]);
                }

                public void MoveP()
                {
                     SetRunLinear();

                    if (Tindex + 2 < TTargets.Length && direction == Direction.forward)
                        MoveProcess(TTargets[Tindex], TTargets[Tindex + 1], TTargets[Tindex + 2], TProperties[Tindex + 1].zone);
                    else if (Tindex - 2 > 0 && direction == Direction.backwards)
                        MoveProcess(TTargets[Tindex], TTargets[Tindex - 1], TTargets[Tindex - 2], TProperties[Tindex - 1].zone);
                    else if (Tindex - 1 >= 0 && Tindex - 2 < 0 && direction == Direction.backwards)
                        MoveProcess(TTargets[Tindex], TTargets[Tindex - 1], TTargets[0], TProperties[Tindex - 1].zone);
                    else if (Tindex - 1 < 0 && direction == Direction.backwards)
                        MoveLinear(TTargets[Tindex], TTargets[TTargets.Length - 1]);
                    else if (Tindex + 2 >= TTargets.Length && Tindex + 1 <= TTargets.Length && direction == Direction.forward)
                        MoveProcess(TTargets[Tindex], TTargets[Tindex + 1], TTargets[0], TProperties[Tindex + 1].zone);
                    else if (Tindex + 1 < TTargets.Length && direction == Direction.forward)
                        MoveLinear(TTargets[Tindex], TTargets[0]);
                }

            #endregion

             #region Calculus
                public void MoveLinear(transformation origin, transformation destination)
                {
                    // Move the robot to the next step
                    SetEE_Frame(Vector3.Lerp(origin.position, destination.position, t), Quaternion.Lerp(origin.rotation, destination.rotation, t));
                    // Calculates the next step
                    UpdateIndex(origin, destination);
                }
            
                public void MoveLinear(Transform origin, Transform destination)
                {
                    MoveLinear(new transformation(origin), new transformation(destination));
                }

                public void MoveJoint(transformation destination)
                {
                    StoreCurrentSpeed();
                    UpdateRobotSpeeds();
                    SetEE_Frame(destination);

                    if (Vector3.Distance(EE_Frame.position, FK_Frame.position) < 0.001)
                    {
                        if (direction == Direction.forward)
                            Tindex++;
                        else
                            Tindex--;

                        ResetPrevSpeed();

                        if (Tindex >= TTargets.Length - 1 && direction == Direction.forward)
                            Tindex = 0;
                        else if (Tindex <= 0 && direction == Direction.backwards)
                            Tindex = TTargets.Length - 1;
                    }

            
                }
    
                public void MoveJoint(Transform destination)
                {
                    MoveJoint(new transformation(destination));
                }

                public void MoveProcess(transformation origin, transformation waypoint, transformation destination, float zone)
                {
                    if (t <= zone)
                        SetEE_Frame(CalculateQ_Case1(origin, waypoint, TProperties[Tindex].accel));
                    else if (t > zone && t <= Tcalculus(origin.position.x, waypoint.position.x, TProperties[Tindex].vel, TProperties[Tindex].accel) - zone)
                        SetEE_Frame(CalculateQ_Case2(origin, waypoint, TProperties[Tindex].vel, TProperties[Tindex].accel));
                    else if (t > Tcalculus(origin.position.x, waypoint.position.x, TProperties[Tindex].vel, TProperties[Tindex].accel) - zone && t < Tcalculus(origin.position.x, waypoint.position.x, TProperties[Tindex].vel, TProperties[Tindex].accel))
                        SetEE_Frame(CalculateQ_Case3(origin, waypoint, TProperties[Tindex].vel, TProperties[Tindex].accel));
  
                    // Calculates the next step
                    UpdateIndex(origin, waypoint);
                }

                public void MoveProcess(Transform origin, Transform waypoint, Transform destination, float zone)
                {
                    MoveProcess(new transformation(origin), new transformation(waypoint), new transformation(destination), zone);
                }

                #region MoveP_Calculus

        // if zone >= t ----> origin + signo(waypoint - origin) * accel/2 * t^2
        private transformation CalculateQ_Case1(transformation origin, transformation waypoint, float accel)
                    {
                        transformation actualPos = new transformation(null);
                        if ((waypoint.position.x - origin.position.x) >= 0)
                            actualPos.position.x = origin.position.x + (accel / 2) * t * t;
                        else
                            actualPos.position.x = origin.position.x - (accel / 2) * t * t;

                        if ((waypoint.position.y - origin.position.y) >= 0)
                            actualPos.position.y = origin.position.y + (accel / 2) * t * t;
                        else
                            actualPos.position.y = origin.position.y - (accel / 2) * t * t;

                        if ((waypoint.position.z - origin.position.z) >= 0)
                            actualPos.position.z = origin.position.z + (accel / 2) * t * t;
                        else
                            actualPos.position.z = origin.position.z - (accel / 2) * t * t;

                        actualPos.rotation = Quaternion.Lerp(origin.rotation, waypoint.rotation, t);

                        return actualPos;

                    }

                    // if t > zone and t <= T-zone ----> origin - signo(waypoint - origin)* speed^2/(2*accel) + signo(waypoint - origin)*speed*t
                    private transformation CalculateQ_Case2(transformation origin, transformation waypoint, float speed, float accel)
                    {
                        transformation actualPos = new transformation(null);
                        if (waypoint.position.x - origin.position.x >= 0)
                            actualPos.position.x = origin.position.x - (speed * speed) / (2 * accel) + speed * t;
                        else
                            actualPos.position.x = origin.position.x + (speed * speed) / (2 * accel) - speed * t;

                        if (waypoint.position.y - origin.position.y >= 0)
                            actualPos.position.y = origin.position.y - (speed * speed) / (2 * accel) + speed * t;
                        else
                            actualPos.position.y = origin.position.y + (speed * speed) / (2 * accel) - speed * t;

                        if (waypoint.position.z - origin.position.z >= 0)
                            actualPos.position.z = origin.position.z - (speed * speed) / (2 * accel) + speed * t;
                        else
                            actualPos.position.z = origin.position.z + (speed * speed) / (2 * accel) - speed * t;

                        actualPos.rotation = Quaternion.Lerp(origin.rotation, waypoint.rotation, t);

                        return actualPos;

                    }

                    // if T-zone < t and t < T ----> q = waypoint - signo(waypoint-origin)*(-accel*T^2/2+accel*T*t - accel/2*t^2)
                    //                               T = signo(waypoint - origin)*(waypoint-origin)/speed + speed/accel
                    private transformation CalculateQ_Case3(transformation origin, transformation waypoint, float accel, float speed)
                    {
                        transformation actualPos = new transformation(null);
                        if (waypoint.position.x - origin.position.x >= 0)
                            actualPos.position.x = waypoint.position.x + (accel * t * Tcalculus(origin.position.x, waypoint.position.x, speed, accel) - (accel / 2) * t * t - (accel * Tcalculus(origin.position.x, waypoint.position.x, speed, accel) * Tcalculus(origin.position.x, waypoint.position.x, speed, accel)) / 2);
                        else
                            actualPos.position.x = waypoint.position.x - (accel * t * Tcalculus(origin.position.x, waypoint.position.x, speed, accel) - (accel / 2) * t * t - (accel * Tcalculus(origin.position.x, waypoint.position.x, speed, accel) * Tcalculus(origin.position.x, waypoint.position.x, speed, accel)) / 2);
                        if (waypoint.position.y - origin.position.y >= 0)
                            actualPos.position.y = waypoint.position.y + (accel * t * Tcalculus(origin.position.y, waypoint.position.y, speed, accel) - (accel / 2) * t * t - (accel * Tcalculus(origin.position.y, waypoint.position.y, speed, accel) * Tcalculus(origin.position.y, waypoint.position.y, speed, accel)) / 2);
                        else
                            actualPos.position.y = waypoint.position.y - (accel * t * Tcalculus(origin.position.y, waypoint.position.y, speed, accel) - (accel / 2) * t * t - (accel * Tcalculus(origin.position.y, waypoint.position.y, speed, accel) * Tcalculus(origin.position.y, waypoint.position.y, speed, accel)) / 2);
                        if (waypoint.position.z - origin.position.z >= 0)
                            actualPos.position.z = waypoint.position.z + (accel * t * Tcalculus(origin.position.z, waypoint.position.z, speed, accel) - (accel / 2) * t * t - (accel * Tcalculus(origin.position.z, waypoint.position.z, speed, accel) * Tcalculus(origin.position.z, waypoint.position.z, speed, accel)) / 2);
                        else
                            actualPos.position.z = waypoint.position.z - (accel * t * Tcalculus(origin.position.z, waypoint.position.z, speed, accel) - (accel / 2) * t * t - (accel * Tcalculus(origin.position.z, waypoint.position.z, speed, accel) * Tcalculus(origin.position.z, waypoint.position.z, speed, accel)) / 2);

                        actualPos.rotation = Quaternion.Lerp(origin.rotation, waypoint.rotation, t);
                        return actualPos;
                    }

                    private float Tcalculus(float origin, float waypoint, float speed, float accel)
                    {
                        float T = 0.0f;
                        if (waypoint - origin >= 0)
                            T = (waypoint - origin) / speed + speed / accel;
                        else
                            T = speed / accel - (waypoint - origin) / speed;

                        return T;
                    }
        #endregion

             #endregion

            // Updates Tindex and t according to several calculus and the direction of the simualtion
            private void UpdateIndex(transformation origin, transformation destination)
            {
                if (t < 1.0f)   // if the linear interpolation is not over
                {
                    if (TProperties[Tindex].time == 0.0f)   // If the time parameter is not used, t is calculated according to the speed
                        t += (0.5f * General_Speed * TProperties[Tindex].vel * Time.deltaTime / Vector3.Distance(origin.position, destination.position));
                    else
                        t += (0.5f * General_Speed * Time.deltaTime / (TProperties[Tindex].time / 2));
                }
                else if(Vector3.Distance(EE_Frame.position, FK_Frame.position) <= 0.001)
                {
                    t = 0;

                    if (direction == Direction.forward)
                        Tindex++;
                    else
                        Tindex--;

                    if (Tindex >= TTargets.Length - 1 && direction == Direction.forward)
                        Tindex = 0;
                    else if (Tindex <= 0 && direction == Direction.backwards)
                        Tindex = TTargets.Length - 1;

                }
            }
        #endregion
    #endregion
}
