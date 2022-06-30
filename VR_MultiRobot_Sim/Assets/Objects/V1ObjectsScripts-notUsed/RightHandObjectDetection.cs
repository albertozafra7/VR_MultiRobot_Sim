using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandObjectDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public bool collisionDetected = false;
    public GameObject miniTable, miniConveyor, miniPallet, miniBox;
    public GameObject BoxPosition, TablePosition, ConvPosition, PalletPosition;
    

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
 
    private void OnTriggerEnter(Collider collision)
    {
        /*
        GrabbingObjectDetection grabbingObject = collision.gameObject.GetComponent<GrabbingObjectDetection>();
        if (collision.gameObject.name == "LocationTableMenu")
        {
            miniTable.GetComponent<BoxCollider>().enabled = true;
        }
        else if (collision.gameObject.name == "LocationConveyorMenu")
        {
            miniConveyor.GetComponent<BoxCollider>().enabled = true;
        }
        else if (collision.gameObject.name == "LocationPalleMenu")
        {
            miniPallet.GetComponent<BoxCollider>().enabled = true;
        }
        else if (collision.gameObject.name == "LocationBoxMenu")
        {
            miniBox.GetComponent<BoxCollider>().enabled = true;
        }
        */
       // GrabbingObjectDetection grabbingObject = collision.gameObject.GetComponent<GrabbingObjectDetection>();
        if (collision.gameObject.name == "miniTable" || collision.gameObject.name == "miniConveyor" || collision.gameObject.name == "miniPallet" || collision.gameObject.name == "miniBox")
        {
         
            if (collision.gameObject.name == "miniTable")
            {
                if(GrabbingChecker(miniTable, TablePosition))
                {
                    collisionDetected = true;
                }

            } else if (collision.gameObject.name == "miniConveyor")
            {
                if (GrabbingChecker(miniConveyor, ConvPosition))
                {
                    collisionDetected = true;
                }
            } else if (collision.gameObject.name == "miniPallet")
            {
                if (GrabbingChecker(miniPallet, PalletPosition))
                {
                    collisionDetected = true;
                }
            } else if (collision.gameObject.name == "miniBox")
            {
                if (GrabbingChecker(miniBox, BoxPosition))
                {
                    collisionDetected = true;
                }
            }
            //Debug.LogWarning("engaaaaaaaaaaaaa");
        }

    }
    /*
    private void OnTriggerExit(Collider collision)
    {
        GrabbingObjectDetection grabbingObject = collision.gameObject.GetComponent<GrabbingObjectDetection>();
        if (collision.gameObject.name == "LocationTableMenu"&& !GrabbingChecker(miniTable, TablePosition))
        {
            
            miniTable.GetComponent<BoxCollider>().enabled = false;
        }
        else if (collision.gameObject.name == "LocationConveyorMenu"&& !GrabbingChecker(miniConveyor, ConvPosition))
        {
            miniConveyor.GetComponent<BoxCollider>().enabled = false;
        }
        else if (collision.gameObject.name == "LocationPalleMenu"&& !GrabbingChecker(miniPallet, PalletPosition))
        {
            miniPallet.GetComponent<BoxCollider>().enabled = false;
        }
        else if (collision.gameObject.name == "LocationBoxMenu"&& !GrabbingChecker(miniBox, BoxPosition))
        {
            Debug.LogWarning("YEEEEEEEEEEEEEE");
            miniBox.GetComponent<BoxCollider>().enabled = false;
        }
    }
    */
    public bool GrabbingChecker(GameObject miniObject, GameObject position)
    {
        if (Mathf.Abs(miniObject.transform.position.x - position.transform.position.x) > 0.086f || Mathf.Abs(miniObject.transform.position.y - position.transform.position.y) > 0.086f || Mathf.Abs(miniObject.transform.position.z - position.transform.position.z) > 0.086f)
        {
            //Debug.LogWarning(Mathf.Abs(miniObject.transform.position.x - position.transform.position.x) + "/" + Mathf.Abs(miniObject.transform.position.y - position.transform.position.y) +"/"+ Mathf.Abs(miniObject.transform.position.z - position.transform.position.z));
            miniObject.transform.parent = GameObject.Find("MiniObjAux").transform;
            //Debug.LogWarning("TRUEEEEEEEEEEE");//cpment
            return true;
        }
        else
        {
            //Debug.LogWarning("FALSEEEEEEEEEEEE");
            //Debug.LogWarning("ojooooooooo"+Mathf.Abs(miniObject.transform.position.x - position.transform.position.x) + "/" + Mathf.Abs(miniObject.transform.position.y - position.transform.position.y) + "/" + Mathf.Abs(miniObject.transform.position.z - position.transform.position.z));
            //Debug.LogWarning(Mathf.Abs(miniObject.transform.position.x )+ "/"+ (Mathf.Abs(position.transform.position.x)));
            return false;
        }
    }

}
