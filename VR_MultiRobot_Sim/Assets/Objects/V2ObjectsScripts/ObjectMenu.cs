using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this script is placed in each object
//this script contains the differents functions used for the object menu placed in each object

public class ObjectMenu : MonoBehaviour
{
    public GameObject AuxParentObjectMenu, Objectmenu;
    public GameObject ObjectEdgesTracker;//it is a visual used for show that the object is being edited
    private GameObject duplicate;//for the object menu option of duplicating the object
    public GameObject laser;
    private bool moveDuplicateObject = false;
    private Laser laserscript;
    private ObjectShowedAndCreate ObjectShowed;
    public GameObject indexFinger;


    private GameObject TableMenuInicPos, PalletMenuInicPos, BoxMenuInicPos, ConveyorMenuInicPos;//for storing the initial rotation of the object menus in each object
    [Header("Objects")]


    private bool RotateAndMovepressed = false;//true when rotate object is pressed in objectmenu
    public GameObject Rotate90button, ResetRotationButton;//for show and hide the rotate buttons
    private bool FirstInteractionTransl = false;
    private GameObject InitRotAndPos;


    //Redimensionate
    private bool RedimensionateBool = false;//true when redimensionate object is pressed in objectmenu

    //for untoggle
    public GameObject RedimensionateMenuButton;
    public GameObject RotateAndMOveMenuButton;

    void Start()
    {
        //DuplicateObject
        duplicate = new GameObject("Duplicate");

        laserscript = laser.GetComponent<Laser>();
        ObjectShowed = indexFinger.GetComponent<ObjectShowedAndCreate>();
        TableMenuInicPos = new GameObject("TableMenuInicPos");
        PalletMenuInicPos = new GameObject("PalletMenuInicPos");
        BoxMenuInicPos = new GameObject("BoxMenuInicPos");
        ConveyorMenuInicPos = new GameObject("ConveyorMenuInicPos");

        //the initial position of the object menu is stored
        TableMenuInicPos.transform.rotation = new Quaternion(0f, 0f, 0f, 1f);
        PalletMenuInicPos.transform.rotation = new Quaternion(-30f, 0f, 0f, 0f);
        BoxMenuInicPos.transform.rotation = new Quaternion(-30f, 90f, 0f, 0f);
        ConveyorMenuInicPos.transform.rotation = new Quaternion(59f, 0f, 0f, 0f);

        //MoveObject

        InitRotAndPos = new GameObject("InitRotAndPos");
        //InitRotAndPos.transform.parent = GeneratedGameobjectsAux.transform;

        //RotateObject
        //offsetSet = false;
        Rotate90button.SetActive(false);
        ResetRotationButton.SetActive(false);




    }

    // Update is called once per frame
    void Update()
    {
        ////////////////////////////////////////////Duplicate//////////////////////////////////////////////
        if (moveDuplicateObject && ObjectShowed.PinchDetected)//if the duplicate button has been pressed and the pich gesture is detected
        {
            PlaceDuplicateObject();//the duplicate object is placed
        }
        if (moveDuplicateObject)
        {//if the duplicate button has been pressed the duplicated object is moved with the raycast (until the pich gesture is detected)

            //the y component is different in each because the objects have different height
            if (this.tag == "Conveyor")
            {
                duplicate.transform.position = new Vector3(laserscript.hitpoint.transform.position.x, laserscript.hitpoint.transform.position.y + 0.35f, laserscript.hitpoint.transform.position.z);//equals the dulpicated object position to where the raycast is poitiong
            }
            else if (this.tag == "Box")
            {
                duplicate.transform.position = new Vector3(laserscript.hitpoint.transform.position.x, laserscript.hitpoint.transform.position.y + 0.155f, laserscript.hitpoint.transform.position.z);//equals the dulpicated object position to where the raycast is poitiong
            }
            else if (this.tag == "Pallet")
            {
                duplicate.transform.position = new Vector3(laserscript.hitpoint.transform.position.x, laserscript.hitpoint.transform.position.y + +0.065f, laserscript.hitpoint.transform.position.z);//equals the dulpicated object position to where the raycast is poitiong
            }
            else if (this.tag == "Table")
            {
                duplicate.transform.position = new Vector3(laserscript.hitpoint.transform.position.x, laserscript.hitpoint.transform.position.y + +0.25f, laserscript.hitpoint.transform.position.z);//equals the dulpicated object position to where the raycast is poitiong
            }
        }


        ////////////////////////////////////////////MoveAndRotateObjects//////////////////////////////////////////////

        if (RotateAndMovepressed)//if move and rotate is pressed
        {
            //BlockObjectRotation();
            BlockObjectTranslation();//transaltion in Y axe is blocked 
        }
        else
        {
            FirstInteractionTransl = true;
        }

        ////////////////////////////////////////////Redimensionate//////////////////////////////////////////////
        if (RedimensionateBool)//if redimensionate button is pressed
        {
            Objectmenu.transform.position = new Vector3(this.transform.position.x, Objectmenu.transform.position.y, this.transform.position.z);//the object menu father is not the object anymore so its position is equal to the object so that they move together
        }
        else
        {
        }

    }

    public void DeleteObject()//is called when delete object is pressed
    {
        Destroy(this.gameObject);
    }

