using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deatach_Menus : MonoBehaviour
{
    public OVRGrabbable This_Menu;
    public RectTransform This_Transform;
    public Transform robot;

    // Update is called once per frame
    void Update()
    {
        if(This_Menu.isGrabbed){
             transform.parent = null;
             This_Transform.localPosition = This_Menu.grabbedBy.transform.position;
             This_Transform.localRotation = This_Menu.grabbedBy.transform.rotation;
        } else
            transform.parent = robot;


    }
}
