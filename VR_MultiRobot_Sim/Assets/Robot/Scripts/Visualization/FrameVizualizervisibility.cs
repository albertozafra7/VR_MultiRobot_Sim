using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameVizualizervisibility : MonoBehaviour
{
    public GameObject ghostTarget;

    // Update is called once per frame
    void Update()
    {
        if (ghostTarget.active) {
            //  GetComponent(FrameVisualizer)
            ghostTarget.AddComponent<FrameVisualizer>();
            Debug.LogWarning("Sidaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }
        else
        {
            Debug.LogWarning("Canceeeeeeeeeeeeeeeeeeeeeeeeeeer");
            Destroy(GetComponent<FrameVisualizer>());
        }
    }
}
