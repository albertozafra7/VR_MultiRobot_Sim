using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RadialMenu.VR {
	
	[RequireComponent(typeof(MeshRenderer))]
	
	public class ToggleRender : MonoBehaviour {
	
		#region Variables
		
		private MeshRenderer mRenderer;
		
		#endregion
		
		#region Main Method
		    // Start is called before the first frame update
		    void Start() {
		        
				mRenderer = GetComponent<MeshRenderer>();
				
		    }

			
			public void ToggleRenderer() {
				
				if(mRenderer)
					mRenderer.enabled = !mRenderer.enabled;
				
			}
		#endregion
	}
}