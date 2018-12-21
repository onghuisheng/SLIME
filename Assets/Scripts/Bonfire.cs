using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour, IShootable {

    [SerializeField]
    private GameObject flamePrefab;    // flame particle

    bool isLighted = false;

    public void OnShot(ArrowBase arrow)
    {
        if(arrow.arrowType==ArrowBase.ArrowType.Flame && !isLighted) //if its flame arrow
        {
            Instantiate(flamePrefab, transform);
            isLighted = true;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
