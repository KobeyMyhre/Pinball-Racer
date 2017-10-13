using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetCorrection : MonoBehaviour {
    public float offset;
	// Use this for initialization
	void Start () {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 50))
        {
            if (hit.collider.tag == "floor")
            {

                offset = (transform.position.y - hit.collider.transform.position.y);
            }

        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 3))
        {
            if (hit.collider.tag == "floor")
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + offset, transform.position.z);
            }

        }
    }
}
