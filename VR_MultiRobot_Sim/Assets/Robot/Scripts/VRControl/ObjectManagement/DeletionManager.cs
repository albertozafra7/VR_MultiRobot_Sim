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
            if(GameObject.ReferenceEquals(RobotSelectionManager.SelectedRobot, DeletionObject)){
                if(RobotSelectionManager.AvailableRobots.Count > 1){
                    for(int i = 0; i < RobotSelectionManager.AvailableRobots.Count; i++){
                        if(!GameObject.ReferenceEquals(RobotSelectionManager.AvailableRobots[i], DeletionObject)){
                            RobotSelectionManager.UpdateRobotSelected(i);
                            break;
                        }

                    }
            
                }
            }
            // We delete the robot
            Destroy(DeletionObject.transform.parent.gameObject);
        }


    }

    public void DeleteAllMenus(){
       DeletionArray = GameObject.FindGameObjectsWithTag("Menu");
       foreach (GameObject DeletionObject in DeletionArray)
            Destroy(DeletionObject);
    }
}
