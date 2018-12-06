using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMovementAnimation : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void MoveSlime()
    {
        GetComponentInParent<Movement>().Move();
    }

    public void StopSlime()
    {
        GetComponentInParent<Movement>().Stop();
    }
}
