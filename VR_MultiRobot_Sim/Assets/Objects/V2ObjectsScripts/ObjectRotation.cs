using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is placed in the miniObjects placed in the left hand object menu
public class ObjectRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation *= Quaternion.AngleAxis(100 * Time.deltaTime, Vector3.up);
    }
}
