using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deatach_Menus : MonoBehaviour
{
    public Transform newparent;
    public Transform left_hand;

    private OVRGrabbable grabbable;
    private RectTransform rectTransform;
    private Vector3 OriginalPos = Vector3.zero;
    private Quaternion OriginalRot = Quaternion.identity;

    private bool was_grabbed = false;

    void Awake(){
        OriginalPos = transform.GetComponent<RectTransform>().localPosition;
        OriginalRot = transform.GetComponent<RectTransform>().localRotation;
    }

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
            transform.parent = newparent;
        else {
            transform.parent = left_hand;
            OriginalPos = rectTransform.localPosition;
            OriginalRot = rectTransform.localRotation;
        }



    }

    public void ResetPos(){
        transform.parent = left_hand;
        transform.GetComponent<RectTransform>().localPosition = OriginalPos;
        transform.GetComponent<RectTransform>().localRotation = OriginalRot;
        was_grabbed = false;
    }

    public bool getwas_grabbed(){
        return was_grabbed;
    }
}
