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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "PurpleDrop" || other.tag != "BlueDrop")
            return;

        switch (Color)
        {
            case SlimeColor.Blue:
                {
                    Instantiate(BlueSlime, transform);
                    break;
                }

            case SlimeColor.Red:
                {
                    Instantiate(RedSlime, transform);
                    break;
                }

            case SlimeColor.Pink:
                {
                    Instantiate(PinkSlime, transform);
                    break;
                }

            case SlimeColor.Yellow:
                {
                    Instantiate(YellowSlime, transform);
                    break;
                }

            default:
                break;
        }

        Destroy(this);
    }
}
