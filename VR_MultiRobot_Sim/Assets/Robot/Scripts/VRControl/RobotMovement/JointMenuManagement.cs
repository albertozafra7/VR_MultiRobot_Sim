using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JointMenuManagement : MonoBehaviour
{

    public List<Slider> JointSliders;
    public List<Text> HighlithtedText;
    private int index;

    void Start()
    {
        index = 0;
        HighlithtedText[index].gameObject.SetActive(true);

    }

    public void SelectedSliderInc()
    {
        int aux = index;
        if (index < JointSliders.ToArray().Length-1)
        {
            index += 1;
        }
        else if (index == JointSliders.ToArray().Length-1)
            index = 0;

        if (aux != index) {
            HighlithtedText[aux].gameObject.SetActive(false);
            HighlithtedText[index].gameObject.SetActive(true);
        }
    }

    public void SelectedSliderDec()
    {
        int aux = index;
        if (index > 0)
        {
            index -= 1;
        }
        else if (index == 0)
            index = JointSliders.ToArray().Length-1;


        if (aux != index)
        {
            HighlithtedText[aux].gameObject.SetActive(false);
            HighlithtedText[index].gameObject.SetActive(true);
        }
    }

    public void IncValue()
    {
        if (JointSliders[index].value<360) {
            JointSliders[index].value += 1;
        }
    }

    public void DecValue()
    {
        if (JointSliders[index].value > -360)
        {
            JointSliders[index].value -= 1;
        }
    }
}
