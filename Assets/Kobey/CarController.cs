using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
public class CarController : MonoBehaviour {

    GamePadState state;
    GamePadState prev;
    PlayerIndex Pidx = PlayerIndex.One;
    public int PlayerNum;
    public float MotorTorque;
    public float SteeringAngle;
    public float BrakingTorque;
    public float ReverseTorque;
    public List<AxleInfo> axleInfo;
    private float maxMotorTorque;
    private float maxSteeringAngle;
    private float maxBrakingTorque;
    private float maxReverseTorque;
    Rigidbody rb;
    public int Gear = 0;
    public float speed;
    public float AntiRool = 2000;
    public Transform centerOfGravity;
    WheelFrictionCurve DriftFriction;
    WheelFrictionCurve DefaultFriction;

    public GameObject[] brakeLights;

    public int drivingState;

    public float Motor;
    // Use this for initialization
    void Start ()
    {
        switch (PlayerNum)
        {
            case 1:
                Pidx = PlayerIndex.One;
                break;
            case 2:
                Pidx = PlayerIndex.Two;
                break;
            case 3:
                Pidx = PlayerIndex.Three;
                break;
            case 4:
                Pidx = PlayerIndex.Four;
                break;
        }
        


        foreach (AxleInfo axleInfo in axleInfo)
        {
            if(axleInfo.motor)
            {
                DefaultFriction = axleInfo.LeftWheel.forwardFriction;
                DriftFriction = axleInfo.LeftWheel.forwardFriction;
            }
        }
        DriftFriction.stiffness = 1;
        DriftFriction.extremumSlip = 5;
        DriftFriction.extremumValue = 1;
        DriftFriction.asymptoteSlip = 4;
        DriftFriction.asymptoteValue = 1;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfGravity.localPosition;

        //DriftFriction.stiffness = 0.5f;
        //DriftFriction.extremumSlip = 0.2f;
        //DriftFriction.extremumValue = 1;
        //DriftFriction.asymptoteSlip = 0.1f;
        //DriftFriction.asymptoteValue = 1;


        maxMotorTorque = MotorTorque;
        maxSteeringAngle = SteeringAngle;
        maxBrakingTorque = BrakingTorque;
        maxReverseTorque = ReverseTorque;

}
    public void ApplyWheelRotation(WheelCollider collider)
    {

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        visualWheel.transform.position = pos;
        visualWheel.transform.localRotation = Quaternion.Euler(0, collider.steerAngle + 90, collider.rpm * Mathf.PI);
    }

    void UpdateDriveMode()
    {
        switch (drivingState)
        {
            case 0:
                for(int i =0; i < axleInfo.Count; i++)
                {
                    if(axleInfo[i].steering)
                    {
                        axleInfo[i].motor = false;
                    }
                    axleInfo[i].LeftWheel.suspensionDistance = 0.5f;
                    axleInfo[i].RightWheel.suspensionDistance = 0.5f;
                    axleInfo[i].LeftWheel.forwardFriction = DefaultFriction;
                    axleInfo[i].RightWheel.forwardFriction = DefaultFriction;
                   
                    maxMotorTorque = MotorTorque;
                }
                break;
            case 1:
                for (int i = 0; i < axleInfo.Count; i++)
                {
                    
                    axleInfo[i].LeftWheel.suspensionDistance = 1;
                    axleInfo[i].RightWheel.suspensionDistance = 1;
                    axleInfo[i].LeftWheel.forwardFriction = DriftFriction;
                    axleInfo[i].RightWheel.forwardFriction = DriftFriction;
                  
                    maxMotorTorque = MotorTorque * .01f;
                }
                break;
        }
        
    }

    void updateLights(bool onOff)
    {
        if(!onOff)
        {
            for (int i = 0; i < brakeLights.Length; i++)
            {
                if (brakeLights[i].activeSelf)
                {
                    brakeLights[i].SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < brakeLights.Length; i++)
            {
                if (!brakeLights[i].activeSelf)
                {
                    brakeLights[i].SetActive(true);
                }
            }
        }

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
            switch (Gear)
            {
                case 0:
                    updateLights(false);
                    break;
                case 1:
                    updateLights(true);
                    break;
            }
            
        }
        if (prev.Buttons.RightShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Pressed)
        {
            drivingState++;
            if (drivingState >= 2)
            {
                drivingState = 0;
            }
           // UpdateDriveMode();
        }
        Gear = Mathf.Clamp(Gear, 0, 1);
        drivingState = Mathf.Clamp(drivingState, 0, 1);
    }

    public float brake;
    public bool CheckGround = true;
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
            if(CheckGround)
            {
                axleInfo.isAxleGrounded = axleInfo.LeftWheel.isGrounded && axleInfo.RightWheel.isGrounded;
            }
           
            
           
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

                if (Motor > 0 || !axleInfo.RightWheel.isGrounded)
                {
                    rb.drag = 0;
                    axleInfo.LeftWheel.wheelDampingRate = 0.25f;
                    axleInfo.RightWheel.wheelDampingRate = 0.25f;
                }
                else if(axleInfo.RightWheel.isGrounded)
                {
                    rb.drag = 1.5f;
                    axleInfo.LeftWheel.wheelDampingRate = 200;
                    axleInfo.RightWheel.wheelDampingRate = 200;
                }

                if ( state.Buttons.RightShoulder == ButtonState.Pressed)
                {
                    axleInfo.LeftWheel.forwardFriction = DriftFriction;
                    axleInfo.RightWheel.forwardFriction = DriftFriction;
                    axleInfo.LeftWheel.sidewaysFriction = DriftFriction;
                    axleInfo.RightWheel.sidewaysFriction = DriftFriction;
                    
                }
                else
                {
                    axleInfo.LeftWheel.forwardFriction = DefaultFriction;
                    axleInfo.RightWheel.forwardFriction = DefaultFriction;
                    axleInfo.LeftWheel.sidewaysFriction = DefaultFriction;
                    axleInfo.RightWheel.sidewaysFriction = DefaultFriction;
                }
                

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
        public bool isAxleGrounded;
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
