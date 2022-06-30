using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RadialMenu.VR {
	
	// Custom Events which send the current menu ID
	public class onHover : UnityEvent<int>{}
	public class onClick : UnityEvent<int>{}
	
	[RequireComponent(typeof(Animator))]	// Require the components which are going to control (show or hide) the rest
	[RequireComponent(typeof(CanvasGroup))]// Allows us to fade out and turn off some components in a easy way
	
	public class VR_RadialMenu : MonoBehaviour {
	
		#region Variables
		
		/*[Header("Controller Properties")]
		public SteamVR_TrackedController controller;	// Allow to someone to select the controller which will control the radial menu
		*/
		public SimpleCapsuleWithStickMovement StickMovement;
		[Header("UI Properties")]
		public List<VR_MenuButton> menuButtons = new List<VR_MenuButton>();	// Contains all the menu buttons
		public RectTransform m_ArrowContainer;
		public Text m_DebugText;		// For displaying the debug text while you have the headset on
		
		
		[Header("Events")]
		public UnityEvent OnMenuChanged = new UnityEvent();
		
		
		private Vector2 currentAxis;	// The Axis of the main controller, where the display is
		private GameObject controllerDevice;	// Gives us access to the controller itself
		
		private Animator animator;
		
		
		// Main flags
		private bool menuOpen = false;
		private bool allowNavigation = false;
		private bool isTouching = false;
		private float currentAngle;
		private bool jointMenuOpen = false;
		
		
		// Current and previous position of the arrow
		private int currentMenuID = -1;
		private int previousMenuID = -1;
		
		// Unity Events
		private onHover OnHover = new onHover();
		private onClick OnClick = new onClick();
		
		#endregion
	
		#region Main Methods
		
		    // Start is called before the first frame update
		    void Start() {
			
				animator = GetComponent<Animator>();	// This guaranties that we have an animator attached, if not it complains
			
		        /*if(controller) {
					
					controllerDevice = SteamVR_Controller.Input((int)controller.controllerIndex);
					
					controller.PadTouched += HandlePadTouched;
					controller.PadUntouched += HandlePadUntouched;
					controller.PadClicked += HandlePadClicked;
					controller.MenuButtonClicked += HandleMenuActivation;
				}*/
				
				// Attach each button of the menu to an event
				if(menuButtons.Count > 0) {
					
					// Loop around the buttons and attach them to the onClick and onHover events
					foreach(var button in menuButtons) {
						
						OnHover.AddListener(button.OnHover);
						OnClick.AddListener(button.Click);
						
					}
					
				}
				
				
				
		    }
			
			void OnDisable() {
				
				/*if(controller) {
					controller.PadTouched -= HandlePadTouched;
					controller.PadUntouched -= HandlePadUntouched;
					controller.PadClicked -= HandlePadClicked;
					controller.MenuButtonClicked -= HandleMenuActivation;
				}*/
				
				if(OnHover != null)
					OnHover.RemoveAllListeners();
					
				if(OnClick != null)
					OnClick.RemoveAllListeners();
			}
		
		    // Update is called once per frame
		    void Update() {
				if(OVRInput.GetDown(OVRInput.Button.Start)){
					//menuOpen = !menuOpen;
					HandleMenuActivation();
				}
					
				if(OnClick != null && menuOpen && (OVRInput.Get(OVRInput.Button.SecondaryThumbstick) || OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || OVRInput.GetDown(OVRInput.RawButton.X))){
					OnClick.Invoke(currentMenuID);
					StickMovement.EnableRotation = true;
					StickMovement.HMDRotatesPlayer = true;
					StickMovement.SmoothRotation = true;
				}
				
			
		    	if(menuOpen)
					UpdateMenu();
			    
		    }
			
		#endregion
		
		
		#region Custom Methods
		
		/*void HandlePadTouched(object sender, ClickedEventArgs e) {
			
			isTouching = true;
			// HandleDebugText("Touched Pad");
			
		}
		
		void HandlePadUntouched(object sender, ClickedEventArgs e) {
			
			isTouching = false;
			// HandleDebugText("Untouched Pad");
			
		}*/
		
		/*void HandlePadClicked(object sender, ClickedEventArgs e) {
			
			// HandleDebugText("Clicked Pad");
			
			if(OnClick != null && menuOpen)
				OnClick.Invoke(currentMenuID);
			
		}*/
		
		void HandleMenuActivation(/*object sender, ClickedEventArgs e*/) {

			GameObject SliderJoint = GameObject.Find("SlidersJoints");
			GameObject GenSpeed = GameObject.Find("GeneralSpeed");
			//if /*(*/(SliderJoint != null && name == "RadialMenu_right")// || (GenSpeed != null && this.name == "RadialMenu_right") || (GameObject.Find("MoveTypeSelector").GetComponent<VR_RadialMenu>().isActive() && this.name == "RadialMenu_right"))
			//	return;

			/*if(this.name == "RadialMenu_right" || this.name == "RadialMenu_left")
            {*/
				menuOpen = !menuOpen;
				if(menuOpen){
					StickMovement.EnableRotation = false;
					StickMovement.HMDRotatesPlayer = false;
					StickMovement.SmoothRotation = false;
				}else{
					StickMovement.EnableRotation = true;
					StickMovement.HMDRotatesPlayer = true;
					StickMovement.SmoothRotation = true;
				}

				//HandleDebugText("Menu is: " + menuOpen);
				HandleDebugText(menuButtons.ToArray()[0].name);

				HandleAnimator();
			//}			
			
		}
		
		public void ActiveMenuFromBtn()
        {
			menuOpen = !menuOpen;
			HandleAnimator();
		}

		public void DisableMenuFromBtn()
        {
			menuOpen = false;
			HandleAnimator();
		}
		
		void HandleAnimator() {
			
			if(animator)
				animator.SetBool("open", menuOpen);
		}
		
		void UpdateMenu() {
		
			if(/*isTouching && */menuOpen){
				
				// Get the current Axis from the Touch Pad and turn it into an Angle
				currentAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
				currentAngle = Vector2.SignedAngle(Vector2.up, currentAxis);
				
				// Up	 --> 0
				// Right --> -90
				// Left  --> 90
				// Down	 --> -180/180
				// HandleDebugText(currentAngle.ToString());
				
				
				float menuAngle = currentAngle;
				
				if(menuAngle < 0)
					menuAngle += 360f;
					
				int updateMenuID = (int)(menuAngle/ (360f / menuButtons.ToArray().Length));


				// HandleDebugText(updateMenuID.ToString());
				if(updateMenuID + 1 == menuButtons.ToArray().Length)
					HandleDebugText(menuButtons.ToArray()[0].name);
				else
					HandleDebugText(menuButtons.ToArray()[updateMenuID+1].name);


				// Update Current Menu ID
				if (updateMenuID != currentMenuID) {
					
					if(OnHover != null)
						OnHover.Invoke(updateMenuID);
						
					if(OnMenuChanged != null)
						OnMenuChanged.Invoke();
					
					previousMenuID = currentMenuID;
					currentMenuID = updateMenuID;
					
				}
				
				
				// Rotate Arrow
				if(m_ArrowContainer)
					m_ArrowContainer.localRotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);

			}
			
		}
		
		void HandleDebugText(string DebugText) {
			
			if(m_DebugText)
				m_DebugText.text = DebugText;
				
		}

		public bool isActive()
        {
			return menuOpen;

		}
		#endregion
	}
}