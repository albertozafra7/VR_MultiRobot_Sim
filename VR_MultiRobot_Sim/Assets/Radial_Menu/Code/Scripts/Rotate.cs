using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public GameObject Menu;
    public Vector3 direction;
    // Update is called once per frame
    void Update()
    {
        if(Menu.activeSelf)
            transform.Rotate(direction);
        
    }
}
