using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchOther : MonoBehaviour {

    public GameObject explosionGO;
    ParticleSystem[] parts;
    SphereCollider coll;
    AudioSource sound;
    public float force = 2500f;
    float timer = 0f;

	// Use this for initialization
	void Start () {
        explosionGO = Instantiate(explosionGO, transform);
        parts = explosionGO.GetComponentsInChildren<ParticleSystem>();
        coll = GetComponent<SphereCollider>();

        if (GetComponent<AudioSource>())
            sound = GetComponent<AudioSource>();
	}

    void Explode()
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].Play();
        }
        sound.Play();        
        coll.enabled = false;
    }    

    void OnTriggerEnter(Collider other)
    {
        other.GetComponentInParent<Rigidbody>().AddExplosionForce(force, transform.position, 10f, 5f, ForceMode.Impulse);
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
