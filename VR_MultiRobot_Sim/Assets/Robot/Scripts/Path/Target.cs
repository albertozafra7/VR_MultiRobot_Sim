using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

public class Target : MonoBehaviour
{
    public UrdfRobot Robot;
    //public GameObject Robot;

    public Vector<float> Pose;
    
    public SteamVR_TrackedObject trackedObject;
    public SteamVR_Controller.Device device;

    public SteamVR_TrackedObject trackedObject_left;
    public SteamVR_Controller.Device device_left;

    public GameObject pointer;

    public bool TargetSelected=false;
    
    public enum TypeOfMoves
    {
        MoveL,
        MoveJ,
        MoveP
    }

    public TypeOfMoves MoveType = TypeOfMoves.MoveJ;

    public float accel = 0.0f;
    public float vel = 0.25f;
    public float Jvel = 1.05f;
    public float time = 0.0f;
    public float zone = 0.0f;
    public float ConstantZone = 0.0f;// always 0 for Move L and J

    public string instruction = "";
    public string targetDef = "";

    private const float def_LnP_accel = 1.2f;
    private const float def_J_accel = 1.4f;

    private  float def_LnP_vel = 0.25f;
    private const float def_J_vel = 1.05f;

    private const string MoveL = "movel";
    private const string MoveJ = "movej";
    private const string MoveP = "movep";

    public Slider DefVel, DefZone, DefJointVel;//default
    public Slider SpecificVel, SpecificZone, SpecificJointVel;
    private GameObject AuxCanvas, SpecificCanvas;
    Hand targetSelect;
    public GameObject AttachedRight;

    public GameObject Targets;
    private GameObject target;

    public URController UrController;

    void Start()
    {
        device = SteamVR_Controller.Input((int)trackedObject.index);
        

        device_left = SteamVR_Controller.Input((int)trackedObject_left.index);



        Pose = new DenseVector(Robot.Values.ToArray());
        ResetDefault();
        vel = DefVel.value / 1000;
        Jvel = DefJointVel.value * Mathf.PI / 180;

        SpecificVel.value = DefVel.value;
        SpecificZone.value = DefZone.value;
        SpecificJointVel.value = DefJointVel.value;
    }

    // Update is called once per frame
    void Update()
    {
        
        targetSelect = AttachedRight.GetComponent<Hand>();
        menuAppears();
        menuDisappears();

        Pose = UrController.IK(UrController.LocalTransformMatrix(this.transform).ToRHWorld(), UrController.IKConfig);

        switch (MoveType)
        {
            case TypeOfMoves.MoveL:
                targetDef = "global " + this.name + "=" + "[" + Pose[0] + ", " + Pose[1] + ", " + Pose[2] + ", " + Pose[3] + ", " + Pose[4] + ", " + Pose[5] + "]";
                instruction = MoveL + "(" + this.name + ", a=" + accel + ", v=" + vel + ", t=" + time + ", r=" + ConstantZone + ")";
                break;

            case TypeOfMoves.MoveJ:
                targetDef = "global " + this.name + "=" + "[" + Pose[0] + ", " + Pose[1] + ", " + Pose[2] + ", " + Pose[3] + ", " + Pose[4] + ", " + Pose[5] + "]";
                instruction = MoveJ + "(" + this.name + ", a=" + accel + ", v=" + Jvel + ", t=" + time + ", r=" + ConstantZone + ")";
                break;

            case TypeOfMoves.MoveP:
                targetDef = "global " + this.name + "=" + "[" + Pose[0] + ", " + Pose[1] + ", " + Pose[2] + ", " + Pose[3] + ", " + Pose[4] + ", " + Pose[5] + "]";
                instruction = MoveP + "(" + this.name + ", a=" + accel + ", v=" + vel + ", r=" + zone + ")";
                break;

        }
    }

    public void ResetDefault()
    {
        switch (MoveType)
        {
            case TypeOfMoves.MoveL:
                accel = def_LnP_accel;
                vel = def_LnP_vel;
                targetDef = "global " + this.name + "=" + "[" + Pose[0] + ", " + Pose[1] + ", " + Pose[2] + ", " + Pose[3] + ", " + Pose[4] + ", " + Pose[5] + "]";
                instruction = MoveL + "("+ this.name +", a=" + accel + ", v=" + vel + ", t=" + time + ", r=" + zone + ")";
                break;

            case TypeOfMoves.MoveJ:
                accel = def_J_accel;
                vel = def_J_vel;
                targetDef = "global " + this.name + "=" + "[" + Pose[0] + ", " + Pose[1] + ", " + Pose[2] + ", " + Pose[3] + ", " + Pose[4] + ", " + Pose[5] + "]";
                instruction = MoveJ + "("+ this.name +", a=" + accel + ", v=" + vel + ", t=" + time + ", r=" + zone + ")";
                break;

            case TypeOfMoves.MoveP:
                accel = def_LnP_accel;
                vel = def_LnP_vel;
                targetDef = "global " + this.name + "=" + "[" + Pose[0] + ", " + Pose[1] + ", " + Pose[2] + ", " + Pose[3] + ", " + Pose[4] + ", " + Pose[5] + "]";
                instruction = MoveP + "("+ this.name+  ", a=" + accel + ", v=" + vel + ", r=" + zone + ")";
                break;

        }
    }
    
    public void SetMoveL()
    {
        MoveType = TypeOfMoves.MoveL;
        //instruction = MoveL + "([" + Pose[0] + "," + Pose[1] + "," + Pose[2] + "," + Pose[3] + "," + Pose[4] + "," + Pose[5] + "], a=" + accel + ", v=" + vel + ", t=" + time + ", r=" + zone + ")";
        //ResetDefault();
    }

