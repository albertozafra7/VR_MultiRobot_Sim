using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralSpeedModify : MonoBehaviour
{
    public Slider auxJoint0;

    // Update is called once per frame
    void Update()
    {
        GeneralPathMover variable = GetComponent<GeneralPathMover>();
        auxJoint0.value = variable.General_Speed*50;
    }

    public void GspeedModify(float value)
    {
        GeneralPathMover variable = GetComponent<GeneralPathMover>();
        variable.General_Speed = value*0.02f;

    }

    public void GspeedModifyIncrement()
    {
        GeneralPathMover variable = GetComponent<GeneralPathMover>();
        if (variable.General_Speed < 2){ 
            variable.General_Speed += 0.02f;
        }
    }

    public void GspeedModifyDecrement()
    {
        GeneralPathMover variable = GetComponent<GeneralPathMover>();
        if (variable.General_Speed > 0)
        {
            variable.General_Speed -= 0.02f;
        }

    }
}
