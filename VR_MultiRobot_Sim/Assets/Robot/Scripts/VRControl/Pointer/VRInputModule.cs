using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VRInputModule : BaseInputModule
{

    public Camera m_Camera;
    // Controller
   // public SteamVR_TrackedObject trackedObject;
   // public SteamVR_Controller.Device device;

    private GameObject m_CurrentObject = null;
    private PointerEventData m_Data = null;

    public GameObject FK_Frame;
    public GameObject EE_Frame;

    protected override void Awake()
    {
        // Selects the right controller
        /*device = SteamVR_Controller.Input((int)trackedObject.index);*/

        base.Awake();

        m_Data = new PointerEventData(eventSystem);
    }

    public override void Process()
    {

        // Reset data, set camera
        m_Data.Reset();
        m_Data.position = new Vector2(m_Camera.pixelWidth / 2, m_Camera.pixelHeight / 2);

        // We just set the Raycast in every frame
        eventSystem.RaycastAll(m_Data, m_RaycastResultCache);
        m_Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_CurrentObject = m_Data.pointerCurrentRaycast.gameObject;

        // Clear Raycast
        m_RaycastResultCache.Clear();

        // Hover State of the object pointed
        HandlePointerExitAndEnter(m_Data, m_CurrentObject);

        // Press
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
            ProcessPress(m_Data);

        // Release
        if (!OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
            ProcessRelease(m_Data);

        ExecuteEvents.Execute(m_Data.pointerDrag, m_Data, ExecuteEvents.dragHandler);

    }

    public PointerEventData GetData()
    {
        return m_Data;
    }

    private void ProcessPress(PointerEventData data)
    {
        // Set Raycast
        data.pointerPressRaycast = data.pointerCurrentRaycast;

        // Check for object hit, get the down handler, call
        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(m_CurrentObject, data, ExecuteEvents.pointerDownHandler);
        GameObject newPointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(data.pointerPressRaycast.gameObject);
        ExecuteEvents.Execute(newPointerDrag, data, ExecuteEvents.beginDragHandler);

        // If no down handler, try and get click handler
        if (newPointerPress == null)
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_CurrentObject);

        // Set data
        data.pressPosition = data.position;
        data.pointerPress = newPointerPress;
        data.pointerDrag = newPointerDrag;
        data.rawPointerPress = m_CurrentObject;

    }

    private void ProcessRelease(PointerEventData data)
    {
        // Execute pointer up
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        // Check for click handler
        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_CurrentObject);

        // Check if actual
        if(data.pointerPress == pointerUpHandler)
        {
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);
        }
        ExecuteEvents.Execute(data.pointerDrag, data, ExecuteEvents.endDragHandler);

        // Clear selected gameobject
        eventSystem.SetSelectedGameObject(null);

        // Reset data
        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress = null;
        data.pointerDrag = null;

        EE_Frame.transform.position = FK_Frame.transform.position;
        EE_Frame.transform.rotation = FK_Frame.transform.rotation;
    }
}
