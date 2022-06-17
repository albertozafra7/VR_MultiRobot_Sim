using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenu : MonoBehaviour
{

	public GameObject menu;
	public GameObject pointer;

	public List<GameObject> MenuList;
	public void showmenu(){

		menu.SetActive(!menu.activeSelf);


		if (menu.activeSelf)
		{
			pointer.SetActive(true);
			HideOtherMenus();
		} else
        {
			pointer.SetActive(false);
        }
	}

	public void HideOtherMenus()
    {
		foreach (GameObject OtherMenu in MenuList)
			OtherMenu.SetActive(false);
    }
   
}
