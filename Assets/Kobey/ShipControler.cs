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

    public float drift;

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
    float Rot = 0;


    Vector3 ForwardVel()
    {
        return transform.forward * Vector3.Dot(rb.velocity, transform.forward);
    }
    Vector3 RightVel()
    {
        return transform.right * Vector3.Dot(rb.velocity, transform.right);
    }

    void FixedUpdate()
    {


        //float Motor = maxMotorTorque * Input.GetAxis("Vertical");
        //float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        float Motor = maxMotorTorque * state.Triggers.Right;
        float steering = maxSteeringAngle * state.ThumbSticks.Left.X;
        float reverse = maxReverseTorque * rb.velocity.magnitude;

        maxBrakingTorque = rb.mass * rb.velocity.magnitude;
        float brake = maxBrakingTorque * -state.Triggers.Left;

        if(Gear == 1)
        {
            rb.AddForce(transform.forward * -Motor);
        }
        if(Gear == 0)
        {
            rb.AddForce(transform.forward * Motor);
        }
        if(state.Triggers.Left ==1)
        {
            rb.AddForce(transform.forward * brake);
        }

        Rot += steering;
        Rot = Mathf.Clamp(Rot, transform.forward.y - 45, transform.forward.y + 45);
        Quaternion thing = Quaternion.Euler( new Vector3(transform.localEulerAngles.x,Rot, transform.localEulerAngles.z));
        
        rb.rotation = thing;

        rb.velocity = ForwardVel() + RightVel() * drift;
        rb.angularVelocity = new Vector3( 0,steering,0);
        



       // rb.MoveRotation()

        







    }

    }


