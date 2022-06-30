using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParentMoveToChild : MonoBehaviour
{


    private Vector3 hola;
    public GameObject father;

    void Start()
    {
        hola=transform.localPosition;
    }
    void Update()
    {
        if ((transform.localPosition-hola)!=Vector3.zero) {
            father.transform.position = father.transform.position + (transform.localPosition-hola);
            transform.localPosition=hola;
        }
        Debug.LogWarning(transform.localPosition);
    }
    
    
}
