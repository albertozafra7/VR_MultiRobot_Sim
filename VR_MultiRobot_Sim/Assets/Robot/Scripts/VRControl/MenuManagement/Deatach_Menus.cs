using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deatach_Menus : MonoBehaviour
{
    public Transform robot;

    private OVRGrabbable grabbable;
    private RectTransform rectTransform;

    void Start(){
        grabbable =  transform.GetComponent<OVRGrabbable>();
        rectTransform = transform.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(grabbable.isGrabbed){
            transform.parent = null;
            rectTransform.localPosition = grabbable.grabbedBy.transform.position;
            rectTransform.localRotation = grabbable.grabbedBy.transform.rotation;
        } else
            transform.parent = robot;


    }
}
