using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for the objectmenu to be at the hand height, placed in each objectmenu

public class ObjectMenuHeight : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menu, head;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        menu.transform.position = new Vector3(menu.transform.position.x, head.transform.position.y, menu.transform.position.z);
    }
}
