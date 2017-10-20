using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ChangeMusic : MonoBehaviour {
    public AudioSource thisfuckingplayer;
    private AudioClip[] audioFiles;
    private AudioSource[] audioPlayer;
    int i = 0;
    void Start()
    {
        audioPlayer = GetComponents<AudioSource>();
        //string[] FilePaths = Directory.GetFiles(Application.dataPath + "/RaphaelStuff/Audio");
        //int iterate = FilePaths.Length;
        //int i = 0;
        //while (i <= iterate)
        //{
        //    audioFiles[i] = AssetDatabase.LoadAssetAtPath<AudioClip>(FilePaths[i]);
        //}
        //audioPlayer = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if(!audioPlayer[i].isPlaying)
        {
            i = Random.Range(0, audioPlayer.Length);
            //thisfuckingplayer.clip = audioFiles[i];
            //thisfuckingplayer.Play();
            audioPlayer[i].Play();

        }
    }
}
