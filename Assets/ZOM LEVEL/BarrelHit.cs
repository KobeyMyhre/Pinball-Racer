using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelHit : MonoBehaviour {

    public GameObject partsGO;
    ParticleSystem[] parts;
    SphereCollider coll;
    AudioSource sound;
    float timer = 0f;

    // Use this for initialization
    void Start()
    {
        partsGO = Instantiate(partsGO, transform);
        parts = partsGO.GetComponentsInChildren<ParticleSystem>();
        coll = GetComponent<SphereCollider>();
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
        if (other.transform.root.tag == "Player")
            EffectsPlay();
    }

    void Update()
    {
        if (coll.enabled == false)
        {
            timer += Time.deltaTime;
            if (timer >= 3f)
            {
                coll.enabled = true;
                timer = 0f;
            }
        }
    }
}
