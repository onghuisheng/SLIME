using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        ArrowBase arrow = other.GetComponent<ArrowBase>();

        if (arrow != null && !arrow.isFired)
        {
            arrow.BuffArrow(ArrowBase.ArrowType.Flame);
        }
    }

}
