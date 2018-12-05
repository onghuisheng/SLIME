using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatFood : MonoBehaviour
{

    public MoveController m_LeftControl, m_RightControl;

    public GameObject m_Target, m_Target2;


    void Update()
    {
        if (m_LeftControl.GetButtonDown(MoveControllerHotkeys.buttonGrab))
        {
            m_Target.transform.Translate(0, Time.deltaTime, 0);
        }

        if (m_LeftControl.GetButtonDown(MoveControllerHotkeys.buttonUse))
        {
            m_Target2.transform.Translate(0, Time.deltaTime, 0);
        }


        if (m_RightControl.GetButtonUp(MoveControllerHotkeys.buttonGrab))
        {
            m_Target.transform.Translate(0, -Time.deltaTime, 0);
        }

        if (m_RightControl.GetButtonUp(MoveControllerHotkeys.buttonUse))
        {
            m_Target2.transform.Translate(0, -Time.deltaTime, 0);
        }

    }

}
