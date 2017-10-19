using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour {

    public GameObject pauseIndicator;

	// Use this for initialization
	void Start () {
        pauseIndicator.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.Escape))
        {
            if (!pauseIndicator.activeInHierarchy)
            {
                Pause();
            }
            else if(pauseIndicator.activeInHierarchy)
            {
                unPause();
            }
        }
	}

    private void Pause()
    {
        Time.timeScale = 0;
        pauseIndicator.SetActive(true);
    }
    private void unPause()
    {
        Time.timeScale = 0;
        pauseIndicator.SetActive(false);
    }
}
