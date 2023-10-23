using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    Camera maincam;

    private void Start()
    {
        maincam = GetComponent<Camera>();
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.Alpha2))
        {
            maincam.depth = -1;
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            maincam.depth = 1;
        }
    }
}
