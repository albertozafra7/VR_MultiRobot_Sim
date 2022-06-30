using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbingObjectDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject BoxPosition, TablePosition, ConvPosition, PalletPosition;
    public GameObject miniBox, miniTable, miniConveyor, miniPallet;
    public GameObject miniConveyorRotation;//this is needed because the rotation script and the colaiders etc are not in the same object
    public GameObject miniObjectsMenu;


    //public GameObject Box, Table, Conveyor, Pallet;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GrabbingChecker(miniPallet,PalletPosition)) //comprueba pallet cogido
        {
            GravitykinematicSwitch(miniPallet, true);
            RotationToggle(miniPallet, false);
        }
        if (GrabbingChecker(miniTable, TablePosition)) //comprueba pallet cogido
        {
            GravitykinematicSwitch(miniTable, true);
            RotationToggle(miniTable, false);
        }
        if (GrabbingChecker(miniBox, BoxPosition)) //comprueba pallet cogido
        {
            GravitykinematicSwitch(miniBox, true);
            RotationToggle(miniBox, false);
        }
        if (GrabbingChecker(miniConveyor, ConvPosition)) //comprueba pallet cogido
        {
            GravitykinematicSwitch(miniConveyor, true);
            RotationToggle(miniConveyorRotation, false);
        }

    }



    
    public bool GrabbingChecker(GameObject miniObject, GameObject position)
    {
        if(Mathf.Abs(miniObject.transform.position.x - position.transform.position.x) > 0.086 || Mathf.Abs(miniObject.transform.position.y - position.transform.position.y) > 0.086 || Mathf.Abs(miniObject.transform.position.z - position.transform.position.z) > 0.086)
        {
            miniObject.transform.parent = GameObject.Find("MiniObjAux").transform;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void GravitykinematicSwitch(GameObject miniObject, bool toggle)//when toggle true gravity true
    {
        Rigidbody rigidbody = miniObject.GetComponent<Rigidbody>();
        if (toggle)//cd esta cogido toggle ponerlo a true
        {
            
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;

        }
        else//cd esta en el menu toggle ponerlo a false
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

        }
    }

    public void RotationToggle(GameObject miniObject, bool toggle)
    {
        ObjectRotation objectRotation = miniObject.GetComponent<ObjectRotation>();
        
        if (toggle)
        {
            objectRotation.enabled = true;

        }
        else
        {

            objectRotation.enabled = false;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if ((collision.gameObject.tag == "floor" || collision.gameObject.tag == "Conveyor" || collision.gameObject.tag == "Box" || collision.gameObject.tag == "Table" || collision.gameObject.tag == "Pallet")&&(GrabbingChecker(miniPallet, PalletPosition) || GrabbingChecker(miniTable, TablePosition) || GrabbingChecker(miniBox, BoxPosition) || GrabbingChecker(miniConveyor, ConvPosition)))////////////////////////////////////////////////////
        {
            if(this.name == "miniConveyor")
            {
                ObjectCreation objectCreation = this.GetComponent<ObjectCreation>();
                if (objectCreation.CheckConveyor) {
                    miniConveyor.transform.parent = miniObjectsMenu.transform;
                    GravitykinematicSwitch(miniConveyor, false);
                    //miniConveyor.GetComponent<BoxCollider>().enabled = false;////////////////
                    miniConveyor.transform.position = ConvPosition.transform.position;
                    miniConveyor.transform.rotation = ConvPosition.transform.rotation;
                    RotationToggle(miniConveyorRotation, true);
                    objectCreation.CheckConveyor = false;
                }
            }
            else if(this.name== "miniTable")
            {
                ObjectCreation objectCreation = this.GetComponent<ObjectCreation>();
                if (objectCreation.CheckTable) {
                    miniTable.transform.parent = miniObjectsMenu.transform;
                    GravitykinematicSwitch(miniTable, false);
                    //miniTable.GetComponent<BoxCollider>().enabled = false;////////////////
                    miniTable.transform.position = TablePosition.transform.position;
                    miniTable.transform.rotation = TablePosition.transform.rotation;
                    RotationToggle(miniTable, true);
                    objectCreation.CheckConveyor = false;
                }
            }
            else if(this.name== "miniBox")
            {
                ObjectCreation objectCreation = this.GetComponent<ObjectCreation>();
                if (objectCreation.CheckBox) {
                    miniBox.transform.parent = miniObjectsMenu.transform;
                    GravitykinematicSwitch(miniBox, false);
                    //miniBox.GetComponent<BoxCollider>().enabled = false;////////////////
                    miniBox.transform.position = BoxPosition.transform.position;
                    miniBox.transform.rotation = BoxPosition.transform.rotation;
                    RotationToggle(miniBox, true);
                    objectCreation.CheckBox = false;
                }

            }
            else if (this.name == "miniPallet")
            {
                ObjectCreation objectCreation = this.GetComponent<ObjectCreation>();
                if (objectCreation.CheckPallet) {
                    miniPallet.transform.parent = miniObjectsMenu.transform;
                    GravitykinematicSwitch(miniPallet, false);
                    //miniPallet.GetComponent<BoxCollider>().enabled = false;////////////////
                    miniPallet.transform.position = PalletPosition.transform.position;
                    miniPallet.transform.rotation = PalletPosition.transform.rotation;
                    RotationToggle(miniPallet, true);
                    objectCreation.CheckPallet = false;
                }
            }
        }else if((collision.gameObject.tag == "floor" || collision.gameObject.tag == "Conveyor" || collision.gameObject.tag == "Box" || collision.gameObject.tag == "Table" || collision.gameObject.tag == "Pallet") && !(GrabbingChecker(miniPallet, PalletPosition) || GrabbingChecker(miniTable, TablePosition) || GrabbingChecker(miniBox, BoxPosition) || GrabbingChecker(miniConveyor, ConvPosition)))
        {
            Physics.IgnoreCollision(this.GetComponent<BoxCollider>(), collision.gameObject.GetComponent<BoxCollider>(), true);
            Debug.LogWarning("lele");
        }

    }

    void OnCollisionExit(Collision collision)
    {
        if ((collision.gameObject.tag == "floor" || collision.gameObject.tag == "Conveyor" || collision.gameObject.tag == "Box" || collision.gameObject.tag == "Table" || collision.gameObject.tag == "Pallet") && (GrabbingChecker(miniPallet, PalletPosition) || GrabbingChecker(miniTable, TablePosition) || GrabbingChecker(miniBox, BoxPosition) || GrabbingChecker(miniConveyor, ConvPosition)))
        {
            Physics.IgnoreCollision(this.GetComponent<BoxCollider>(), collision.gameObject.GetComponent<BoxCollider>(), false);
            Debug.LogWarning("lolo");
        }
    }

}
