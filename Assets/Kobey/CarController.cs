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
    public float maxBrakingTorque;
    public float maxReverseTorque;
    Rigidbody rb;
    public int Gear = 0;
    public float speed;
    public float AntiRool = 2000;
    public Transform centerOfGravity;
    WheelFrictionCurve DriftFriction;
    WheelFrictionCurve DefaultFriction;

    public float Motor;
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
        DriftFriction.stiffness = 4;
        DriftFriction.extremumSlip = 4;
        DriftFriction.extremumValue = 1;
        DriftFriction.asymptoteSlip = 2;
        DriftFriction.asymptoteValue = 1;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfGravity.localPosition;
    }
    public void ApplyWheelRotation(WheelCollider collider)
    {

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        visualWheel.transform.position = pos;
        visualWheel.transform.localRotation = Quaternion.Euler(collider.rpm * Mathf.PI, collider.steerAngle, 0);
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

    public float brake;
    // Update is called once per frame
    void FixedUpdate ()
    {
        
       
        //float Motor = maxMotorTorque * Input.GetAxis("Vertical");
        //float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        Motor = maxMotorTorque * state.Triggers.Right;
        float steering = maxSteeringAngle * state.ThumbSticks.Left.X;
        float reverse = maxReverseTorque * -state.Triggers.Right;
        maxBrakingTorque = rb.mass * rb.velocity.magnitude;
        brake = maxBrakingTorque * state.Triggers.Left;
        foreach (AxleInfo axleInfo in axleInfo)
        {
            
            DoRollBar(axleInfo.LeftWheel, axleInfo.RightWheel);
            if (state.Triggers.Left == 1)
            {
                
                axleInfo.LeftWheel.brakeTorque = brake * 50;
                axleInfo.RightWheel.brakeTorque = brake * 50;
                if (axleInfo.motor)
                {

                    axleInfo.DoTrail(true);
                }
        }
            else
            {
                if (axleInfo.motor)
                {
                    axleInfo.DoTrail(false);

                }
                axleInfo.LeftWheel.brakeTorque = 0;
                axleInfo.RightWheel.brakeTorque = 0;
            }
            if (axleInfo.steering)
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
            
            ApplyWheelRotation(axleInfo.LeftWheel);
            ApplyWheelRotation(axleInfo.RightWheel);

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
        public TrailRenderer Ltrail;
        public TrailRenderer Rtrail;
        public WheelCollider LeftWheel;
        public WheelCollider RightWheel;
        public bool motor;
        public bool steering;
        float timer = 1;
        public void DoTrail(bool OnOff)
        {
            if(OnOff)
            {
                timer = 1;
                if (LeftWheel.isGrounded)
                {
                    Ltrail.enabled = true;
                }
                else
                {
                  
                    Ltrail.enabled = false;
                }
                if (RightWheel.isGrounded)
                {
                   
                    Rtrail.enabled = true;
                }
                else
                {
                   
                    Rtrail.enabled = false;
                }
            }
            else
            {
                //if(Ltrail.enabled == true)
                //{
                //    var SpawnLine = Instantiate(Ltrail);
                //    SpawnLine.transform.position = Ltrail.transform.localPosition;



                //    Ltrail.enabled = false;
                //}
                Ltrail.enabled = false;
                Rtrail.enabled = false;

            }
           
        }
    }

}
