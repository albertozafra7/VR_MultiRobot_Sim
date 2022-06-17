using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RadialMenu.VR;

public class GetBack : MonoBehaviour
{
    public SteamVR_TrackedObject trackedObject;
    public SteamVR_Controller.Device device;

    void Start()
    {
        device = SteamVR_Controller.Input((int)trackedObject.index);
    }

    // Update is called once per frame
    void Update()
    {
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            if (GameObject.Find("SlidersJoints") != null)
            {
                GameObject.Find("SlidersJoints").SetActive(false);
                if (GameObject.Find("JointController") != null)
                    GameObject.Find("JointController").GetComponent<VR_RadialMenu>().DisableMenuFromBtn();
                if (GameObject.Find("Pointer_GRP") != null)
                    GameObject.Find("Pointer_GRP").SetActive(false);
                if (GameObject.Find("RadialMenu_right") != null)
                    GameObject.Find("RadialMenu_right").GetComponent<VR_RadialMenu>().ActiveMenuFromBtn();
            }
            else if (GameObject.Find("SpeedController") != null)
            {
                GameObject.Find("SpeedController").GetComponent<VR_RadialMenu>().DisableMenuFromBtn();
                if (GameObject.Find("GeneralSpeed") != null)
                    GameObject.Find("GeneralSpeed").SetActive(false);
                if (GameObject.Find("Pointer_GRP") != null)
                    GameObject.Find("Pointer_GRP").SetActive(false);
            }
            if (GameObject.Find("DefaultTargetValues") != null)
            {
                GameObject.Find("DefaultTargetValues").SetActive(false);
                if (GameObject.Find("Pointer_GRP") != null)
                    GameObject.Find("Pointer_GRP").SetActive(false);
                if (GameObject.Find("RadialMenu_right") != null)
                    GameObject.Find("RadialMenu_right").GetComponent<VR_RadialMenu>().ActiveMenuFromBtn();
            }
            if (GameObject.Find("Configuration") != null)
            {
                GameObject.Find("Configuration").SetActive(false);
                if (GameObject.Find("Pointer_GRP") != null)
                    GameObject.Find("Pointer_GRP").SetActive(false);
                if (GameObject.Find("RadialMenu_right") != null)
                    GameObject.Find("RadialMenu_right").GetComponent<VR_RadialMenu>().ActiveMenuFromBtn();
            }
            if (GameObject.Find("MoveTypeSelector").GetComponent<VR_RadialMenu>().isActive())
            {
                GameObject.Find("MoveTypeSelector").GetComponent<VR_RadialMenu>().DisableMenuFromBtn();
                if (GameObject.Find("RadialMenu_right") != null)
                    GameObject.Find("RadialMenu_right").GetComponent<VR_RadialMenu>().ActiveMenuFromBtn();
            }
        }
    }

}
