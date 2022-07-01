using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PointableObject_Custom : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    // Colors
    [Header("Selection Mode")]
    public Color HoverColor_Selection;
    public bool force_color = false;
    
    [Header("Deletion Mode")]
    public Color HoverColor_Deletion;

    [Header("Move Mode")]
    public Color HoverColor_Move;

    [Header("Creation Mode")]
    public Color HoverColor_Creation;

    [Header("Meshes")]
    public List<Renderer> meshes;

    [Header("Robot Selection Manager")]
    public RobotSelection RobotSelectionManager;

    

    //public Component[] meshes;
    private Queue<Color> originalColors = new Queue<Color>();
    public OVRGrabbable grabbable;
    /*
    private bool SelectionMode = false;
    private bool DeletionMode = false;
    private bool MoveMode = false;
    private bool CreationMode = false;*/

    /*public void Awake(){
        foreach (Renderer mesh_Renderer in meshes){
            for(int i = 0; i < mesh_Renderer.materials.Length; i++){
                originalColors.Enqueue(mesh_Renderer.materials[i].color);
            }
        }
    }*/

    #region callbacks

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        
        //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
        Debug.Log(name + " Game Object Clicked!");

        switch(RobotSelectionManager.getCurrentMode()){
            case RobotSelection.Mode.Selection:
                if(gameObject.tag == "Robot")
                    RobotSelectionManager.UpdateRobotSelected(gameObject);
                break;

            // Under development
            case RobotSelection.Mode.Deletion:
                if(gameObject.tag == "Robot")
                    DeleteRobot();
                else if(gameObject.tag == "Object")
                    DeleteObject();
                    
                break;
            

        }
    }

 
    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Output to console the GameObject's name and the following message
        Debug.Log("Cursor Entering " + name + " GameObject");

        if(GameObject.Find("Pointer") != null && !grabbable.isGrabbed){
            switch(RobotSelectionManager.getCurrentMode()){
                case RobotSelection.Mode.Selection:
                    if(gameObject.tag == "Robot")
                        ApplyColorMask(HoverColor_Selection);
                    break;

                case RobotSelection.Mode.Deletion:
                    if(gameObject.tag == "Robot" || gameObject.tag == "Object")
                        ApplyColorMask(HoverColor_Deletion);
                    break;

                case RobotSelection.Mode.Move:
                    if(gameObject.tag == "Robot" || gameObject.tag == "Object")
                        ApplyColorMask(HoverColor_Move);
                    break;
            }
        }
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Output the following message with the GameObject's name
        Debug.Log("Cursor Exiting " + name + " GameObject");
        if(gameObject != null)
            RestoreColor();
    }

        //Detect current clicks on the GameObject (the one with the script attached)
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        Debug.Log(name + "Game Object Click in Progress");
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        Debug.Log(name + "No longer being clicked");
    }

    #endregion

    #region Color methods

    public void ApplyColorMask(Color colorMask){



        //meshes = GetComponentsInChildren<MeshRenderer>();

        foreach (Renderer mesh_Renderer in meshes){
            for(int i = 0; i < mesh_Renderer.materials.Length; i++){
                originalColors.Enqueue(mesh_Renderer.materials[i].color);
                if(force_color)
                    mesh_Renderer.materials[i].color = colorMask;
                else
                    mesh_Renderer.materials[i].color = mesh_Renderer.materials[i].color * colorMask;
            }
        }
           
    }

    public void RestoreColor(){
        //meshes = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh_Renderer in meshes){
            for(int i = 0; i < mesh_Renderer.materials.Length; i++){
                if(originalColors.Count > 0)
                    mesh_Renderer.materials[i].color = originalColors.Dequeue();
            }      
        }
    }

    public Queue<Color> getOriginalColors(){
        return originalColors;
    }

    public void setOriginalColors(Queue<Color> desiredColors){
        for(int i = 0; i < originalColors.Count; i++)
            originalColors.Dequeue();
        
        for(int i = 0; i < desiredColors.Count; i++)
            originalColors.Enqueue(desiredColors.Dequeue());
    }
    #endregion

    #region Constructors and Destructors

    private void DeleteRobot(){

        if(GameObject.ReferenceEquals(RobotSelectionManager.SelectedRobot, gameObject)){
            if(RobotSelectionManager.AvailableRobots.Count > 1){
                for(int i = 0; i < RobotSelectionManager.AvailableRobots.Count; i++){
                    if(RobotSelectionManager.AvailableRobots[i].gameObject != null){
                        if(!GameObject.ReferenceEquals(RobotSelectionManager.AvailableRobots[i].gameObject, gameObject)){
                            RobotSelectionManager.UpdateRobotSelected(i);
                            break;
                        }
                    }
                }
        
            }
        }

        transform.parent.parent = null;
        RobotSelectionManager.UpdateRobotList();
        // We delete the robot
        Destroy(transform.parent.gameObject);

    }

    private void DeleteObject(){
        Destroy(gameObject);
    }

    /*void OnDestroy() {
        if(gameObject.tag == "Robot")
            RobotSelectionManager.UpdateRobotList();
    }*/

    #endregion

}
