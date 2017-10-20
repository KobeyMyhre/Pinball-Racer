using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour {

    public GameObject boostGO;
    ParticleSystem[] parts;
    BoxCollider coll;
    float timer = 0f;
    public float boostAmount = 5000f;
    AudioSource yeehaw;

    // Use this for initialization
    void Start()
    {
        boostGO = Instantiate(boostGO, transform);
        parts = boostGO.GetComponentsInChildren<ParticleSystem>();
        coll = GetComponent<BoxCollider>();
        yeehaw = GetComponent<AudioSource>();
    }

    void EffectsPlay()
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].Play();
        }
        yeehaw.Play();
        coll.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        other.GetComponentInParent<Rigidbody>().AddForce(transform.forward * boostAmount, ForceMode.Impulse);
        EffectsPlay();
    }

    // Update is called once per frame
    void Update()
    {
        if (coll.enabled == false)
        {
            timer += Time.deltaTime;
            if (timer >= 2f)
            {
                coll.enabled = true;
                timer = 0f;
            }
        }
    }
}
