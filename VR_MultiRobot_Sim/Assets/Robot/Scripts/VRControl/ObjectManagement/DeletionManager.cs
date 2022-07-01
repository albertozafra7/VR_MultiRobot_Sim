using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletionManager : MonoBehaviour
{

    [Header("Robot Selection Manager")]
    public RobotSelection RobotSelectionManager;

    private GameObject DeletionObject;
    private GameObject[] DeletionArray;

    public void DeleteAllObjects(){
       DeletionArray = GameObject.FindGameObjectsWithTag("Object");
       foreach (GameObject DeletionObject in DeletionArray)
            Destroy(DeletionObject);
    }

    public void DeleteAllRobots(){
        DeletionArray = GameObject.FindGameObjectsWithTag("RobotEssential");
        foreach (GameObject DeletionObject in DeletionArray){
            DeletionObject.transform.parent = null;
            // We delete the robot
            Destroy(DeletionObject);
        }


    }

    public void DeleteAllMenus(){
       DeletionArray = GameObject.FindGameObjectsWithTag("Menu");
       foreach (GameObject DeletionObject in DeletionArray)
            Destroy(DeletionObject);
    }
}
