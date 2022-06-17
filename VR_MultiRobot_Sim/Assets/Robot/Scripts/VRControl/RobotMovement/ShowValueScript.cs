using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowValueScript : MonoBehaviour
{

	public Text shownText;
    // Start is called before the first frame update
    void Start()
    {
    	//shownText=GetComponent<Text>();    
    }

    // Update is called once per frame
    public void TextUpdate(float value)
    {
        shownText.text= value.ToString();
    }
}
