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
    public float speed;
    public float AntiRool = 2000;

    WheelFrictionCurve DriftFriction;
    WheelFrictionCurve DefaultFriction;


    // Use this for initialization
    void Start ()
    {
        foreach (AxleInfo axleInfo in axleInfo)
        {
            if(axleInfo.motor)
            {
                DefaultFriction = axleInfo.LeftWheel.forwardFriction;
                DriftFriction = axleInfo.LeftWheel.forwardFriction;
            }
        }
        DriftFriction.stiffness = 2;
        DriftFriction.extremumSlip = 1.4f;
        DriftFriction.extremumValue = 1.3f;
        DriftFriction.asymptoteSlip = .4f;
        DriftFriction.asymptoteValue = .5f;
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
            DoRollBar(axleInfo.LeftWheel, axleInfo.RightWheel);
            if(axleInfo.steering)
            {
               
                    axleInfo.LeftWheel.steerAngle = steering;
                    axleInfo.RightWheel.steerAngle = steering;
               
              
                
            }
            if(axleInfo.motor)
            {
                if (state.Buttons.RightShoulder == ButtonState.Pressed)
                {

                    axleInfo.LeftWheel.forwardFriction = DriftFriction;
                    axleInfo.RightWheel.forwardFriction = DriftFriction;
                    //maxBrakingTorque = rb.mass * rb.velocity.magnitude;
                    //float braker = maxBrakingTorque * state.Triggers.Left;

                    //axleInfo.LeftWheel.brakeTorque = braker/2;
                    //axleInfo.RightWheel.brakeTorque = braker/2;
                }
                else
                {
                    axleInfo.LeftWheel.forwardFriction = DefaultFriction;
                    axleInfo.RightWheel.forwardFriction = DefaultFriction;
                }

                //if (state.Triggers.Left >= 0.8f)
                //{

                //    axleInfo.LeftWheel.forwardFriction = DriftFriction;
                //    axleInfo.RightWheel.forwardFriction = DriftFriction;
                //}
                //else
                //{
                //    axleInfo.LeftWheel.forwardFriction = DefaultFriction;
                //    axleInfo.RightWheel.forwardFriction = DefaultFriction;
                //}
                
                if (Gear == 0)
                {
                    
                    axleInfo.LeftWheel.motorTorque = Motor * speed;
                    axleInfo.RightWheel.motorTorque = Motor * speed;
                }
                
                if (Gear == 1)
                {
                    axleInfo.LeftWheel.motorTorque = reverse;
                    axleInfo.RightWheel.motorTorque = reverse;
                }
            }



            //maxBrakingTorque = rb.mass * rb.velocity.magnitude;
            //float brake = maxBrakingTorque * state.Triggers.Left;

            //axleInfo.LeftWheel.brakeTorque = brake;
            //axleInfo.RightWheel.brakeTorque = brake;
            

           

            //ApplyWheelRotation(axleInfo.LeftWheel);
            //ApplyWheelRotation(axleInfo.RightWheel);
        }

	}

    void DoRollBar(WheelCollider wheelL, WheelCollider wheelR)
    {
        WheelHit hit;
        float travelL = 1;
        float travelR = 1;

        bool groundedL = wheelL.GetGroundHit(out hit);
        if (groundedL)
        {
            travelL = (-wheelL.transform.InverseTransformPoint(hit.point).y - wheelL.radius) / wheelL.suspensionDistance;
        }
        bool groundedR = wheelR.GetGroundHit(out hit);
        if (groundedR)
        {
            travelR = (-wheelR.transform.InverseTransformPoint(hit.point).y - wheelR.radius) / wheelR.suspensionDistance;
        }
        float antiRollForce = (travelL - travelR) * AntiRool;

        if (groundedL)
        {
            rb.AddForceAtPosition(wheelL.transform.up * -antiRollForce, wheelL.transform.position);
        }
        if (groundedR)
        {
            rb.AddForceAtPosition(wheelR.transform.up * -antiRollForce, wheelR.transform.position);
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
