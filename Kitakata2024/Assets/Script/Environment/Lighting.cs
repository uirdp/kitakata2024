using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour
{
    public Light normalLight;
    public Light nightLight;
    
    private void ChangeLighting()
    {
        normalLight.enabled = !normalLight.enabled;
        nightLight.enabled = !nightLight.enabled;
    }

    private void Start()
    {
        normalLight.enabled = true;
        nightLight.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeLighting();
        }
    }

}