    public void DuplicateObject()// is called when duplicate object is pressed
    {
        UntoggleMoveAndRedimensionate();
        if (!moveDuplicateObject)//if movdeSuplicateObject has not been already pressed
        {
            RotateAndMoveObject(false);
            RedimensionateObject(false);

            duplicate = Instantiate(this.gameObject);//the object is suplicated
            duplicate.transform.Find("ObjectMenu").gameObject.SetActive(false);//the objectmenu is hidden

            if (this.gameObject.tag == "Table")//the objectmenu is reset to its inital rotation
            {
                duplicate.transform.Find("ObjectMenu").gameObject.transform.rotation = TableMenuInicPos.transform.rotation;
            }
            else if (this.gameObject.tag == "Box")
            {
                duplicate.transform.Find("ObjectMenu").gameObject.transform.rotation = BoxMenuInicPos.transform.rotation;

            }
            else if (this.gameObject.tag == "Pallet")
            {
                duplicate.transform.Find("ObjectMenu").gameObject.transform.rotation = PalletMenuInicPos.transform.rotation;

            }
            else if (this.gameObject.tag == "Conveyor")
            {
                duplicate.transform.Find("ObjectMenu").gameObject.transform.rotation = ConveyorMenuInicPos.transform.rotation;

            }

            BoxCollider boxCollider = duplicate.GetComponent<BoxCollider>();
            boxCollider.enabled = false;//box collider disabled while the duplicated object is being moved with the ray cast

            Rigidbody rigidbody = duplicate.GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;//gravity disabled while the duplicated object is being moved with the ray cast

            duplicate.name = this.name + "(Duplicate)";

            duplicate.transform.parent = GameObject.Find("CreatedObjects").transform;//put the new objects in the empty object CreatedObjects
            duplicate.transform.rotation = this.transform.rotation;
            laser.SetActive(true);
            moveDuplicateObject = true;
        }
    }

    public void PlaceDuplicateObject()//duplicated obect stops following the raycast and is placed
    {
        BoxCollider boxCollider = duplicate.GetComponent<BoxCollider>();
        boxCollider.enabled = true;

        Rigidbody rigidbody = duplicate.GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;

        laser.SetActive(false);
        moveDuplicateObject = false;


    }
    public void RotateAndMoveObject(bool toggle)//is scalled when rotate object is pressed
    {
        RotateAndMovepressed = toggle;//true when rotate object pressed
        Rotate90button.SetActive(toggle);
        ResetRotationButton.SetActive(toggle);
        if (RedimensionateBool || RotateAndMovepressed)//if before pressing move and rotate redimensionate was pressed
        {
            //this.gameObject.GetComponent<LeapRTS>().enabled = true;//the LeapRTS script is enabled, which controls the movement, rotationd and redimensioante
            ObjectEdgesTracker.SetActive(true);


        }
        else
        {
            ObjectEdgesTracker.SetActive(false);

            //this.gameObject.GetComponent<LeapRTS>().enabled = false;

        }
        //this.gameObject.GetComponent<LeapRTS>().RotateRedimensionate = toggle;

        gameObject.GetComponent<Rigidbody>().isKinematic = toggle;
        gameObject.GetComponent<Rigidbody>().useGravity = !toggle;

        if (toggle)
        {
            RedimensionateObject(false);

        }



    }

    public void Rotate90Object()// is called when rotate 90 button is pressed
    {
        if (this.gameObject.tag == "Conveyor")
            this.transform.rotation *= Quaternion.Euler(0, 0, 90);
        else
            this.transform.rotation *= Quaternion.Euler(0, 90, 0);

        //Debug.LogWarning("holiiii");

    }

    public void ResetRotationObject()// is called when reset rotation button is pressed
    {

        if (this.gameObject.tag == "Conveyor")
            this.transform.rotation = Quaternion.Euler(-90, 0, 0);
        else
            this.transform.rotation = Quaternion.Euler(0, 0, 0);

        //Debug.LogWarning("yeeee");

    }

    public void BlockObjectTranslation()//blocks Y transaltion
    {
        if (true)
        {
            if (FirstInteractionTransl)
            {
                InitRotAndPos.transform.position = this.gameObject.transform.position;
                FirstInteractionTransl = false;

            }
            {
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, InitRotAndPos.transform.position.y, this.gameObject.transform.position.z);
            }
        }
        else
        {
        }

    }


    public void RedimensionateObject(bool toggle)
    {
        RedimensionateBool = toggle;

        if (RedimensionateBool || RotateAndMovepressed)//if before pressing  redimensionate move and rotate was pressed
        {
            //this.gameObject.GetComponent<LeapRTS>().enabled = true;//the LeapRTS script is enabled, which controls the movement, rotationd and redimensioante
            ObjectEdgesTracker.SetActive(true);


        }
        else
        {
            //this.gameObject.GetComponent<LeapRTS>().enabled = false;
            ObjectEdgesTracker.SetActive(false);


        }

        //this.gameObject.GetComponent<LeapRTS>()._allowScale = toggle;

        if (toggle)
        {
            RotateAndMoveObject(false);

            Objectmenu.transform.parent = AuxParentObjectMenu.transform;//the object menu parent is not the object anymore due to if the parent stills being the object the objectmenu will be redimensionated as well as the object

        }
        else
        {
            Objectmenu.transform.parent = this.transform;
        }
    }

    public void ObjectMenuBackToParent()//object menu parent is again th eobject
    {
        Objectmenu.transform.parent = this.transform;

    }

    public void UntoggleMoveAndRedimensionate()
    {
        //RedimensionateMenuButton.GetComponent<InteractionToggle>().Untoggle();
        //RotateAndMOveMenuButton.GetComponent<InteractionToggle>().Untoggle();

    }
}
