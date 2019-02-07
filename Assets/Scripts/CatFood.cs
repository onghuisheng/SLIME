using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using DG.Tweening;


public class CatFood : MonoBehaviour
{

    public MoveController m_LeftControl, m_RightControl;

    public GameObject m_Target, m_Target2;

    private float intensity = 3;
    private float gammaTarget = 0;

    void Update()
    {
        if (m_LeftControl.GetButton(MoveControllerButton.MiddleButton))
        {
            foreach (var enemy in FindObjectsOfType<EnemyHit>())
            {
                enemy.OnShot(null);
            }
        }

        //    if (m_LeftControl.GetButton(MoveControllerHotkeys.buttonUse))
        //    {
        //        m_Target2.transform.Translate(0, Time.deltaTime, 0);
        //    }

        //    if (m_RightControl.GetButton(MoveControllerHotkeys.buttonGrab))
        //    {
        //        m_Target.transform.Translate(0, -Time.deltaTime, 0);
        //    }

        //    if (m_RightControl.GetButton(MoveControllerHotkeys.buttonUse))
        //    {
        //        m_Target2.transform.Translate(0, -Time.deltaTime, 0);
        //    }


        //// Register all buttons to the dictionary
        //foreach (var key in System.Enum.GetValues(typeof(KeyCode)))
        //{
        //    if (Input.GetKeyUp((KeyCode)key))
        //    {
        //        Debug.Log(key.ToString());
        //    }
        //}

        //int ggx = 0;

        //string output = "";

        //foreach (var gg in Input.GetJoystickNames())
        //{
        //    ggx++;

        //    if (gg != "None")
        //    {
        //        output += " , " + gg + ' ';
        //    }
        //}

        //Debug.Log("Total: " + ggx + output);

    }

}
