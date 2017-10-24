using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class ChangeMusic : MonoBehaviour {

    public AudioSource thisfuckingplayer;
    private AudioClip[] audioFiles;
    //private AudioSource[] audioPlayer;
    int i = 0;
    void Start()
    {
        //audioPlayer = GetComponents<AudioSource>();
        audioFiles = Resources.LoadAll<AudioClip>("Audio/Audio");
        while(i < audioFiles.Length)
        {
            Debug.Log(audioFiles[i].name);
            i++;
        }
        //audioPlayer = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if(!thisfuckingplayer.isPlaying)
        {
            i = Random.Range(0, audioFiles.Length);
            Debug.Log("Now playing" + audioFiles[i].name);
            thisfuckingplayer.clip = audioFiles[i];
            thisfuckingplayer.Play();
            //audioPlayer[i].Play();

        }
    }
}
