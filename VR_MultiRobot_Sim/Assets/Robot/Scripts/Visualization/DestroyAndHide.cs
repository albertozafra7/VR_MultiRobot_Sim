using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAndHide : MonoBehaviour
{
    
    public GameObject Targets;
    public bool Frame=true;
    FrameVisualizer aux1;
    LinearPath aux2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (aux!=toggle)
        {
            HideFrame();
        }
        aux = toggle;
        */
    }

    public void HideFrame(bool toggle) {

        Frame = toggle; 
        if (toggle)
        {

            for (int i = 0; i < Targets.transform.childCount; ++i)
            {
                aux1 = Targets.transform.GetChild(i).GetComponent<FrameVisualizer>();
                aux1.enabled = true;

            }

        }
        else
        {
            for (int i = 0; i < Targets.transform.childCount ; ++i)
            {
                aux1 = Targets.transform.GetChild(i).GetComponent<FrameVisualizer>();
                aux1.enabled = false;

            }

        }

    
    }

    public void DestroyTargets()
    {
        aux2 = Targets.transform.GetComponent<LinearPath>();
        for (int i = 0; i < Targets.transform.childCount ; ++i)
        {
            Destroy(Targets.transform.GetChild(i).gameObject);
            
        }
        aux2.Run = false;
    }
}
