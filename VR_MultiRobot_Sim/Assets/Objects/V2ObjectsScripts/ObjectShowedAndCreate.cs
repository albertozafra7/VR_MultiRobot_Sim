using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// This script is placed in the right hand index. It contains the collision detection fot the collider place in the right hand index.
//The main functionality of this script is the creation and placement of  objects
public class ObjectShowedAndCreate : MonoBehaviour
{
    //enum for selecting the object that will be created
    enum minis
    {
        miniConveyor,
        miniBox,
        miniTable,
        miniPallet,
        nothing
    };
    public GameObject laser;//raycast on the right hand
    public GameObject GhostTable, GhostBox, GhostPallet, GhostConveyor;//ghost objects that aapears at the end of the raycast for showing the position where the object will be created
    public GameObject Box, Pallet, Table, Conveyor;//objects to duplicate and place where the ghost objects are
    public GameObject GhostObjects;
    private Laser laserscript;
    public int numConveyor = 0, numBox = 0, numPallet = 0, numTable = 0;
    minis miniObjects;
    public GameObject miniMenu;
    public bool PinchDetected;//true if pinch gesture is detected
    public bool IndexPointingDetected = false;//true if Index pointing gesture is detected
    static public bool targetCollision = false;
    static public GameObject touchedTarget;
    void Start()
    {
        laserscript = laser.GetComponent<Laser>();
        miniObjects = minis.nothing;
    }
    void Update()
    {
        if (laser.activeSelf)
            GhostObjects.transform.position = laserscript.hitpoint.transform.position;
    }
    private void OnTriggerEnter(Collider collision)//right hand index finger collider collisions
    {
        if (collision.gameObject.name == "Sphere")//if it collides with one target
        {
            touchedTarget = collision.gameObject;
            targetCollision = true;
        }
        if ((collision.gameObject.name == "miniconveyor" || collision.gameObject.name == "miniBox" || collision.gameObject.name == "miniTable" || collision.gameObject.name == "miniPallet"))//if it collides with one of the mini objects showed in the menu
        {
            laser.SetActive(true);//raycast active
            if (collision.gameObject.name == "miniconveyor")//if the collision has been with the miniconveyor the ghost object showed is the conveyor
            {
                miniObjects = minis.miniConveyor;
                GhostBox.SetActive(false);
                GhostConveyor.SetActive(true);
                GhostTable.SetActive(false);
                GhostPallet.SetActive(false);
            }
            else if (collision.gameObject.name == "miniBox")//if the collision has been with the miniBox the ghost object showed is the box
            {
                miniObjects = minis.miniBox;
                GhostBox.SetActive(true);
                GhostConveyor.SetActive(false);
                GhostTable.SetActive(false);
                GhostPallet.SetActive(false);
            }
            else if (collision.gameObject.name == "miniTable")//if the collision has been with the miniTable the ghost object showed is the table
            {
                miniObjects = minis.miniTable;
                GhostBox.SetActive(false);
                GhostConveyor.SetActive(false);
                GhostTable.SetActive(true);
                GhostPallet.SetActive(false);
            }
            else if (collision.gameObject.name == "miniPallet")//if the collision has been with the miniPallet the ghost object showed is the pallet
            {
                miniObjects = minis.miniPallet;
                GhostBox.SetActive(false);
                GhostConveyor.SetActive(false);
                GhostTable.SetActive(false);
                GhostPallet.SetActive(true);
            }
        }
        //object menu activation and desactivation
        if ((collision.gameObject.tag == "Table" || collision.gameObject.tag == "Pallet" || collision.gameObject.tag == "Conveyor" || collision.gameObject.tag == "Box") && IndexPointingDetected)//if the index finger collides with one created object
        {
            GameObject aux;
            collision.gameObject.GetComponent<ObjectMenu>().ObjectMenuBackToParent();
            aux = collision.gameObject.transform.Find("ObjectMenu").gameObject;
            if (aux.activeSelf == false)//if the objectmenu is active it will be desactivated, if it is not active the other way around
            {
                aux.SetActive(true);
                ButtonUntoggeled();
            }
            else
            {
                ObjectMenu aux2 = collision.gameObject.GetComponent<ObjectMenu>();
                aux2.UntoggleMoveAndRedimensionate();
                aux2.RotateAndMoveObject(false);
                aux2.RedimensionateObject(false);
                aux.SetActive(false);
            }
        }
    }
    public void ButtonRotation()// not yet used function - the idea is to put one button for rotating the object before placing it
    {
        if (miniObjects == minis.miniConveyor)
        {
            GhostConveyor.transform.rotation *= Quaternion.AngleAxis(90, Vector3.up);
        }
        else if (miniObjects == minis.miniBox)
        {
            GhostBox.transform.rotation *= Quaternion.AngleAxis(90, Vector3.up);
        }
        else if (miniObjects == minis.miniTable)
        {
            GhostTable.transform.rotation *= Quaternion.AngleAxis(90, Vector3.up);
        }
        else if (miniObjects == minis.miniPallet)
        {
            GhostPallet.transform.rotation *= Quaternion.AngleAxis(90, Vector3.up);
        }
    }
    public void ButtonUntoggeled()//it is called when the object menu button is unpressed
    {
        laser.SetActive(false);
        GhostBox.SetActive(false);
        GhostConveyor.SetActive(false);
        GhostTable.SetActive(false);
        GhostPallet.SetActive(false);
        miniObjects = minis.nothing;
    }
    //for duplicating and placing the objects
    public void pinchDetectedTrue()//it is called when the pinch gesture is detected
    {
        if (miniObjects != minis.nothing)//if one object has been selected (it is not nothing)
        {
            if (miniObjects == minis.miniConveyor)//depending of the selected object
            {
                GameObject duplicate = Instantiate(Conveyor);//the object is duplicated
                if (numConveyor == 0)//the name of the object is changed
                {
                    duplicate.name = "Conveyor";
                }
                else
                {
                    duplicate.name = "Conveyor" + "(" + numConveyor + ")";
                }
                numConveyor++;
                duplicate.transform.parent = GameObject.Find("CreatedObjects").transform;//put the new objects in the empty object Targets
                duplicate.transform.position = GhostConveyor.transform.position;//place it in the ghost object position
                duplicate.transform.position = GhostConveyor.transform.position;
                Rigidbody rigidbody = duplicate.GetComponent<Rigidbody>();
                rigidbody.isKinematic = false;//activates the physics of the create dobject
                rigidbody.useGravity = true;
                duplicate.SetActive(true);//the created object is showed
            }
            else if (miniObjects == minis.miniBox)
            {
                GameObject duplicate = Instantiate(Box);
                if (numBox == 0)
                {
                    duplicate.name = "Box";
                }
                else
                {
                    duplicate.name = "Box" + "(" + numBox + ")";
                }
                numBox++;
                duplicate.transform.parent = GameObject.Find("CreatedObjects").transform;
                duplicate.transform.position = GhostBox.transform.position;
                duplicate.transform.position = GhostBox.transform.position;
                Rigidbody rigidbody = duplicate.GetComponent<Rigidbody>();
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                duplicate.SetActive(true);
            }
            else if (miniObjects == minis.miniTable)
            {
                GameObject duplicate = Instantiate(Table);
                if (numTable == 0)
                {
                    duplicate.name = "Table";
                }
                else
                {
                    duplicate.name = "Table" + "(" + numTable + ")";
                }
                numTable++;
                duplicate.transform.parent = GameObject.Find("CreatedObjects").transform;
                duplicate.transform.position = GhostTable.transform.position;
                duplicate.transform.position = GhostTable.transform.position;
                Rigidbody rigidbody = duplicate.GetComponent<Rigidbody>();
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                duplicate.SetActive(true);
            }
            else if (miniObjects == minis.miniPallet)
            {
                GameObject duplicate = Instantiate(Pallet);
                if (numPallet == 0)
                {
                    duplicate.name = "Pallet";
                }
                else
                {
                    duplicate.name = "Pallet" + "(" + numPallet + ")";
                }
                numPallet++;
                duplicate.transform.parent = GameObject.Find("CreatedObjects").transform;
                duplicate.transform.position = GhostPallet.transform.position;
                duplicate.transform.position = GhostPallet.transform.position;
                Rigidbody rigidbody = duplicate.GetComponent<Rigidbody>();
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                duplicate.SetActive(true);
            }
        }
    }
    public void PichBoolTrue()//true when pinch gesture detected
    {
        PinchDetected = true;
    }
    public void PichBoolFalse()//false when pinch gesture not detected
    {
        PinchDetected = false;
    }
    public void IndexPointingTrue()//true when right index finger poiting detected
    {
        IndexPointingDetected = true;
    }
    public void IndexPointingFalse()//false when right index finger poiting not detected
    {
        IndexPointingDetected = false;
    }
}