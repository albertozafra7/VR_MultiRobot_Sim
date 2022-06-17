
using UnityEngine;
using UnityEngine.AI;

public class MoveToClick : MonoBehaviour {

float distance = 1;

void OnMouseDrag() {


}


// Use this for initialization
void Start () {

}

// Update is called once per frame
void Update () {
if (Input.GetMouseButton(0)){

    Vector3 mousePosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distance);
    Vector3 objPosition = Camera.main.ScreenToWorldPoint (mousePosition);

    transform.position = objPosition;
	}

  }
}
	