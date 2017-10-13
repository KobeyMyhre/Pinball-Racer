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
    
    public float applyDrag;
    Rigidbody rb;
    public int Gear = 0;
    public float Offset;
    public float RotOffset;
    Vector3 wantedPos;
    public float drift;

    // Use this for initialization
    void Start ()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 50))
        {
            if (hit.collider.tag == "floor")
            {
                Offset = (transform.position.y - hit.collider.transform.position.y);
                
            }

        }
        rb = GetComponent<Rigidbody>();
    }

    float AngleBetween(Vector3 hitPos, Vector3 Tpos)
    {

        return Mathf.Cos(Vector3.Dot(hitPos, Tpos) / (hitPos.magnitude * Tpos.magnitude));

       
    }

    // Update is called once per frame
    void Update ()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 50))
        {
            if (hit.collider.tag == "floor")
            {
                wantedPos = new Vector3(transform.position.x, hit.point.y + Offset, transform.position.z);
                RotOffset = hit.collider.transform.rotation.x;
                //transform.forward = (transform.position - hit.point).normalized;
                // transform.LookAt(hit.collider.transform.up * Offset);

            }

        }


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
       


        float y = Mathf.Lerp(  transform.position.y, wantedPos.y, Time.deltaTime * 1.2f);
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
        

        float Motor = maxMotorTorque * state.Triggers.Right;
        float steering = maxSteeringAngle * state.ThumbSticks.Left.X;
       

        

        
        if(state.Triggers.Left >= 0.9)
        {
            rb.drag += applyDrag * Time.deltaTime;
        }
        else { rb.drag = 0; }

       
        rb.AddForce(transform.forward * Motor);
        rb.velocity = ForwardVel() + RightVel() * drift;
        rb.angularVelocity = new Vector3(0,steering,0);
        



       

        







    }

    }


