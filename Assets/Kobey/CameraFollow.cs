using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public Transform childPos;
    CarController car;
    bool FreeForm = false;

    //bool isWheelOneGrounded;
    //bool isWheelTwoGrounded;

    //bool areWheelsGrounded
    //{
    //    get
    //    {
    //        return isWheelOneGrounded && isWheelTwoGrounded;
    //    }
    //}

    //private Vector3 offset;
    //private Vector3 angleOffset;
    void Start()
    {
        car = player.GetComponent<CarController>();
        //offset = transform.position - player.transform.position;
        //angleOffset = transform.rotation.eulerAngles - player.transform.rotation.eulerAngles;
    }

    void FreeFormCamera()
    {
        if(gameObject.transform.parent != null)
        {
            gameObject.transform.parent = null;
        }
        transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0, 25, 0), Time.deltaTime/3);
        transform.LookAt(player.transform);
    }

    void SnapToCamera()
    {
        gameObject.transform.parent = player.transform;
        transform.position = childPos.position;
        transform.rotation = childPos.rotation;
    }

    

    void LateUpdate()
    {
       //transform.position = player.transform.position + offset;
       // float offsetAngle = player.GetComponent<CarController>().axleInfo[1].LeftWheel.steerAngle;
       // // transform.localRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + offsetAngle, transform.rotation.z);
       // transform.rotation = Quaternion.Euler(player.transform.rotation.eulerAngles + offset);

        //for(int i = 0; i < car.axleInfo.Count; i++)
        //{
            
        //}

        if(!car.axleInfo[0].isAxleGrounded && !car.axleInfo[1].isAxleGrounded )
        {
             FreeFormCamera();
           // Invoke("FreeFormCamera", 1);
        }
        else
        {
            //if(IsInvoking("FreeFormCamrea"))
            //{
            //    CancelInvoke();
            //}
            SnapToCamera();
        }

    }
}
