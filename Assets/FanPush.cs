using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanPush : MonoBehaviour {

    public float windPower = 500f;

    void OnTriggerStay(Collider other)
    {
        other.GetComponentInParent<Rigidbody>().AddForce(transform.forward * windPower, ForceMode.Impulse);
    }
}
