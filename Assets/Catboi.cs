using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Catboi : MonoBehaviour {
    	// Update is called once per frame
	void Update () {


        GetComponent<Text>().text = XRDevice.isPresent.ToString();


	}
}
