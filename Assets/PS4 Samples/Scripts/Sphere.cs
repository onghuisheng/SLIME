using UnityEngine;
using System.Collections;

public class Sphere : MonoBehaviour {

    Vector3 newCol = new Vector3(1, 0, 0);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter()
    {
        print("NEAR");
        GetComponent<NetworkView>().RPC("SetColor", RPCMode.AllBuffered, newCol);        
    }

    [RPC]
    void SetColor(Vector3 color)
    {
        GetComponent<Renderer>().material.color = new Color(color.x, color.y, color.z, 1);
    }
}
