﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRInputManager_Custom : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
    }

    void FixedUpdate()
    {
        OVRInput.FixedUpdate();
    }
}
