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
        if (GetComponentInParent<Movement>())
            GetComponentInParent<Movement>().Move();

        else if (GetComponentInParent<SlimePatrol>())
            GetComponentInParent<SlimePatrol>().Move();
    }

    public void StopSlime()
    {
        if (GetComponentInParent<Movement>())
            GetComponentInParent<Movement>().Stop();

        else if (GetComponentInParent<SlimePatrol>())
            GetComponentInParent<SlimePatrol>().Stop();
    }
}
