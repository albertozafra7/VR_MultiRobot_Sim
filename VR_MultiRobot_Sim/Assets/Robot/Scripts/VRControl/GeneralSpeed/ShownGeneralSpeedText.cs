using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShownGeneralSpeedText : MonoBehaviour
{

    public Text shownText;
    // Start is called before the first frame update
 

    // Update is called once per frame
    public void TextUpdate(float value)
    {
        
        shownText.text = value + "%";//para pasarlo a porcentaje
    }
}
