using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSlimeAfterShot : MonoBehaviour {

    public GameObject BlueSlime;
    public GameObject RedSlime;
    public GameObject PinkSlime;
    public GameObject YellowSlime;

    public enum SlimeColor
    {
        None,
        Blue,
        Red,
        Pink,
        Yellow,
    }

    public SlimeColor Color = SlimeColor.None;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
