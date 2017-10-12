using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
public class CarController : MonoBehaviour {

    GamePadState state;
    GamePadState prev;
    PlayerIndex Pidx = PlayerIndex.One;


    public List<AxleInfo> axleInfo;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    private float maxBrakingTorque;
    public float maxReverseTorque;
    Rigidbody rb;
    public int Gear = 0;
	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
    public void ApplyWheelRotation(WheelCollider collider)
    {
        Transform visualWheel = collider.transform;

        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        //visualWheel.transform.position = pos;
        visualWheel.transform.rotation = rot;
    }


    private void Update()
    {
        prev = state;
        state = GamePad.GetState(Pidx);

        if(prev.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed)
        {
            Gear++;
            if(Gear >= 2)
            {
                Gear = 0;
            }
        }
        Gear = Mathf.Clamp(Gear, 0, 1);

    }


    // Update is called once per frame
    void FixedUpdate ()
    {
        

        //float Motor = maxMotorTorque * Input.GetAxis("Vertical");
        //float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        float Motor = maxMotorTorque * state.Triggers.Right;
        float steering = maxSteeringAngle * state.ThumbSticks.Left.X;
        float reverse = maxReverseTorque * -state.Triggers.Right;
        foreach (AxleInfo axleInfo in axleInfo)
        {
            if(axleInfo.steering)
            {
               
                    axleInfo.LeftWheel.steerAngle = steering;
                    axleInfo.RightWheel.steerAngle = steering;
               
              
                
            }
            if(axleInfo.motor)
            {
                if (Gear == 0)
                {
                    axleInfo.LeftWheel.motorTorque = Motor;
                    axleInfo.RightWheel.motorTorque = Motor;
                }
                if (Gear == 1)
                {
                    axleInfo.LeftWheel.motorTorque = reverse;
                    axleInfo.RightWheel.motorTorque = reverse;
                }
            }

          
           
            maxBrakingTorque = rb.mass * rb.velocity.magnitude;
            float brake = maxBrakingTorque * state.Triggers.Left;

            axleInfo.LeftWheel.brakeTorque = brake;
            axleInfo.RightWheel.brakeTorque = brake;
            

           

            ApplyWheelRotation(axleInfo.LeftWheel);
            ApplyWheelRotation(axleInfo.RightWheel);
        }

	}

    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider LeftWheel;
        public WheelCollider RightWheel;
        public bool motor;
        public bool steering;

    }

}
