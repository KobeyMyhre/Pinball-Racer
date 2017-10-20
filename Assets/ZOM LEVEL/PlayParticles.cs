using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticles : MonoBehaviour {

    public GameObject partsGO;
    ParticleSystem[] parts;
    BoxCollider coll;
    AudioSource sound;

    // Use this for initialization
    void Start()
    {
        partsGO = Instantiate(partsGO, transform);
        parts = partsGO.GetComponentsInChildren<ParticleSystem>();
        coll = GetComponent<BoxCollider>();
        if (GetComponent<AudioSource>())
            sound = GetComponent<AudioSource>();
    }

    void EffectsPlay()
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].Play();
        }

        if (sound)
            sound.Play();

        coll.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            EffectsPlay();
    }
}
