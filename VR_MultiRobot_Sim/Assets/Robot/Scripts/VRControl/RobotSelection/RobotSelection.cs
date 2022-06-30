using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotSelection : MonoBehaviour
{
    [Header("Available Robots")]
    public List<URController> AvailableRobots = new List<URController>();

    [Header("Selected Robot")]
    public GameObject SelectedRobot;
    private URController SelectedRobot_Controller;

    [Header("Slider Menu")]
    public Text RobotName;
    public List<Slider> Sliders;

    // Enum that determine the mode of the program
    public enum Mode {
        Selection,
        Deletion,
        Move,
        Creation
    }
    private Mode currentMode = Mode.Selection;

    // Start is called before the first frame update

    // We get the childs which have component URController
    void Awake(){
        UpdateRobotList();
    }
    void Start()
    {
        UpdateRobotList();

        if(SelectedRobot != null)
            UpdateRobotSelected(SelectedRobot);
        else if(AvailableRobots.Count != 0)
            UpdateRobotSelected(transform.GetChild(0).gameObject);            

        Debug.Log(AvailableRobots.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Custom Methods
    public void UpdateRobotList()
    {
        if (AvailableRobots.Count != transform.childCount)
        {
            AvailableRobots = new List<URController>(transform.childCount);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
           AvailableRobots[i] = transform.GetChild(i).GetComponentInChildren(typeof(URController)) as URController;

        }
    }

    public void UpdateRobotSelected(GameObject RobotChosen){

        if(AvailableRobots.Count == 0)
            UpdateRobotList();

        ManageSelectedIndicator(RobotChosen);
        // Set the global variables
        SelectedRobot = RobotChosen;
        SelectedRobot_Controller = RobotChosen.GetComponent<URController>();

        // In case that we have a joint Menu
        setJointMenu();


    }

    public void UpdateRobotSelected(int index){
        // we ensure that there is at least one robot at the list
        if(AvailableRobots.Count == 0)
            UpdateRobotList();
        
        if(AvailableRobots.Count != 0 && AvailableRobots.Count > index){
            UpdateRobotSelected(AvailableRobots[index].gameObject);
        }
    }

    // In case we changed the robot selected we need to update the menu attached to the left hand
    public void setJointMenu(){
        

        RobotName.text = SelectedRobot.name;

        int SlidersUsed = SelectedRobot.GetComponent<UrdfRobot>().Values.Count;

        for(int i = 0; i < Sliders.Count; i++){
            if(i <= SlidersUsed){
                Sliders[i].enabled = true;
                Sliders[i].onValueChanged.RemoveAllListeners();
                switch(i){
                    case 0:
                        Sliders[i].onValueChanged.AddListener(value => SelectedRobot.GetComponent<MoveJoints>().joint0(Sliders[0].value));
                        SelectedRobot.GetComponent<MoveJoints>().auxJoint0 = Sliders[0];
                        break;

                    case 1:
                        Sliders[i].onValueChanged.AddListener(value => SelectedRobot.GetComponent<MoveJoints>().joint1(Sliders[1].value));
                        SelectedRobot.GetComponent<MoveJoints>().auxJoint1 = Sliders[1];
                        break;

                    case 2:
                        Sliders[i].onValueChanged.AddListener(value => SelectedRobot.GetComponent<MoveJoints>().joint2(Sliders[2].value));
                        SelectedRobot.GetComponent<MoveJoints>().auxJoint2 = Sliders[2];
                        break;

                    case 3:
                        Sliders[i].onValueChanged.AddListener(value => SelectedRobot.GetComponent<MoveJoints>().joint3(Sliders[3].value));
                        SelectedRobot.GetComponent<MoveJoints>().auxJoint3 = Sliders[3];
                        break;

                    case 4:
                        Sliders[i].onValueChanged.AddListener(value => SelectedRobot.GetComponent<MoveJoints>().joint4(Sliders[4].value));
                        SelectedRobot.GetComponent<MoveJoints>().auxJoint4 = Sliders[4];
                        break;

                    case 5:
                        Sliders[i].onValueChanged.AddListener(value => SelectedRobot.GetComponent<MoveJoints>().joint5(Sliders[5].value));
                        SelectedRobot.GetComponent<MoveJoints>().auxJoint5 = Sliders[5];
                        break;

                    default:
                        Debug.LogError("THERE ARE NOT SO MANY JOINTS ON THE ROBOTS");
                        break;
                }
                Sliders[i].onValueChanged.AddListener(delegate {SelectedRobot.GetComponent<MoveJoints>().UpdateEE_Frame();});

            } else
                Sliders[i].enabled = false;

        }


    }

    public void ManageSelectedIndicator(GameObject RobotChosen){
        GameObject indicator = SelectedRobot.transform.parent.Find("SelectionIndicator").gameObject;
        if(indicator != null)
            indicator.SetActive(false);

        indicator = RobotChosen.transform.parent.Find("SelectionIndicator").gameObject;
        if(indicator != null)
            indicator.SetActive(true);
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
}
