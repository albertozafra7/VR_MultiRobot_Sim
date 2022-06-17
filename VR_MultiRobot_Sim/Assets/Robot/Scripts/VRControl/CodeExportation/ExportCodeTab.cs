using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExportCodeTab : MonoBehaviour
{
    public GameObject Targets;
    public Text text;
    public bool shownTargets=true;
    Target aux1;
    private int n=0,j=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WriteCode()
    {
        text.text = "";
        if (shownTargets)
        {
            for (int i = n; i < Targets.transform.childCount; ++i)
            {
                aux1 = Targets.transform.GetChild(i).GetComponent<Target>();
                text.text = text.text + aux1.targetDef + "\n";

            }
            text.text = text.text + "\n";
        }

        for (int i = j; i < Targets.transform.childCount; ++i)
        {
            aux1 = Targets.transform.GetChild(i).GetComponent<Target>();
            text.text = text.text + aux1.instruction + "\n";

        }

    }

    public void TargetShown(bool toggle)
    {
        shownTargets = toggle;
        WriteCode();
    }
    
    public void DownText()
    {
        if (shownTargets)
        {
            if (n < Targets.transform.childCount) { 
                ++n;
            }else{
                if (j < Targets.transform.childCount)
                {
                    ++j;
                }
            }
        }
        else
        {
            if (j < Targets.transform.childCount)
            {
                ++j;
            }

        }
        WriteCode();
    }

    public void UpText()
    {
        if (shownTargets)
        {
            if (j >0)
            {
                --j;
            }
            else
            {
                if (n >0)
                {
                    --n;
                }
            }
        }
        else
        {
            if (j > 0)
            {
                --j;
            }

        }
        WriteCode();
    }
     public void Inicialize()
    {
        n = 0;
        j = 0;
    }
    
}