    public void SetMoveJ()
    {
        MoveType = TypeOfMoves.MoveJ;
        //instruction = MoveJ + "([" + Pose[0] + "," + Pose[1] + "," + Pose[2] + "," + Pose[3] + "," + Pose[4] + "," + Pose[5] + "], a=" + accel + ", v=" + vel + ", t=" + time + ", r=" + zone + ")";
        //ResetDefault();
    }

    public void SetMoveP()
    {
        MoveType = TypeOfMoves.MoveP;
        //instruction = MoveP + "([" + Pose[0] + "," + Pose[1] + "," + Pose[2] + "," + Pose[3] + "," + Pose[4] + "," + Pose[5] + "], a=" + accel + ", v=" + vel + ", r=" + zone + ")";
        //ResetDefault();
    }

    // Insert the speed, zone & accel modifications
    public void modifyDefaultSpeed()
    {
        vel = DefVel.value/1000;
    }
    public void modifyDefaultZone()
    {
        zone = DefZone.value/100;

    }
    public void modifyDefaultJointSpeed()
    {
        Jvel = DefJointVel.value * Mathf.PI / 180;

    }
    ///*-------------------------------------------------------------
    public void modifySpecificSpeed()
    {
        vel = SpecificVel.value / 1000;
        //ResetDefault();
    }
    public void modifySpecificZone()
    {
        zone = SpecificZone.value / 100;
        //ResetDefault();

    }

    public void modifySpecificJointSpeed()
    {
        Jvel = SpecificJointVel.value * Mathf.PI / 180;
        //ResetDefault();

    }

    public void menuAppears()
    {

        string hola = this.name;
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && TargetSelected)
        {
            

                    if (hola != "FK Frame")
                    {
                        target = Targets.gameObject.transform.Find(hola).gameObject;

                        //AuxCanvas.activeSelf==true&& SpecificCanvas.activeSelf == true
                        switch (MoveType)
                        {
                            case TypeOfMoves.MoveL:
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("NameAndMove").GetComponent<Text>().text="";
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("NameAndMove").GetComponent<Text>().text=hola+" : MoveL";
                               
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("SpecVel").gameObject.SetActive(true);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("TextVel").gameObject.SetActive(true);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("tittleVel").gameObject.SetActive(true);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("SpecZone").gameObject.SetActive(false);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("TextZone").gameObject.SetActive(false);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("tittleZone").gameObject.SetActive(false);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("SpecJointVel").gameObject.SetActive(false);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("TextJvel").gameObject.SetActive(false);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("tittleJvel").gameObject.SetActive(false);

                        break;

                            case TypeOfMoves.MoveJ:
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("NameAndMove").GetComponent<Text>().text="";
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("NameAndMove").GetComponent<Text>().text=hola+" : MoveJ";

                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("SpecVel").gameObject.SetActive(false);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("TextVel").gameObject.SetActive(false);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("tittleVel").gameObject.SetActive(false);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("SpecZone").gameObject.SetActive(false);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("TextZone").gameObject.SetActive(false);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("tittleZone").gameObject.SetActive(false);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("SpecJointVel").gameObject.SetActive(true);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("TextJvel").gameObject.SetActive(true);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("tittleJvel").gameObject.SetActive(true);
                        break;

                            case TypeOfMoves.MoveP:
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("NameAndMove").GetComponent<Text>().text="";
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("NameAndMove").GetComponent<Text>().text=hola+" : MoveP";

                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("SpecVel").gameObject.SetActive(true);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("TextVel").gameObject.SetActive(true);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("tittleVel").gameObject.SetActive(true);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("SpecZone").gameObject.SetActive(true);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("TextZone").gameObject.SetActive(true);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("tittleZone").gameObject.SetActive(true);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("SpecJointVel").gameObject.SetActive(false);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("TextJvel").gameObject.SetActive(false);
                                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").transform.Find("tittleJvel").gameObject.SetActive(false);
                                break;
                         }
                    


                        target.transform.Find("Sphere").transform.Find("AuxCanvasRot").gameObject.SetActive(true);
                        //target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").gameObject.SetActive(true);
                        pointer.SetActive(true);

                        
                        //AuxCanvas.SetActive(false);
                        //SpecificCanvas.SetActive(false);
                        //pointer.SetActive(false);
                        
                     }
                




            /*
            AuxCanvas=this.gameObject.transform.Find("Sphere").transform.Find("AuxCanvasRot").gameObject;
            SpecificCanvas= this.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").gameObject;
            AuxCanvas.SetActive(true);
            SpecificCanvas.SetActive(true);
            pointer.SetActive(true);
            Debug.LogWarning("yeeeeeeeeeeeee");
            */
            
        }


    }

    public void menuDisappears()
    {
        //AuxCanvas = this.gameObject.transform.Find("Sphere").transform.Find("AuxCanvasRot").gameObject;
        //SpecificCanvas = this.gameObject.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").gameObject;
        string hola = this.name;
        Debug.LogWarning(hola);
        if (hola!= "FK Frame") {
            target = Targets.gameObject.transform.Find(hola).gameObject;
        
            //AuxCanvas.activeSelf==true&& SpecificCanvas.activeSelf == true
            if (target.transform.Find("Sphere").transform.Find("AuxCanvasRot")!= null && device_left.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            {

                target.transform.Find("Sphere").transform.Find("AuxCanvasRot").gameObject.SetActive(false);
                //target.transform.Find("Sphere").transform.Find("AuxCanvasRot").transform.Find("TargetManagement").gameObject.SetActive(false);
                pointer.SetActive(false);

                /*
                AuxCanvas.SetActive(false);
                SpecificCanvas.SetActive(false);
                pointer.SetActive(false);
                */
            }
        }
    }
   // */
}
