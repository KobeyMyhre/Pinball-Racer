using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchOther : MonoBehaviour {

    public GameObject explosionGO;
    ParticleSystem[] parts;
    SphereCollider coll;
    float timer = 0f;

	// Use this for initialization
	void Start () {
        explosionGO = Instantiate(explosionGO, transform);
        parts = explosionGO.GetComponentsInChildren<ParticleSystem>();
        coll = GetComponent<SphereCollider>();
	}

    void Explode()
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].Play();
        }        
        coll.enabled = false;
    }    

    void OnTriggerEnter(Collider other)
    {
        other.GetComponentInParent<Rigidbody>().AddExplosionForce(2500f, transform.position, 10f, 5f, ForceMode.Impulse);
        Explode();
    }

	// Update is called once per frame
	void Update () {
		if (coll.enabled == false)
        {
            timer += Time.deltaTime;
            if (timer >= 10f)
            {
                coll.enabled = true;
                timer = 0f;
            }
        }
	}
}
