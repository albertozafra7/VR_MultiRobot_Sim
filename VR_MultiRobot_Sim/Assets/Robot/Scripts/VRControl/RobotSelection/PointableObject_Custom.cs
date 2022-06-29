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
    
    [Header("Deletion Mode")]
    public Color HoverColor_Deletion;

    [Header("Move Mode")]
    public Color HoverColor_Move;

    [Header("Creation Mode")]
    public Color HoverColor_Creation;

    [Header("Meshes")]
    public List<MeshRenderer> meshes;

    [Header("Robot Selection Manager")]
    public RobotSelection RobotSelectionManager;

    // Enum that determine the mode of the program
    public enum Mode {
        Selection,
        Deletion,
        Move,
        Creation
    }
    private Mode currentMode = Mode.Selection;

    //public Component[] meshes;
    private Queue<Color> originalColors;
    /*
    private bool SelectionMode = false;
    private bool DeletionMode = false;
    private bool MoveMode = false;
    private bool CreationMode = false;*/

    #region callbacks

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        
        //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
        Debug.Log(name + " Game Object Clicked!");

        switch(currentMode){
            case Mode.Selection:
                RobotSelectionManager.UpdateRobotSelected(gameObject);
                break;

            // Under development
            case Mode.Deletion:
                break;
            

        }
    }

 
    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Output to console the GameObject's name and the following message
        Debug.Log("Cursor Entering " + name + " GameObject");

        switch(currentMode){
            case Mode.Selection:
                ApplyColorMask(HoverColor_Selection);
                break;
        }
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Output the following message with the GameObject's name
        Debug.Log("Cursor Exiting " + name + " GameObject");
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

    #region Setters N Getters

    public void setMode(Mode selectedMode){
        currentMode = selectedMode;
    }

    public void setMode2Selection(){
        currentMode = Mode.Selection;
    }

    public void setMode2Deletion(){
        currentMode = Mode.Deletion;
    }

    public void setMode2Creation(){
        currentMode = Mode.Creation;
    }

    public void setMode2Move(){
        currentMode = Mode.Move;
    }

    public Mode getMode(){
        return currentMode;
    }

    public Mode getCurrentMode(){
        return currentMode;
    }

    #endregion

    #region Color methods

    public void ApplyColorMask(Color colorMask){



        //meshes = GetComponentsInChildren<MeshRenderer>();

        Debug.Log(meshes.Count);

        foreach (MeshRenderer mesh_Renderer in meshes){
            for(int i = 0; i < mesh_Renderer.materials.Length; i++){
                originalColors.Enqueue(mesh_Renderer.materials[i].color);
                mesh_Renderer.materials[i].color = mesh_Renderer.materials[i].color * colorMask;
            }
        }
           
    }

    public void RestoreColor(){
        //meshes = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh_Renderer in meshes){
            for(int i = 0; i < mesh_Renderer.materials.Length; i++)
                mesh_Renderer.materials[i].color = originalColors.Dequeue();
        }
    }
    #endregion

}
