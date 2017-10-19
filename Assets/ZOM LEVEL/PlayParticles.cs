using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticles : MonoBehaviour {

    public GameObject partsGO;
    ParticleSystem[] parts;
    BoxCollider coll;    

    // Use this for initialization
    void Start()
    {
        partsGO = Instantiate(partsGO, transform);
        parts = partsGO.GetComponentsInChildren<ParticleSystem>();
        coll = GetComponent<BoxCollider>();
    }

    void ParticlePlay()
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].Play();
        }
        coll.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            ParticlePlay();
    }
}
