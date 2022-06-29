using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deatach_Menus : MonoBehaviour
{
    public Transform robot;
    public Transform left_hand;

    private OVRGrabbable grabbable;
    private RectTransform rectTransform;

    private bool was_grabbed = false;

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
            was_grabbed = true;
        } else if(was_grabbed)
            transform.parent = robot;
        else
            transform.parent = left_hand;


    }
}
