using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {

    public float movespeed;
    public float rotationSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<NetworkView>().isMine)
        {
            float horizontal_input = Input.GetAxis("Horizontal");
            float vertical_input = Input.GetAxis("Vertical");


            transform.Translate(Vector3.forward * movespeed * vertical_input * Time.deltaTime);

            if (Input.GetAxis("Horizontal") != 0)
                transform.Rotate(Vector3.up * rotationSpeed * horizontal_input * Time.deltaTime);

            if (Input.GetKey(KeyCode.W))
                transform.Translate(Vector3.forward * movespeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.A))
                transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.D))
                transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        else
        {
            enabled = false;
        }
	}
}
