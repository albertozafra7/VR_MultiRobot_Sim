using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasRotation : MonoBehaviour
{
    public GameObject canvas;
    public GameObject aux;

    // Start is called before the first frame update
    void Start()
    {   
        canvas.transform.rotation=aux.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        canvas.transform.rotation = aux.transform.rotation;
       /*
        if (canvas.transform.rotation==)
        {


        }
       */
    }
}
