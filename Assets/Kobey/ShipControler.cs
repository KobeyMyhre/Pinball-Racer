using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
public class ShipControler : MonoBehaviour {

    GamePadState state;
    GamePadState prev;
    PlayerIndex Pidx = PlayerIndex.One;

    public float maxMotorTorque;
    public float maxSteeringAngle;
    private float maxBrakingTorque;
    public float maxReverseTorque;
    Rigidbody rb;
    public int Gear = 0;
    float maxRot;
    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        prev = state;
        state = GamePad.GetState(Pidx);

        if (prev.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed)
        {
            Gear++;
            if (Gear >= 2)
            {
                Gear = 0;
            }
        }
        Gear = Mathf.Clamp(Gear, 0, 1);
    }

    void FixedUpdate()
    {


        //float Motor = maxMotorTorque * Input.GetAxis("Vertical");
        //float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        float Motor = maxMotorTorque * state.Triggers.Right;
        float steering = maxSteeringAngle * state.ThumbSticks.Left.X;
        float reverse = maxReverseTorque * -state.Triggers.Right;

        maxBrakingTorque = rb.mass * rb.velocity.magnitude;
        float brake = maxBrakingTorque * state.Triggers.Left;

        if(Gear == 1)
        {
            rb.AddForce(transform.forward * -Motor);
        }
        if(Gear == 0)
        {
            rb.AddForce(transform.forward * Motor);
        }
        rb.AddForce(transform.forward * brake);

        maxRot += steering;
        maxRot = Mathf.Clamp(maxRot, -45, 45);
        
        transform.Rotate(0,maxRot , 0);

       // rb.MoveRotation()

        







    }

    }


