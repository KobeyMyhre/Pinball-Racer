using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIbar : MonoBehaviour {

    public CarController player;
    public Slider Acelleration;
    public Text gear;
	// Use this for initialization
	void Start ()
    {
        player = FindObjectOfType<CarController>().GetComponent<CarController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (player.Gear)
        {
            case 0:
                gear.text = "D";
                break;
            case 1:
                gear.text = "R";
                break;
        }

        //Acelleration.value = player.Motor / player.maxMotorTorque;
	}
}
