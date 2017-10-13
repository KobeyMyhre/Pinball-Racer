using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
public class UpdatedCarController : MonoBehaviour {
    GamePadState state;
    GamePadState prev;
    PlayerIndex Pidx = PlayerIndex.One;


    public float idealRPM = 500;
    public float maxRPM = 1000;

    public Transform centerOfGravity;
    public List<AxleInfo> axleInfo;

    public float turnRadius = 6;
    public float torque = 25;
    public float brakeTorque = 100f;

    public float AntiRool = 2000;

    Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfGravity.localPosition;
	}


    private void Update()
    {
        prev = state;
        state = GamePad.GetState(Pidx);
    }

    // Update is called once per frame
    void FixedUpdate ()
    {


        float scaledTorque = state.Triggers.Right * torque;

        float scaledTurn = state.ThumbSticks.Left.X * turnRadius;



        


        foreach (AxleInfo axleInfo in axleInfo)
        {
            DoRollBar(axleInfo.LeftWheel, axleInfo.RightWheel);
            if(axleInfo.motor)
            {
                if(axleInfo.LeftWheel.rpm < idealRPM)
                {
                    scaledTorque = Mathf.Lerp(scaledTorque / 10, scaledTorque, axleInfo.LeftWheel.rpm / idealRPM);
                }
                else
                {
                    scaledTorque = Mathf.Lerp(scaledTorque, 0, (axleInfo.LeftWheel.rpm - idealRPM) / (maxRPM - idealRPM));
                }
                axleInfo.LeftWheel.motorTorque = scaledTorque;
                axleInfo.RightWheel.motorTorque = scaledTorque;
            }
            if (axleInfo.steering)
            {
                axleInfo.LeftWheel.steerAngle = scaledTurn;
                axleInfo.RightWheel.steerAngle = scaledTurn;
                if (state.Triggers.Left == 1)
                {
                    axleInfo.RightWheel.brakeTorque = brakeTorque;
                    axleInfo.LeftWheel.brakeTorque = brakeTorque;
                }
                else
                {
                    axleInfo.RightWheel.brakeTorque = 0;
                    axleInfo.LeftWheel.brakeTorque = 0;
                }
            }
        }



    }

    void DoRollBar(WheelCollider wheelL, WheelCollider wheelR)
    {
        WheelHit hit;
        float travelL = 1;
        float travelR = 1;

        bool groundedL = wheelL.GetGroundHit(out hit);
        if(groundedL)
        {
            travelL = (-wheelL.transform.InverseTransformPoint(hit.point).y - wheelL.radius) / wheelL.suspensionDistance;
        }
        bool groundedR = wheelR.GetGroundHit(out hit);
        if (groundedR)
        {
            travelR = (-wheelR.transform.InverseTransformPoint(hit.point).y - wheelR.radius) / wheelR.suspensionDistance;
        }
        float antiRollForce = (travelL - travelR) * AntiRool;

        if(groundedL)
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
