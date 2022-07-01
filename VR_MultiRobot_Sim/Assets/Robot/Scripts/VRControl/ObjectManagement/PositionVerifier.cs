using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionVerifier : MonoBehaviour
{

    public bool is_feasible_pos = true;
 
    //make a list to track collided objects
    private List<Collider> collidedObjects = new List<Collider>();
    
 
    // if there is collision with an object in either Enter or Stay, add them to the list 
    // (you can check if it already exists in the list to avoid double entries, 
    // just in case, as well as the tag).
    void OnCollisionEnter(Collision col) 
    {
        //Debug.LogError("EH HA HABIDO UNA COLISION");
        if (!collidedObjects.Contains(col.collider) && (col.gameObject.tag == "Robot" || col.gameObject.tag == "Object" || col.gameObject.tag == "ObjectMenu" || col.gameObject.tag == "RobotMenu" || col.gameObject.tag == "RobotEssential"))
        {
            //Debug.LogError("Era un objeto");
            is_feasible_pos = false;
            collidedObjects.Add(col.collider); 
        }
    }

    void OnCollisionExit(Collision col) {
        //Debug.LogError("EH SE HA SALIDO DE UNA COLISION");
        if(collidedObjects.Contains(col.collider) && (col.gameObject.tag == "Robot" || col.gameObject.tag == "Object" || col.gameObject.tag == "ObjectMenu" || col.gameObject.tag == "RobotMenu" || col.gameObject.tag == "RobotEssential"))
        {
            collidedObjects.Remove(col.collider);
            if(collidedObjects.Count > 0){
                foreach(Collider ObjectCollider in collidedObjects){
                    if(ObjectCollider.gameObject.tag == "Robot" || ObjectCollider.gameObject.tag == "Object" || ObjectCollider.gameObject.tag == "ObjectMenu" || ObjectCollider.gameObject.tag == "RobotMenu" || ObjectCollider.gameObject.tag == "RobotEssential")
                        return;
                }
                is_feasible_pos = true;
            } else 
                is_feasible_pos = true;
        }
    }

    void OnCollisionStay(Collision col){
        is_feasible_pos = true;
        collidedObjects.Clear();
        OnCollisionEnter(col);

  
    }

}
