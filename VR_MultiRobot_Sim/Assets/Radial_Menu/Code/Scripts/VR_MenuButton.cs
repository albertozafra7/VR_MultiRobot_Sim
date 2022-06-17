using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace RadialMenu.VR {

	[RequireComponent(typeof(Animator))]	// If we want to have some fade out extra or a hover animation, this is necessary
	[RequireComponent(typeof(Image))]		// This is only to make sure that we have an image
	
	
	public class VR_MenuButton : MonoBehaviour {
	
		#region Variables
		
		[Header("Button Properties")]
		public int buttonID;
		public string buttonText;	// This allows the user to input some text
		public Image buttonIcon;
		public Sprite normalImage;	// This is the image that you see when the button is not interacted
		public Sprite hoverImage;	// This is the image that you see highlighted or hovered when you press this button
		
		[Header("Events")]
		public UnityEvent OnClick = new UnityEvent();   // This will simulate the OnClick event of the buttons
		
		private Animator animator;
		private Image currentImage; // This will control the image that the button is using
		
		#endregion
		
		#region Main Methods
		    // Start is called before the first frame update
		    void Start() {
		        
				animator = GetComponent<Animator>();
				currentImage = GetComponent<Image>();
				
				if(currentImage && normalImage)
					currentImage.sprite = normalImage;
				
		    }
		
		#endregion
		
		#region Custom Methods
		
		public void OnHover(int ID) {
			
			if(currentImage) {
				
				if(ID == buttonID && hoverImage) {
				
					currentImage.sprite = hoverImage;
					HandleAnimator(true);
				
				} else if(normalImage) {
				
					currentImage.sprite = normalImage;
					HandleAnimator(false);
				}
			}
			
		}
		
		public void Click(int ID) {
			
			if(buttonID == ID) {
			
				if(OnClick != null) {
					
					OnClick.Invoke();
					
				}
			}
		}

		void HandleAnimator(bool Toggle) {
			
			if(animator)
				animator.SetBool("hover", Toggle);
			
		}
		
		#endregion
	}
}