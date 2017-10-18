using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatipultEvent : MonoBehaviour {

    public float force = 2;
    public float StopPoint = 0.0F;
    public float RePoint = 90.0F;
    public float rot = 0.0F;
    public bool check = false;
    public bool fired = false;
	void Update () {
      
        
        rot = GetComponentInParent<Transform>().rotation.z;
        if (rot <= RePoint && fired == false)
        {
            check = true;
            GetComponent<Rigidbody>().AddForce(Vector3.right * force);
        }

        if (rot <= StopPoint)
        {
            fired = false;
        }
    }
}
