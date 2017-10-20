using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {

    Vector3 originPos;
    Quaternion originFace;
    Vector3 originVel;
    Rigidbody rb;
    float timer = 0f;
    public float maxLife = 10f;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        originPos = transform.position;
        originFace = transform.rotation;
        originVel = rb.velocity;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (timer > maxLife)
        {
            transform.position = originPos;
            transform.rotation = originFace;
            rb.velocity = originVel;
            timer = 0f;
        }
	}
}
