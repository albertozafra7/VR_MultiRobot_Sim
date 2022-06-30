using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAxisRotation : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject yee;
    void Start()
    {
        yee = new GameObject();
        yee.transform.localPosition = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = new Vector3(yee.transform.localPosition.x, this.transform.localPosition.y, yee.transform.localPosition.z);

    }
}
