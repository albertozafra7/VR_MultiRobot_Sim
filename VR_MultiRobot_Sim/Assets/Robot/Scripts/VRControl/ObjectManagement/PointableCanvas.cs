using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PointableCanvas : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public List<ObjectCreationManager> ObjectManager;
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        foreach(ObjectCreationManager Manager in ObjectManager)
            Manager.setCloning(false);
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        foreach(ObjectCreationManager Manager in ObjectManager){
            if(GameObject.Find("ObjectLibrary") != null && Manager.gameObject.name == "ObjectsForCloning")
                Manager.setCloning(true);
            
            if(GameObject.Find("RobotLibrary") != null && Manager.gameObject.name == "RobotsForCloning")
                Manager.setCloning(true);
        }
    }
}
