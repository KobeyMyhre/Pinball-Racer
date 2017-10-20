using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBodyCollision : MonoBehaviour {

    public ParticleSystem[] sparks;
	// Use this for initialization
	void Start () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
        for (int i = 0; i < sparks.Length; i++)
        {
            sparks[i].Play();
        }
    }

   
    // Update is called once per frame
    void Update () {
		
	}
}
