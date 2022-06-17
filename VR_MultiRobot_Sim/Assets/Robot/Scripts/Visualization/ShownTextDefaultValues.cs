using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShownTextDefaultValues : MonoBehaviour
{
    public Text shownText;
    public Text shownTextZone;
    public Text shownTextJoint;
    // Start is called before the first frame update


    // Update is called once per frame
    public void TextSpeedUpdate(float value)
    {

        shownText.text = value + "mm/s";//para pasarlo a porcentaje
    }

    public void TextZoneUpdate(float value)
    {

        shownTextZone.text = "z"+value;//para pasarlo a porcentaje
    }

    public void TextSpeedJointUpdate(float value)
    {

        shownTextJoint.text = value + "º/s";//para pasarlo a porcentaje
    }
}
