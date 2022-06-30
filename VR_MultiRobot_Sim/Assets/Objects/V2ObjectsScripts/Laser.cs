using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//it is used for the right hand raycast, it is placed in Laser

public class Laser : MonoBehaviour
{
    private LineRenderer lr;
    public GameObject hitpoint;
    // Start is called before the first frame update
    void Start()
    {
        hitpoint = new GameObject();
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                lr.SetPosition(1, hit.point);
                hitpoint.transform.position=hit.point;
                Debug.LogError(hitpoint.transform.position);
            }
        }
        else
        {
            lr.SetPosition(1, transform.position + (transform.forward * 5000));
        }
    }
}
