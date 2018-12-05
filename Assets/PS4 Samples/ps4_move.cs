using UnityEngine;
using System.Collections;
using System;

using UnityEngine.PS4;

public class ps4_move : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	

	void OnGUI() 
	{
		int numDetected = 0;
        for (int slot = 0; slot < 4; slot++)
        {
            // two move controllers are supported per user .. in addition to the regular controller
            for (int controller = 0; controller < 2; controller++)
            {
                if (PS4Input.MoveIsConnected(slot, controller))
                {
                    string data = String.Format("user {0} move {1} buttons {2} {3} gyro {4}",
                        slot, controller,
                        PS4Input.MoveGetButtons(slot, controller),
                        PS4Input.MoveGetAnalogButton(slot, controller),
                        PS4Input.GetLastMoveGyro(slot, controller)
                            );
                    GUI.Label(new Rect(64, 64 + slot * 40 + controller * 20, 1500, 20), data);
                    numDetected++;
                }
            }
        }

		GUI.Label(new Rect(64, 800, 1500, 20), String.Format("{0} Move controlers detected", numDetected) );
		
	}
}
