using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreationManager : MonoBehaviour
{
    [Header("Clone Objects")]
    public List<GameObject> ObjectsToClone;
    public List<int> ObjectsCloned;
    private GameObject ObjectSelected;
    private bool cloning = false;
    
    [Header("Parent")]
    public Transform ObjectClonedParent;

    [Header("Visuals")]
    public Transform pointToFollow;
    private PointableObject_Custom pointableObject;

    [Header("Management")]
    public RobotSelection RobotManager;


    // Start is called before the first frame update
    void Start()
    {
        int i = 0;

        foreach (GameObject objectClone in ObjectsToClone)
        {
            ObjectsCloned[i] = 1;
            i++;
            if(objectClone.activeSelf)
                objectClone.SetActive(false);
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cloning){
            //SetAllObjectsRigidBody(true);
            ObjectSelected.transform.parent.position = pointToFollow.position;
            if(!ObjectSelected.activeSelf)
                ObjectSelected.SetActive(true);

            if(ObjectSelected.GetComponent<PositionVerifier>().is_feasible_pos){

                if(ObjectSelected.tag == "RobotMenu"){
                    pointableObject = FindRobotChild(ObjectSelected).GetComponent<PointableObject_Custom>();
                }
                else if (ObjectSelected.tag == "ObjectMenu")
                    pointableObject = ObjectSelected.GetComponent<PointableObject_Custom>();

                pointableObject.RestoreColor();
                pointableObject.ApplyColorMask(pointableObject.HoverColor_Creation);

                if(OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) >= 0.8f)
                    PlaceObjectCopy(ObjectSelected);

            } else {
                if(ObjectSelected.tag == "RobotMenu")
                    pointableObject = FindRobotChild(ObjectSelected).GetComponent<PointableObject_Custom>();
                else if (ObjectSelected.tag == "RobotMenu")
                    pointableObject = ObjectSelected.GetComponent<PointableObject_Custom>();

                pointableObject.RestoreColor();
                pointableObject.ApplyColorMask(pointableObject.HoverColor_Deletion);
            }
            

        } else {
            SetAllObjectsRigidBody(true);
            //SetAllObjectsRigidBody(false);
            if(ObjectSelected != null && ObjectSelected.activeSelf)
                ObjectSelected.SetActive(false);
        }
    }

    public GameObject FindRobotChild(GameObject RobotParent){
        for(int i = 0; i < RobotParent.transform.childCount; i++){
          if(RobotParent.transform.GetChild(i).gameObject.tag == "Robot" || RobotParent.transform.GetChild(i).gameObject.tag == "RobotMenu"){
            return RobotParent.transform.GetChild(i).gameObject;
          } 
        }

        return null;
    }

    public void PlaceObjectCopy(GameObject ObjectToPlace){
        GameObject duplicate = Instantiate(ObjectToPlace);//the object is duplicated
 
        duplicate.name = ObjectToPlace.name + "(" + ObjectsCloned[FindObjectIndex(ObjectToPlace.name)] + ")";
        ObjectsCloned[FindObjectIndex(ObjectToPlace.name)]++;
        duplicate.transform.parent = ObjectClonedParent;
        if(ObjectToPlace.tag == "RobotMenu"){
            duplicate.tag = "RobotEssential";
            FindRobotChild(duplicate).tag = "Robot";
            Destroy(duplicate.GetComponent<BoxCollider>());
            RobotManager.UpdateRobotList();
        } else if(ObjectToPlace.tag == "ObjectMenu"){
            duplicate.tag = "Object";
        }
        
        duplicate.transform.position = ObjectToPlace.transform.position;//place it in the ghost object position
        duplicate.transform.position = ObjectToPlace.transform.position;

        
        if(ObjectSelected.tag == "RobotMenu"){
            PointableObject_Custom ObjectToRestoreColor = FindRobotChild(duplicate).GetComponent<PointableObject_Custom>();
            ObjectToRestoreColor.RestoreColor();
        } else if (ObjectSelected.tag == "ObjectMenu"){
            PointableObject_Custom ObjectToRestoreColor = duplicate.GetComponent<PointableObject_Custom>();
            //Debug.LogError("Tiene " + ObjectToPlace.GetComponent<PointableObject_Custom>().getOriginalColors().Count + " Original Colors");
            ObjectToRestoreColor.setOriginalColors(new Queue<Color>(ObjectToPlace.GetComponent<PointableObject_Custom>().getOriginalColors()));
            ObjectToRestoreColor.RestoreColor();
            //duplicate.GetComponent<Rigidbody>().isKinematic  = false;
            //duplicate.GetComponent<Rigidbody>().useGravity = true;
        }

        duplicate.SetActive(true);//the created object is showed

        duplicate.layer = LayerMask.NameToLayer("Default");
        ChangeLayersRecursively(duplicate.transform,"Default");
    }

    public int FindObjectIndex(string ObjectName){
        for(int i = 0; i < ObjectsToClone.Count; i++){
            if(ObjectsToClone[i].name == ObjectName)
                return i;
        }
        return 0;
    }

    public void SetAllObjectsRigidBody(bool kinematic){
        GameObject ObjectManager = GameObject.Find("/Objects");
        

        if(ObjectManager != null){
            for(int i = 0; i < ObjectManager.transform.childCount; i++){
                GameObject ChildObject = ObjectManager.transform.GetChild(i).gameObject;
                if(ChildObject.gameObject.tag == "Object"){
                    if(kinematic){
                        ChildObject.GetComponent<Rigidbody>().isKinematic  = true;
                        ChildObject.GetComponent<Rigidbody>().useGravity = false;
                        ChildObject.GetComponent<Rigidbody>().constraints =  RigidbodyConstraints.None;
                    } else {
                        ChildObject.GetComponent<Rigidbody>().isKinematic  = false;
                        ChildObject.GetComponent<Rigidbody>().useGravity = true;
                        ChildObject.GetComponent<Rigidbody>().constraints =  RigidbodyConstraints.FreezeAll;
                    }
                } 
            }
        }
    }

    public void SetupCloneMode(GameObject DesiredObject){
        if(ObjectSelected != null && ObjectSelected.activeSelf)
            ObjectSelected.SetActive(false);

        ObjectSelected = DesiredObject;
        cloning = true;
        //SetAllObjectsRigidBody(true);
        //Debug.LogError("Setup Mode Invoked");

    }

    public void StopCloning(){
        //SetAllObjectsRigidBody(false);
        if(ObjectSelected.activeSelf)
            ObjectSelected.SetActive(false);
        cloning = false;
    }

    public void ManageActivity(){
        if(cloning){
            StopCloning();
            RobotManager.setMode2None();
        }
    }

    public void ChangeLayersRecursively(Transform trans,string name)
    {
        foreach (Transform child  in trans)
        {
            child.gameObject.layer = LayerMask.NameToLayer(name);
            ChangeLayersRecursively(child, name);
        }
    }

    public void setCloning(bool value){
        cloning = value;
    }
}
