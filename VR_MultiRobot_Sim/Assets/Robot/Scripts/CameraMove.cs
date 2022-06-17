using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{


// Use this for initialization
void Start () {

}

// Update is called once per frame
void Update () {
if (Input.GetMouseButton(1)){

		float x = 5 * Input.GetAxis ("Mouse X");
         float y = 5 * -Input.GetAxis ("Mouse Y");
         Camera.main.transform.Rotate (x, y, 0);
	}

  }
}

