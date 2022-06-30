using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Box, Conveyor, Pallet, Table;
    public int numConveyor=0, numBox=0, numPallet=0, numTable=0;
    public bool CheckConveyor = false, CheckBox = false, CheckPallet = false, CheckTable = false;
    public bool duplicationAvoidance = true;// evita q se duplique objeto
    public GameObject RightPalm;
    //public GameObject duplicate;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.LogWarning("collisionnnnnnnnnnnnnnnn");
        RightHandObjectDetection rightHandObject = RightPalm.GetComponent<RightHandObjectDetection>();
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if ((collision.gameObject.tag == "floor" || collision.gameObject.tag == "Conveyor" || collision.gameObject.tag == "Box" || collision.gameObject.tag == "Table" || collision.gameObject.tag == "Pallet")&&rightHandObject.collisionDetected)
        {
            //Debug.LogWarning("iffffffffffffffffff");
            if (this.name == "miniConveyor")
            {
                GameObject duplicate = Instantiate(Conveyor);
                if (numConveyor==0) {
                    duplicate.name = "Conveyor";
                }
                else
                {
                    duplicate.name = "Conveyor"+"("+numConveyor+")";
                }
                numConveyor++;
                duplicate.transform.parent = GameObject.Find("CreatedObjects").transform;//put the new objects in the empty object Targets

                duplicate.transform.position = new Vector3(this.transform.position.x, this.transform.position.y+ 0.35f, this.transform.position.z);
                CheckConveyor = true;
                Rigidbody rigidbody = duplicate.GetComponent<Rigidbody>();
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                
                duplicate.SetActive(true);
                rightHandObject.collisionDetected = false;

                //duplicationAvoidance = false;
                //Debug.LogWarning("1111111111111");

            }
            else if (this.name == "miniTable")
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
                duplicate.transform.parent = GameObject.Find("CreatedObjects").transform;//put the new objects in the empty object Targets

                duplicate.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.25f, this.transform.position.z);
                CheckTable = true;
                Rigidbody rigidbody = duplicate.GetComponent<Rigidbody>();
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;

                duplicate.SetActive(true);
                rightHandObject.collisionDetected = false;

                //duplicationAvoidance = false;
               // Debug.LogWarning("222222222222222");
            }
            else if (this.name == "miniBox")
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
                duplicate.transform.parent = GameObject.Find("CreatedObjects").transform;//put the new objects in the empty object Targets

                duplicate.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.155f, this.transform.position.z);
                CheckBox = true;
                Rigidbody rigidbody = duplicate.GetComponent<Rigidbody>();
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;

                duplicate.SetActive(true);
                rightHandObject.collisionDetected = false;
                //duplicationAvoidance = false;
                //Debug.LogWarning("3333333333333");


            }
            else if (this.name == "miniPallet")
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
                duplicate.transform.parent = GameObject.Find("CreatedObjects").transform;//put the new objects in the empty object Targets

                duplicate.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.065f, this.transform.position.z);
                CheckPallet = true;
                Rigidbody rigidbody = duplicate.GetComponent<Rigidbody>();
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;

                duplicate.SetActive(true);
                rightHandObject.collisionDetected = false;

                //duplicationAvoidance = false;
                //Debug.LogWarning("444444444444444");
            }
            else
            {
                //duplicationAvoidance = true;
                CheckTable = false;
                CheckPallet = false;
                CheckConveyor = false;
                CheckBox = false;
            }
            
        }


    }
}
