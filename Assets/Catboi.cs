using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Catboi : MonoBehaviour {
    	// Update is called once per frame
	void Update () {


        GetComponent<Text>().text = RenderSettings.reflectionIntensity.ToString();
	}
}
