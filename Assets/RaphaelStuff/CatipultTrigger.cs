using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatipultTrigger : MonoBehaviour {

    public GameObject Spring;
    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.name);
        if (Spring.GetComponent<CatipultEvent>().fired == false && 
            Spring.GetComponent<CatipultEvent>().check == true)
        {
            Spring.GetComponent<CatipultEvent>().fired = true;
            Spring.GetComponent<CatipultEvent>().check = false;
        }
    }
}
