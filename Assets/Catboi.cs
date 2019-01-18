using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Catboi : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {


        if (MoveController.GetControllerThatHolds(null).GetButtonDown(MoveControllerButton.Triangle))
        {
            RenderSettings.reflectionIntensity += 0.05f;
        }
        if (MoveController.GetControllerThatHolds(null).GetButtonDown(MoveControllerButton.Circle))
        {
            RenderSettings.reflectionIntensity -= 0.05f;
        }

        if (MoveController.GetControllerThatHolds(null).GetButtonDown(MoveControllerButton.Square))
        {
            RenderSettings.ambientIntensity += 0.05f;
        }
        if (MoveController.GetControllerThatHolds(null).GetButtonDown(MoveControllerButton.X))
        {
            RenderSettings.ambientIntensity -= 0.05f;
        }

        GetComponent<Text>().text = string.Format("Ambient: {0} - Reflection: {1}", RenderSettings.ambientIntensity.ToString(), RenderSettings.reflectionIntensity.ToString());


    }
}
