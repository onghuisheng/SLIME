using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PS4;

public class PS4MoveController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(PS4Input.MoveIsConnected(0, 0))
        {
            PS4Input.MoveSetLightSphere(0, 0, 1, 1, 1);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
