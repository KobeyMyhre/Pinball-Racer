using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAndDestroy : MonoBehaviour {

    public GameObject partsGO;
    ParticleSystem[] parts;
    CapsuleCollider coll;
    AudioSource sound;

    // Use this for initialization
    void Start()
    {
        partsGO = Instantiate(partsGO, transform);
        parts = partsGO.GetComponentsInChildren<ParticleSystem>();
        coll = GetComponent<CapsuleCollider>();
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
        Destroy(this, 1f);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.root.tag == "Player")
            EffectsPlay();
    }
}
