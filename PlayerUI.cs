using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public GameObject crossHair;
    public GameObject menu;

     
    private void Start()
    {      
        Menu.IsOn = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            ToggleUI();
        }

    }

    void ToggleUI()
    {
        crossHair.SetActive(!crossHair.activeSelf);
        menu.SetActive(!menu.activeSelf);
        Menu.IsOn = menu.activeSelf;
    }
}
